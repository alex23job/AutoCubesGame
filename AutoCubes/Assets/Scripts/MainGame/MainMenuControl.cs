using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuControl : MonoBehaviour
{
    [SerializeField] private Image[] foneStoreButtons;
    [SerializeField] private Image[] foneRemovalButtons;
    [SerializeField] private Button[] storeLevelButtons;
    [SerializeField] private GameObject selectPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewSelectGamePanel()
    {
        int i;
        GameManager.Instance.currentPlayer.currentLevel = GameManager.Instance.currentPlayer.maxLevel;
        for (i = 0; i < foneStoreButtons.Length; i++)
        {
            if (i == GameManager.Instance.currentPlayer.maxLevel - 1) foneStoreButtons[i].gameObject.SetActive(true);
            else foneStoreButtons[i].gameObject.SetActive(false);
            if (i < GameManager.Instance.currentPlayer.maxLevel)
            {
                storeLevelButtons[i].interactable = true;
            }
            else
            {
                storeLevelButtons[i].interactable = false;
            }
        }
        GameManager.Instance.currentPlayer.numBoxRemoval = 0;
        for (i = 0; i < foneRemovalButtons.Length; i++)
        {
            if (i == 0) foneRemovalButtons[i].gameObject.SetActive(true);
            else foneRemovalButtons[i].gameObject.SetActive(false);
        }
        selectPanel.SetActive(true);
    }

    public void OnBtnLevelClick(int lvl)
    {
        foneStoreButtons[GameManager.Instance.currentPlayer.currentLevel - 1].gameObject.SetActive(false);
        GameManager.Instance.currentPlayer.currentLevel = lvl;
        foneStoreButtons[GameManager.Instance.currentPlayer.currentLevel - 1].gameObject.SetActive(true);
    }

    public void OnBtnRemovalClick(int numBox)
    {
        foneRemovalButtons[GameManager.Instance.currentPlayer.numBoxRemoval].gameObject.SetActive(false);
        GameManager.Instance.currentPlayer.numBoxRemoval = numBox;
        GameManager.Instance.currentPlayer.countSecondRemoval = 150 + numBox * 30;
        foneRemovalButtons[GameManager.Instance.currentPlayer.numBoxRemoval].gameObject.SetActive(true);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void LoadRemoval()
    {
        if (PlayersGarage.Instance.CountCars > 0) PlayersGarage.Instance.DropAllPlayerCars();
        SceneManager.LoadScene("RemovalScene");
    }

    public void LoadGarage()
    {
        SceneManager.LoadScene("GaragScene");
    }

    public void LoadAutoShow()
    {
        SceneManager.LoadScene("AutoShowScene");
    }

    public void ViewRecord()
    {

    }

    public void LoadComplete()
    {

    }

    public void ViewAvatar()
    {

    }

    public void ViewLeaderboard(string strJson)
    {

    }
}
