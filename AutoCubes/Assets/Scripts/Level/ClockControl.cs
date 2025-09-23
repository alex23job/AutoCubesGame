using UnityEngine;
using UnityEngine.UI;

public class ClockControl : MonoBehaviour
{
    [SerializeField] private Text txtClock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ViewClock(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewClock(int second)
    {
        int minute = second / 60;
        int curSecond = second % 60;
        txtClock.text = $"{minute:00}:{curSecond:00}";
    }
}
