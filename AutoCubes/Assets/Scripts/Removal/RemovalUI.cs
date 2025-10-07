using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RemovalUI : MonoBehaviour
{
    [SerializeField] private Text txtClock;
    [SerializeField] private Text txtOrders;

    [SerializeField] private GameObject lossPanel;
    [SerializeField] private Text txtLossOrders;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private Text txtExp;
    [SerializeField] private Text txtEndOrders;
    [SerializeField] private Button btnAds;

    private int exp = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winPanel.SetActive(false);
        lossPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewClock(int value)
    {
        int minute = value / 60;
        int second = value % 60;
        txtClock.text = $"{minute:00}:{second:00}";
    }

    public void ViewOrders(int orders, int maxOrders)
    {
        txtOrders.text = $"{orders}/{maxOrders}";
    }

    public void ViewWinPanel(int orders, int exp)
    {
        txtEndOrders.text = orders.ToString();
        this.exp = exp;
        txtExp.text = exp.ToString();
        winPanel.SetActive(true);
    }

    public void ViewExp(int exp)
    {
        this.exp = exp;
        txtExp.text = exp.ToString(); 
    }

    public void ViewLossPanel(int orders, int maxOrders)
    {
        string s1, s2;
        if (Language.Instance.CurrentLanguage == "ru")
        {
            s1 = "Загружено коробок "; s2 = "из";
        }
        else
        {
            s1 = "Loaded boxes "; s2 = "out of";
        }
        txtLossOrders.text = $"{s1}{orders} {s2} {maxOrders}";
        lossPanel.SetActive(true);
    }

    public void Restart()
    {
        GameManager.Instance.currentPlayer.currentScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        GameManager.Instance.currentPlayer.totalScore += GameManager.Instance.currentPlayer.currentScore;
        GameManager.Instance.currentPlayer.totalGold += GameManager.Instance.currentPlayer.sessionGold;
        GameManager.Instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
}
