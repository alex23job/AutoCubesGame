using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoShowControl : MonoBehaviour
{
    [SerializeField] private Button btnBuy;

    private Animator anim;
    private bool isRotate = false;
    private int currentViewCar = -1;

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

    public void NextCar()
    {
        if (currentViewCar != -1) transform.GetChild(currentViewCar + 1).gameObject.SetActive(false);
        currentViewCar++;
        currentViewCar %= 7;
        transform.GetChild(currentViewCar + 1).gameObject.SetActive(true);
        CarPassport carPassport = transform.GetChild(currentViewCar + 1).gameObject.GetComponent<CarPassport>();
        if (carPassport != null)
        {
            if ((carPassport.ExpForSale <= GameManager.Instance.currentPlayer.totalScore) && (carPassport.PriceCar <= GameManager.Instance.currentPlayer.totalGold))
            {
                btnBuy.interactable = true;
            }
            else { btnBuy.interactable = false; }
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
