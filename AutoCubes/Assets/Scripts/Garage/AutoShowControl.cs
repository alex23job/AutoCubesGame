using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoShowControl : MonoBehaviour
{
    [SerializeField] private Button btnBuy;
    [SerializeField] private Text txtExp;
    [SerializeField] private Text txtGold;
    [SerializeField] private GameObject[] itemPanels;

    private Animator anim;
    private bool isRotate = false;
    private int currentViewCar = -1;
    private bool isCarAdding = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NextCar();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BoardRotate()
    {
        isRotate = !isRotate;
        anim.SetBool("IsRotate", isRotate);
    }

    public void NextCar()
    {
        if (currentViewCar != -1) transform.GetChild(currentViewCar + 1).gameObject.SetActive(false);
        currentViewCar++;
        currentViewCar %= 7;
        transform.GetChild(currentViewCar + 1).gameObject.SetActive(true);
        CarPassport carPassport = transform.GetChild(currentViewCar + 1).gameObject.GetComponent<CarPassport>();
        if (carPassport != null)
        {
            if ((carPassport.ExpForSale <= GameManager.Instance.currentPlayer.totalScore) && (carPassport.PriceCar <= GameManager.Instance.currentPlayer.totalGold))
            {
                btnBuy.interactable = true;
            }
            else { btnBuy.interactable = false; }
            ViewParams(carPassport);
        }
    }

    private void ViewParams(CarPassport carPassport)
    {
        string lang = Language.Instance.CurrentLanguage;
        ViewItemPanel(itemPanels[0], PrefabsPak.Instance.GetItemName(0, lang), PrefabsPak.Instance.GetCarName(carPassport.CarID - 1, lang));
        ViewItemPanel(itemPanels[1], PrefabsPak.Instance.GetItemName(1, lang), PrefabsPak.Instance.GetBoxType(carPassport.BoxType, lang));
        ViewItemPanel(itemPanels[2], PrefabsPak.Instance.GetItemName(2, lang), $"{carPassport.MaxVelocity}");
        ViewItemPanel(itemPanels[3], PrefabsPak.Instance.GetItemName(3, lang), $"{carPassport.MaxCeilOrders}");
        ViewItemPanel(itemPanels[4], PrefabsPak.Instance.GetItemName(4, lang), $"{carPassport.RemainingTrips}");
        txtExp.text = $"{carPassport.ExpForSale}/{GameManager.Instance.currentPlayer.totalScore}";
        txtExp.color = (carPassport.ExpForSale <= GameManager.Instance.currentPlayer.totalScore) ? Color.green : Color.red;
        txtGold.text = $"{carPassport.PriceCar}/{GameManager.Instance.currentPlayer.totalGold}";
        txtGold.color = (carPassport.PriceCar <= GameManager.Instance.currentPlayer.totalGold) ? Color.green : Color.red;
    }

    private void ViewItemPanel(GameObject panel, string name, string value)
    {
        Text txtName = panel.transform.GetChild(0).gameObject.GetComponent<Text>();
        Text txtValue = panel.transform.GetChild(1).gameObject.GetComponent<Text>();
        txtName.text = name;
        txtValue.text = value;
    }

    public void OnButtonBuyClick()
    {
        CarPassport carPassport = transform.GetChild(currentViewCar + 1).gameObject.GetComponent<CarPassport>();
        //GameManager.Instance.currentPlayer.totalGold -= carPassport.PriceCar;
        if (carPassport.PriceCar <= GameManager.Instance.currentPlayer.totalGold)
        {
            btnBuy.interactable = true;
        }
        else { btnBuy.interactable = false; }
        ViewGold(carPassport);
        PlayersGarage.Instance.AddCar(carPassport.CarID);
        isCarAdding = true;
    }

    public void LoadMenu()
    {
        if (isCarAdding) GameManager.Instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void ViewGold(CarPassport carPassport)
    {
        txtGold.text = $"{carPassport.PriceCar}/{GameManager.Instance.currentPlayer.totalGold}";
        txtGold.color = (carPassport.PriceCar <= GameManager.Instance.currentPlayer.totalGold) ? Color.green : Color.red;
    }
}
