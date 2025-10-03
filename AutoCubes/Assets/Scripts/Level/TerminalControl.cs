using UnityEngine;
using UnityEngine.UI;

public class TerminalControl : MonoBehaviour
{
    [SerializeField] private int numberTerminal;
    [SerializeField] private LevelControl levelControl;
    [SerializeField] private Button btnRevarded;

    public int NumberTerminal { get { return numberTerminal; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonAdsClick()
    {   //  вызов функции яндекс, показывающей рекламу
        //  а пока вызываем функцию открыти€ термина сразу
        //levelControl.OpenTerminal(gameObject);
    }

    public void ButtonInteractable(bool value)
    {
        btnRevarded.interactable = value;
    }
}
