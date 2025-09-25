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

    private Color colorRed = new Color(0.8f, 0, 0);
    private Color colorGreen = new Color(0, 0.9f, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endLevelPanel.SetActive(false);
        ViewExp(0);
        ViewMany(0);
        ViewCars(0, 10);
        ViewOrders(0, 100);
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

    public void ViewCars(int cars, int maxCars)
    {
        txtCars.color = (cars < maxCars) ? colorRed : colorGreen;
        txtCars.text = $"{cars}/{maxCars}";
        txtEndCars.text = $"{cars}/{maxCars}";
    }

    public void ViewOrders(int orders, int maxOrders)
    {
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
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
