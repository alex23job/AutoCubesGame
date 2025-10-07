using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterText : MonoBehaviour
{
    [SerializeField] private string ru;
    [SerializeField] private string en;
    // Start is called before the first frame update
    void Start()
    {
        Text txt = GetComponent<Text>();
        if (Language.Instance != null)
        {
            if (Language.Instance.CurrentLanguage == "ru")
            {
                txt.text = ru;
            }
            else
            {
                txt.text = en;
            }
        }
        else txt.text = ru;

        Invoke("UpdateLanguage", 0.5f);
    }

    public void UpdateLanguage()
    {
        Text txt = GetComponent<Text>();
        if (Language.Instance.CurrentLanguage == "ru")
        {
            txt.text = ru;
        }
        else
        {
            txt.text = en;
        }
    }
}

