using UnityEngine;
using UnityEngine.UI;

public class PeriodicColorChange : MonoBehaviour
{
    [SerializeField] private float ChangeDelay = 0.05f;
    private Color imgColor;
    private bool isChanged = false;
    private bool isIncrement = true;
    private float timer = 0.05f;
    private int currentIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imgColor = GetComponent<Image>().color;
        print($"r={imgColor.r} g={imgColor.g} b={imgColor.b}");
        if (imgColor.r < 0.1f) imgColor.r = 0.3f;
        if (imgColor.g < 0.1f) imgColor.g = 0.7f;
        if (imgColor.b < 0.1f) imgColor.b = 0.85f;
        imgColor.a = 1f;
        GetComponent<Image>().color = imgColor;
        //SetChange(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isChanged)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else
            {
                timer = ChangeDelay;
                ChangeColor();
            }
        }
    }

    public void SetChange(bool zn)
    {
        isChanged = zn;
        if (isChanged == false) GetComponent<Image>().color = imgColor;
    }

    private void ChangeColor()
    {
        Color color = new Color(imgColor.r - 0.1f, imgColor.g - 0.1f, imgColor.b - 0.1f, 1f);
        color.r += currentIndex * 0.01f;
        color.g += currentIndex * 0.01f;
        color.b += currentIndex * 0.01f;
        if (isIncrement)
        {
            currentIndex++;
            if (currentIndex > 20)
            {
                isIncrement = false;
                currentIndex = 19;
            }
        }
        else
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                isIncrement = true;
                currentIndex = 1;
            }
        }
        //currentIndex %= 20;
        GetComponent<Image>().color = color;
    }
}
