using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour
{
    [SerializeField] private Text txtExp;
    [SerializeField] private Text txtMany;
    [SerializeField] private Text txtCars;
    [SerializeField] private Text txtOrders;

    private Color colorRed = new Color(0.8f, 0, 0);
    private Color colorGreen = new Color(0, 0.9f, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ViewExp(0);
        ViewMany(0);
        ViewCars(0, 10);
        ViewOrders(100, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewExp(int exp)
    {
        txtExp.text = exp.ToString();
    }

    public void ViewMany(int many)
    {
        txtMany.text = many.ToString();
    }

    public void ViewCars(int cars, int maxCars)
    {
        txtCars.color = (cars < maxCars) ? colorRed : colorGreen;
        txtCars.text = $"{cars}/{maxCars}";
    }

    public void ViewOrders(int orders, int maxOrders)
    {
        txtOrders.color = (orders < maxOrders) ? colorRed : colorGreen;
        txtOrders.text = $"{orders}/{maxOrders}";
    }
}
