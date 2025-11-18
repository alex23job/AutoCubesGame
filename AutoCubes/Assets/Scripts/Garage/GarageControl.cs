using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GarageControl : MonoBehaviour
{
    [SerializeField] private Button btnRepair;
    [SerializeField] private Button btnSell;
    [SerializeField] private Button btnNext;
    [SerializeField] private Text txtExp;
    [SerializeField] private Text txtGold;
    [SerializeField] private GameObject[] itemPanels;

    private Animator anim;
    private bool isRotate = false;
    private bool isCarsChanged = false;
    private int repairCost = 0;

    private CarPassportInfo garageCarPassport = null;
    private CarPassport currentCarPassport = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayersGarage.Instance.CreateAllPlayerCars();
        btnSell.interactable = PlayersGarage.Instance.CountCarPassports > 1;
        btnNext.interactable = PlayersGarage.Instance.CountCarPassports > 1;
        NextCar();
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
        garageCarPassport = PlayersGarage.Instance.GetNextPassport();
        if (garageCarPassport != null)
        {
            //CarPassport carPassport = garageCar.GetComponent<CarPassport>();
            print($"NextCar CarID={garageCarPassport.CarID} PassportID={garageCarPassport.PassportID}");
            for (int i = 1; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child != null)
                {
                    //print($"i={i} childName={child.name}");
                    CarInfo carChildInfo = child.GetComponent<CarInfo>();
                    if (garageCarPassport.CarID == carChildInfo.CarID)
                    {
                        child.SetActive(true);
                        currentCarPassport = child.GetComponent<CarPassport>();
                        if (currentCarPassport != null)
                        {
                            repairCost = (currentCarPassport.RemainingTrips - garageCarPassport.RemTrips) * currentCarPassport.PriceCar / 100;
                            ViewParams(currentCarPassport, garageCarPassport);
                        }
                        btnRepair.interactable = ((repairCost > 0) && (repairCost <= GameManager.Instance.currentPlayer.totalGold));
                    }
                    else child.SetActive(false);
                }
            }
        }
    }

    private void ViewParams(CarPassport carPassport, CarPassportInfo garagePassport)
    {
        string lang = Language.Instance.CurrentLanguage;
        string strPrice = (lang == "ru") ? "цена доставки" : "shipping price";
        ViewItemPanel(itemPanels[0], PrefabsPak.Instance.GetItemName(0, lang), $"{PrefabsPak.Instance.GetCarName(carPassport.CarID - 1, lang)}  N% {garagePassport.PassportID}");
        ViewItemPanel(itemPanels[1], PrefabsPak.Instance.GetItemName(1, lang), PrefabsPak.Instance.GetBoxType(carPassport.BoxType, lang));
        ViewItemPanel(itemPanels[2], PrefabsPak.Instance.GetItemName(2, lang), $"{carPassport.MaxVelocity}  ( {strPrice} : {carPassport.PriceMult:0.0} )");
        //ViewItemPanel(itemPanels[2], PrefabsPak.Instance.GetItemName(2, lang), $"{carPassport.MaxVelocity}");
        ViewItemPanel(itemPanels[3], PrefabsPak.Instance.GetItemName(3, lang), $"{carPassport.MaxCeilOrders}");
        string strRepairCost = (lang == "ru") ? "цена ремонта" : "repair cost";
        ViewItemPanel(itemPanels[4], PrefabsPak.Instance.GetItemName(4, lang), $"{garagePassport.RemTrips} ({strRepairCost} : {repairCost})");
        //txtExp.text = $"{carPassport.ExpForSale}/{GameManager.Instance.currentPlayer.totalScore}";
        txtExp.text = $"{carPassport.ExpForSale} ( {GameManager.Instance.currentPlayer.totalScore} )";
        txtExp.color = (carPassport.ExpForSale <= GameManager.Instance.currentPlayer.totalScore) ? Color.green : Color.red;
        txtGold.text = $"{carPassport.PriceCar - repairCost}";
        txtGold.color = Color.green;
        //txtGold.text = $"{carPassport.PriceCar - repairCost}/{GameManager.Instance.currentPlayer.totalGold}";
        //txtGold.color = (carPassport.PriceCar <= GameManager.Instance.currentPlayer.totalGold) ? Color.green : Color.red;
    }

    private void ViewItemPanel(GameObject panel, string name, string value)
    {
        Text txtName = panel.transform.GetChild(0).gameObject.GetComponent<Text>();
        Text txtValue = panel.transform.GetChild(1).gameObject.GetComponent<Text>();
        txtName.text = name;
        txtValue.text = value;
    }

    public void OnButtonSellClick()
    {
        if (garageCarPassport != null)
        {
            //int profit = garageCarPassport.PriceCar - repairCost;
            int profit = 0;
            if (currentCarPassport != null) profit = currentCarPassport.PriceCar - repairCost;
            GameManager.Instance.currentPlayer.totalGold += profit;
            PlayersGarage.Instance.RemoveCar(garageCarPassport.PassportID);
            garageCarPassport = null;
            NextCar();
        }
        isCarsChanged = true;
    }

    public void OnButtonRepairClick()
    {
        if (garageCarPassport != null)
        {
            if (GameManager.Instance.currentPlayer.totalGold >= repairCost)
            {
                GameManager.Instance.currentPlayer.totalGold -= repairCost;
                repairCost = 0;
                for (int i = 1; i < transform.childCount; i++)
                {
                    GameObject child = transform.GetChild(i).gameObject;
                    if (child != null)
                    {
                        CarInfo carChildInfo = child.GetComponent<CarInfo>();
                        if (garageCarPassport.CarID == carChildInfo.CarID)
                        {
                            CarPassport carPassport = child.GetComponent<CarPassport>();
                            if (carPassport != null)
                            {
                                garageCarPassport.SetRemainingTrips(carPassport.RemainingTrips);
                                PlayersGarage.Instance.RepairCar(garageCarPassport.PassportID, carPassport.RemainingTrips);
                            }
                            ViewParams(carPassport, garageCarPassport);
                            btnRepair.interactable = false;
                            break;
                        }
                    }
                }
                
            }
        }
        isCarsChanged = true;
    }

    public void LoadMenu()
    {
        if (isCarsChanged) GameManager.Instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
}
