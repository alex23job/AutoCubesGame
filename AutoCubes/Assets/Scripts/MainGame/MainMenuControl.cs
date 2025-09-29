using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("LevelScene");
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
