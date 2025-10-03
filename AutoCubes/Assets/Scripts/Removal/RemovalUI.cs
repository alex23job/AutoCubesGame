using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RemovalUI : MonoBehaviour
{
    [SerializeField] private Text txtClock;
    [SerializeField] private Text txtOrders;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
