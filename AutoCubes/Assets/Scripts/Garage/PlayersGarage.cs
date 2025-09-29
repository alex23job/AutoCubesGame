using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayersGarage : MonoBehaviour
{
    private List<GameObject> cars = new List<GameObject>();
    private string loadingCsvGarageString = "";
    private string defaultSeparator = "#";
    private int currentCar = 0;

    public int CountCars {  get { return cars.Count; } }

    public static PlayersGarage Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGarageCsvString(string csvGarageString)
    {
        loadingCsvGarageString = csvGarageString;
    }

    public void CreateAllPlayerCars()
    {
        string[] ar = loadingCsvGarageString.Split(defaultSeparator, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string carCsv in ar)
        {
            int id = CarPassport.GetCarIDFromCsv(carCsv, "=");
            GameObject prefabCar = PrefabsPak.Instance.GetCarPrefab(id - 1);
            GameObject car = Instantiate(prefabCar);
            CarPassport carPassport = car.GetComponent<CarPassport>();
            carPassport.SetParamsFromCsv(carCsv);
            cars.Add(car);
        }
    }

    public void CreatePlayerCars(string csv, string sep = "#")
    {
        string[] ar = csv.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string carCsv in ar)
        {
            int id = CarPassport.GetCarIDFromCsv(carCsv, "=");
            GameObject prefabCar = PrefabsPak.Instance.GetCarPrefab(id - 1);
            GameObject car = Instantiate(prefabCar);
            CarPassport carPassport = car.GetComponent<CarPassport>();
            carPassport.SetParamsFromCsv(carCsv);
            cars.Add(car);
        }
    }

    public GameObject GetNextCar(bool isFree = false)
    {
        if (CountCars > 0)
        {
            if (isFree)
            {
                foreach(GameObject car in cars)
                {
                    CarPassport carPassport = car.GetComponent<CarPassport>();
                    if (carPassport != null && carPassport.IsUsing == false) return car; 
                }
            }
            else
            {
                if ((currentCar >= 0) && (currentCar < CountCars))
                {
                    GameObject resCar = cars[currentCar];
                    currentCar++;
                    currentCar %= CountCars;
                    return resCar;
                }
            }
        }
        return null;
    }

    public void RemoveCar(int passpotID)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            CarPassport carPassport = cars[i].GetComponent<CarPassport>();
            if (carPassport != null && carPassport.PassportCarID == passpotID)
            {
                cars.RemoveAt(i);
                break;
            }
        }
    }

    public void AddGar(int carID)
    {
        GameObject car = Instantiate(PrefabsPak.Instance.GetCarPrefab(carID - 1));
        car.GetComponent<CarPassport>().PassportCarID = GenerateNextPassportCarID();
        cars.Add(car);
    }

    private int GenerateNextPassportCarID()
    {
        if (CountCars > 0)
        {
            int maxNum = 0;
            foreach (GameObject car in cars)
            {
                CarPassport carPassport = car.GetComponent<CarPassport>();
                if (carPassport != null)
                {
                    if (carPassport.PassportCarID > maxNum) maxNum = carPassport.PassportCarID;
                }
            }
            return maxNum + 1;
        }
        return 1;
    }

    public string GarageToCsvString(string sep = "#")
    {
        StringBuilder sb = new StringBuilder();
        foreach(GameObject car in cars)
        {
            CarPassport carPassport = car.GetComponent<CarPassport>();
            sb.Append($"{carPassport.ToCsvString("=")}{sep}");
        }
        return sb.ToString();
    }
}
