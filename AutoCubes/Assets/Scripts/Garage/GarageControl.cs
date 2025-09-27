using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageControl : MonoBehaviour
{

    private Animator anim;
    private bool isRotate = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BoardRotate()
    {
        isRotate = !isRotate;
        anim.SetBool("IsRotate", isRotate);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
