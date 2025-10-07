using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuControl : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetLeaderboardEntries();

    [SerializeField] private Image[] foneStoreButtons;
    [SerializeField] private Image[] foneRemovalButtons;
    [SerializeField] private Button[] storeLevelButtons;
    [SerializeField] private GameObject selectPanel;

    [SerializeField] private GameObject[] arRecItems;
    [SerializeField] private Image foneAvatar;
    [SerializeField] private RawImage riAvatar;
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtRecord;

    [SerializeField] private Image imgFone;
    [SerializeField] private Image imgProgress;
    [SerializeField] private Button btnPlay;

    private float timer = 5f;
    private bool isLoad = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectPanel.SetActive(false);
        txtName.text = "-----";
        txtRecord.text = "-----   -----";
        ViewRecord();
        ViewLeaderboard("");
        Invoke("GetLeaderboard", 0.02f);
        btnPlay.interactable = false;
        if (GameManager.Instance.currentPlayer.isLoaded) LoadComplete();
        Invoke("LoadComplete", 3f);
    }


    // Update is called once per frame
    void Update()
    {
        if (isLoad == false)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                imgProgress.fillAmount = (5f - timer) / 5f;
            }
            else
            {
                timer = 5f;
            }
        }
    }

    public void LoadComplete()
    {
        isLoad = true;
        if (GameManager.Instance.currentPlayer.isLoaded)
        btnPlay.interactable = true;
        imgFone.gameObject.SetActive(false);
        imgProgress.gameObject.SetActive(false);
        foneAvatar.gameObject.SetActive(true);
        //riAvatar.gameObject.SetActive(true);
        txtName.gameObject.SetActive(true);
        txtRecord.gameObject.SetActive(true);
    }

    public void ViewSelectGamePanel()
    {
        int i;
        GameManager.Instance.currentPlayer.currentLevel = GameManager.Instance.currentPlayer.maxLevel;
        for (i = 0; i < foneStoreButtons.Length; i++)
        {
            if (i == GameManager.Instance.currentPlayer.maxLevel - 1) foneStoreButtons[i].gameObject.SetActive(true);
            else foneStoreButtons[i].gameObject.SetActive(false);
            if (i < GameManager.Instance.currentPlayer.maxLevel)
            {
                storeLevelButtons[i].interactable = true;
            }
            else
            {
                storeLevelButtons[i].interactable = false;
            }
        }
        GameManager.Instance.currentPlayer.numBoxRemoval = 0;
        for (i = 0; i < foneRemovalButtons.Length; i++)
        {
            if (i == 0) foneRemovalButtons[i].gameObject.SetActive(true);
            else foneRemovalButtons[i].gameObject.SetActive(false);
        }
        selectPanel.SetActive(true);
    }

    public void OnBtnLevelClick(int lvl)
    {
        foneStoreButtons[GameManager.Instance.currentPlayer.currentLevel - 1].gameObject.SetActive(false);
        GameManager.Instance.currentPlayer.currentLevel = lvl;
        foneStoreButtons[GameManager.Instance.currentPlayer.currentLevel - 1].gameObject.SetActive(true);
    }

    public void OnBtnRemovalClick(int numBox)
    {
        foneRemovalButtons[GameManager.Instance.currentPlayer.numBoxRemoval].gameObject.SetActive(false);
        GameManager.Instance.currentPlayer.numBoxRemoval = numBox;
        GameManager.Instance.currentPlayer.countSecondRemoval = 150 + numBox * 30;
        foneRemovalButtons[GameManager.Instance.currentPlayer.numBoxRemoval].gameObject.SetActive(true);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void LoadRemoval()
    {
        if (PlayersGarage.Instance.CountCars > 0) PlayersGarage.Instance.DropAllPlayerCars();
        SceneManager.LoadScene("RemovalScene");
    }

    public void LoadGarage()
    {
        SceneManager.LoadScene("GaragScene");
    }

    public void LoadAutoShow()
    {
        SceneManager.LoadScene("AutoShowScene");
    }

    public void ViewAvatar()
    {
        txtName.text = GameManager.Instance.currentPlayer.playerName;
        riAvatar.texture = GameManager.Instance.currentPlayer.photo;
        Debug.Log($"ViewAvatar => name={GameManager.Instance.currentPlayer.playerName}");
        ViewRecord();
    }
    public void GetLeaderboard()
    {
#if UNITY_WEBGL
        GetLeaderboardEntries();
#endif
    }

    public void ViewRecord()
    {
        int gold = GameManager.Instance.currentPlayer.totalGold;
        //if (level == 0) level = 1;
        if (Language.Instance.CurrentLanguage == "ru")
        {
            txtRecord.text = $"Ä:{gold} Îï:{GameManager.Instance.currentPlayer.totalScore}";
        }
        else
        {
            txtRecord.text = $"M:{gold} Exp:{GameManager.Instance.currentPlayer.totalScore}";
        }
        /*if (Language.Instance.CurrentLanguage == "ru")
        {
            txtRecord.text = $"Óð.{level} Î÷:{GameManager.Instance.currentPlayer.totalScore}";
        }
        else
        {
            txtRecord.text = $"Lv.{level} Sc:{GameManager.Instance.currentPlayer.totalScore}";
        }*/
    }

    public void ViewLeaderboard(string strJson)
    {
        if (strJson == "")
        {
            Debug.Log("ViewLeaderboard strJson= <" + strJson + ">");
            for (int i = 0; i < arRecItems.Length; i++)
            {
                Text txtRecName = arRecItems[i].transform.GetChild(1).gameObject.GetComponent<Text>();
                Text txtRecScore = arRecItems[i].transform.GetChild(2).gameObject.GetComponent<Text>();
                txtRecName.text = "..............";
                txtRecScore.text = "";
            }
            return;
        }
        try
        {
            //Debug.Log("ViewLeaderboard => " + strJson);
            //PersonRecord[] data = JsonConvert.DeserializeObject<PersonRecord[]>(strJson);
            //PersonRecord[] data = JsonUtility.FromJson<PersonRecord[]>(strJson);
            PersonRecord[] data = GetDataFromJson(strJson);
            //Debug.Log("data=>" + data);
            //StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length && i < arRecItems.Length; i++)
            {
                Text txtRecName = arRecItems[i].transform.GetChild(1).gameObject.GetComponent<Text>();
                Text txtRecScore = arRecItems[i].transform.GetChild(2).gameObject.GetComponent<Text>();
                txtRecName.text = data[i].Name;
                txtRecScore.text = $"{data[i].Score}";

                //arTxtRecItems[i].text = $"{data[i]}";
                //Debug.Log("VL => " + data[i].ToString());
                //sb.Append($"{data[i]}\n");
            }
            //txtDescrLeader.text = sb.ToString();
            //Debug.Log("VL sb=" + sb.ToString());
        }
        catch
        {
            Text txtRecName = arRecItems[0].transform.GetChild(1).gameObject.GetComponent<Text>();
            txtRecName.text = Language.Instance.CurrentLanguage == "ru" ? "Îøèáêà" : "Error";
        }
        //panelLiders.SetActive(true);
    }

    private PersonRecord[] GetDataFromJson(string s)
    {
        List<PersonRecord> arr = new List<PersonRecord>();
        string[] ss = s.Split("{");
        for (int i = 1; i < ss.Length; i++)
        {
            int end = ss[i].LastIndexOf('}');
            //Debug.Log($"ss[i]={ss[i]} end={end}");
            string strJson = $"{ss[i].Substring(0, end)}";
            strJson = "{" + strJson + "}";
            //Debug.Log($"strJson={strJson}");
            PersonRecord pr = JsonUtility.FromJson<PersonRecord>(strJson);
            //Debug.Log($"pr={pr}");
            arr.Add(pr);
        }

        return arr.ToArray();
    }
}

[Serializable]
public class MyArrRecords
{
    public PersonRecord[] records { get; set; }
    public MyArrRecords() { }
    public override string ToString()
    {
        return $"Counts={records.Length}";
    }
}

[Serializable]
public class PersonRecord
{
    //public int Rank { get; set; }
    public int Rank;
    //public int Score { get; set; }
    public int Score;
    //public string Name { get; set; }
    public string Name;

    public PersonRecord() { }
    public PersonRecord(int r, int sc, string nm)
    {
        Rank = r;
        Score = sc;
        Name = nm;
    }
    public override string ToString()
    {
        //string nm = String.Format("{0,-25}", Name);
        //return $"{Rank:00} {nm} {Score}";
        return $"{Rank:00} {Name} {Score}";
    }
}

