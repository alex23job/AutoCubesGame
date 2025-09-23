using UnityEngine;
using UnityEngine.UI;

public class ClockControl : MonoBehaviour
{
    [SerializeField] private Text txtClock;
    [SerializeField] private LevelControl levelControl;

    private int numSpawnPoint = -1;
    private int currentSecond = -1;
    private float timer = 1f;
    private bool isTimer = false;

    public int NumSpawnPoint { get { return numSpawnPoint; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ViewClock(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimer)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else
            {
                timer = 1f;
                currentSecond--;
                ViewClock(currentSecond);
                if (currentSecond == 0)
                {
                    isTimer = false;
                    OnCkickGO();
                }
            }
        }
    }

    public void ViewClock(int second)
    {
        int minute = second / 60;
        int curSecond = second % 60;
        txtClock.text = $"{minute:00}:{curSecond:00}";
    }

    public void OnCkickGO()
    {
        levelControl.CarGO(numSpawnPoint);
    }

    public void SetNumPoint(int numPoint)
    {
        numSpawnPoint = numPoint;
    }

    public void StartTimer(int maxSecond)
    {
        if (!isTimer)
        {
            currentSecond = maxSecond;
            ViewClock(currentSecond);
            timer = 1f;
            isTimer = true;
        }
    }

    public void StopTimer()
    {
        isTimer = false;
        timer = 0f;
        currentSecond = -1;
    }
}
