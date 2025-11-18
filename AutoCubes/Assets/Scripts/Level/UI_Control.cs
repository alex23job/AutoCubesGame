using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour
{
    [SerializeField] private Text txtExp;
    [SerializeField] private Text txtMany;
    [SerializeField] private Text txtCars;
    [SerializeField] private Text txtOrders;
    [SerializeField] private Text txtTermo;

    [SerializeField] private GameObject endLevelPanel;
    [SerializeField] private Text txtEndExp;
    [SerializeField] private Text txtEndMany;
    [SerializeField] private Text txtEndCars;
    [SerializeField] private Text txtEndOrders;

    [SerializeField] private Button btnAds;
    [SerializeField] private PeriodicColorChange imgHelpControl;

    private Color colorRed = new Color(0.8f, 0, 0);
    private Color colorGreen = new Color(0, 0.9f, 0);

    private bool isHelp = false;
    private float timer = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btnAds.interactable = true;
        endLevelPanel.SetActive(false);
        ViewExp(0);
        ViewMany(0);
        ViewCars(0, 10);
        ViewOrders(0, 100);
    }

    private void Update()
    {
        if (isHelp == false)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else
            {
                isHelp = true;
                imgHelpControl.SetChange(true);
            }
        }
    }
    private void ResetImgHelpColor()
    {
        isHelp = false;
        timer = 3f;
        imgHelpControl.SetChange(false);
    }

    public void ViewExp(int exp)
    {
        txtExp.text = exp.ToString();
        txtEndExp.text = exp.ToString();
    }

    public void ViewMany(int many)
    {
        txtMany.text = many.ToString();
        txtEndMany.text = many.ToString();
    }

    public void ViewEndMany(int many)
    {
        txtEndMany.text = many.ToString();
        //btnAds.interactable = false;
    }

    public void ViewCars(int cars, int maxCars)
    {
        txtCars.color = (cars < maxCars) ? colorRed : colorGreen;
        txtCars.text = $"{cars}/{maxCars}";
        txtEndCars.text = $"{cars}/{maxCars}";
    }

    public void ViewOrders(int orders, int maxOrders)
    {
        ResetImgHelpColor();
        txtOrders.color = (orders < maxOrders) ? colorRed : colorGreen;
        txtOrders.text = $"{orders}/{maxOrders}";
        txtEndOrders.text = $"{orders}/{maxOrders}";
    }

    public void ViewTermo(int termo)
    {
        string strTermo = $"{(termo > 0 ? '+' : '-')}{Mathf.Abs(termo)}";
        if (termo == 0) strTermo = "0";
        txtTermo.text = strTermo;
    }

    public void ViewEndLevelPanel()
    {
        endLevelPanel.SetActive(true);
        btnAds.interactable = true;
    }

    public void LoadMainMenu()
    {
        PlayersGarage.Instance.ClearAllUsing();
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        PlayersGarage.Instance.ClearAllUsing();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
