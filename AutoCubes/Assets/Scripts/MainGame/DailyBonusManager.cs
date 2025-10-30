using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonusManager : MonoBehaviour
{
    [SerializeField] private Sprite[] bonusSprites;
    [SerializeField] private Text txtTime;
    [SerializeField] private Text txtDay;
    [SerializeField] private Text txtValue;
    [SerializeField] private GameObject[] itemPanels;
    [SerializeField] private Button btnAccept;
    [SerializeField] private Image imgBonus;
    [SerializeField] private MainMenuControl menuControl;

    private float timer = 0;
    private int oldDay;
    private Color baseColorItemPanel;
    // Типы бонусов
    public enum BonusType { Money = 0, Experience = 1, Car = 2 }

    // Класс бонуса
    [Serializable]
    public class Bonus
    {
        public BonusType type;
        public string value;
        public int count;
    }

    // Массив бонусов
    public Bonus[] bonuses = new Bonus[]
    {
        new Bonus { type = BonusType.Money, value = "+250", count = 250 },
        new Bonus { type = BonusType.Experience, value = "+200", count = 200 },
        new Bonus { type = BonusType.Money, value = "+500", count = 500 },
        new Bonus { type = BonusType.Experience, value = "+400", count = 400 },
        new Bonus { type = BonusType.Money, value = "+750", count = 750 },
        new Bonus { type = BonusType.Experience, value = "+700", count = 700 },
        new Bonus { type = BonusType.Car, value = "+1", count = 1}
    };

    // Последняя дата сбора бонуса
    private DateTime lastCollectionDate;
    private int currentBonusIndex = 0;

    void Start()
    {
        oldDay = DateTime.Now.Day;
        baseColorItemPanel = itemPanels[0].GetComponent<Image>().color;
        //print($"baseColorItemPanel=<{baseColorItemPanel}>");
        // Получаем последний собранный бонус (например, из PlayerPrefs)
        lastCollectionDate = DateTime.Now.Date;

        Invoke("GetGameManagerParams", 0.8f);
    }

    private void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            timer = 1f;
            DateTime timeEnd1 = DateTime.Now, timeEnd;
            if (timeEnd1.Day != oldDay)
            {
                oldDay = timeEnd1.Day;
                btnAccept.interactable = true;
                currentBonusIndex = (currentBonusIndex + 1) % bonuses.Length;
                ViewCurrentBonus();
            }
            timeEnd1 = timeEnd1.AddDays(1);
            timeEnd = new DateTime(timeEnd1.Year, timeEnd1.Month, timeEnd1.Day, 0, 0, 0);
            TimeSpan ostTime = timeEnd - DateTime.Now;
            //print($"timeEnd={timeEnd}  ostTime={ostTime}  time1={timeEnd1}");
            txtTime.text = $"{ostTime.Hours:00}:{ostTime.Minutes:00}:{ostTime.Seconds:00}";
        }
    }

    private void GetGameManagerParams()
    {
        //float deltaMinutes = Mathf.Abs((float)(lastCollectionDate - GameManager.Instance.currentPlayer.acceptBonusTime).TotalMinutes);
        double deltaMinutes = (float)(DateTime.Now - GameManager.Instance.currentPlayer.acceptBonusTime).TotalMinutes;
        print($"deltaMinutes={deltaMinutes}   last={lastCollectionDate}   acceptBonusTime={GameManager.Instance.currentPlayer.acceptBonusTime.Date}");
        if (deltaMinutes > 2f)
        {            
            lastCollectionDate = GameManager.Instance.currentPlayer.acceptBonusTime;            
            int deltaDay = Mathf.RoundToInt((float)(DateTime.Now.Date - lastCollectionDate.Date).TotalDays);
            //print($"deltaDay={deltaDay}     lastCollectionDate={lastCollectionDate}   numberBonusDay={GameManager.Instance.currentPlayer.numberBonusDay}");
            btnAccept.interactable = true;
            if (deltaDay < 1)
            {
                currentBonusIndex = GameManager.Instance.currentPlayer.numberBonusDay;
                btnAccept.interactable = false;
            }
            else if (deltaDay > 1)
            {
                currentBonusIndex = 0;
            }
            else
            {   //  deltaDay == 1
                currentBonusIndex = GameManager.Instance.currentPlayer.numberBonusDay;
                currentBonusIndex = (currentBonusIndex + 1) % bonuses.Length;
            }
        }
        ViewCurrentBonus();
        if (btnAccept.interactable)
        {
            btnAccept.GetComponent<PeriodicColorChange>().SetChange(true);
        }
    }

    public void TryCollectDailyBonus()
    {
        // Проверяем, прошел ли день с последнего сбора
        if ((DateTime.Now.Date - lastCollectionDate).TotalDays >= 1)
        {
            // Проверяем пропуск дней
            if (lastCollectionDate.Day != DateTime.Now.Day)
            {
                ResetBonuses(); // Сбрасываем на начало цикла
            }

            // Собираем текущий бонус
            CollectCurrentBonus();

            // Обновляем дату последнего сбора
            lastCollectionDate = DateTime.Now.Date;
        }
        else
        {
            Debug.Log("Ежедневный бонус уже собран сегодня!");
        }
    }

    private void ResetBonuses()
    {
        currentBonusIndex = 0;
    }

    private void CollectCurrentBonus()
    {
        // Берем текущий бонус
        Bonus bonus = bonuses[currentBonusIndex];

        // Обрабатываем бонус (например, добавляем деньги или опыт)
        ProcessBonus(bonus);

        // Переходим к следующему бонусу
        currentBonusIndex = (currentBonusIndex + 1) % bonuses.Length;
    }

    private void ProcessBonus(Bonus bonus)
    {
        Debug.Log($"Собран бонус: {bonus.type} ({bonus.value})");
        // Здесь можно добавить логику обработки бонуса (добавление денег, опыта и т.д.)
        switch (bonus.type)
        {
            case BonusType.Money:
                GameManager.Instance.currentPlayer.totalGold += bonus.count;
                menuControl.ViewRecord();
                break;
            case BonusType.Experience:
                GameManager.Instance.currentPlayer.totalScore += bonus.count;
                menuControl.ViewRecord();
                break;
            case BonusType.Car:
                PlayersGarage.Instance.AddCar(4);
                break;
        }
        //DateTime curDate = DateTime.Now;
        //GameManager.Instance.currentPlayer.acceptBonusTime = new DateTime(curDate.Year, curDate.Month, curDate.Day);
        GameManager.Instance.currentPlayer.acceptBonusTime = DateTime.Now.Date;
        GameManager.Instance.currentPlayer.numberBonusDay = currentBonusIndex;
        GameManager.Instance.SaveGame();
    }

    public void OnButtonAcceptBonusClick()
    {
        btnAccept.interactable = false;
        btnAccept.GetComponent<PeriodicColorChange>().SetChange(false);
        ProcessBonus(bonuses[currentBonusIndex]);
        //currentBonusIndex = (currentBonusIndex + 1) % bonuses.Length;
        //ViewCurrentBonus();
    }

    private void ViewCurrentBonus()
    {
        string lang = Language.Instance.CurrentLanguage;
        string day = (lang == "ru") ? "День " : "Day ";
        txtDay.text = $"{day}{currentBonusIndex + 1}";
        Bonus current = bonuses[currentBonusIndex];
        imgBonus.sprite = bonusSprites[(int)current.type];
        imgBonus.color = (current.type == BonusType.Experience) ? new Color(0, 0.7f, 0.4f, 1f) : Color.white;
        txtValue.text = current.value;
        for(int i = 0; i < itemPanels.Length; i++)
        {
            itemPanels[i].GetComponent<Image>().color = (i == currentBonusIndex) ? new Color(0.3f, 0.7f, 1f, baseColorItemPanel.a + 0.45f) : baseColorItemPanel;
        }
    }
}
