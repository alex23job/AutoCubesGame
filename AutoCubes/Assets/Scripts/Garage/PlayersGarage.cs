using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayersGarage : MonoBehaviour
{
    private List<GameObject> cars = new List<GameObject>();
    private List<CarPassportInfo> carPassports = new List<CarPassportInfo>();
    private string loadingCsvGarageString = "";
    private string defaultSeparator = "#";
    private int currentCar = 0;
    private int currentCarPassport = 0;

    public int CountCars { get { return cars.Count; } }
    public int CountCarPassports { get { return carPassports.Count; } }

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
        //print($"loadingCsvGarageString = {loadingCsvGarageString}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGarageCsvString(string csvGarageString)
    {
        loadingCsvGarageString = csvGarageString;
    }

    public void CreateAllPassports(string csvGarageString)
    {
        loadingCsvGarageString = csvGarageString;
        if (loadingCsvGarageString == "") return;
        carPassports.Clear();
        string[] ar = loadingCsvGarageString.Split(defaultSeparator, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string carCsv in ar)
        {
            int id = CarPassport.GetCarIDFromCsv(carCsv, "=");
            GameObject prefabCar = PrefabsPak.Instance.GetCarPrefab(id - 1);
            //GameObject car = Instantiate(prefabCar);
            CarPassportInfo carPassport = new CarPassportInfo(carCsv);
            if (carPassport != null)
            {
                //carPassport.SetParamsFromCsv(carCsv);
                carPassports.Add(carPassport);
            }
        }
        print($"CreateAllPassports countPassports=<{carPassports.Count}>");
    }

    public void CreateAllPlayerCars()
    {
        //if ((loadingCsvGarageString == "") || (cars.Count > 0)) return;
        if (loadingCsvGarageString == "") return;
        cars.Clear();
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

    public void DropAllPlayerCars()
    {
        for(int i = CountCars; i > 0; i--)
        {
            GameObject car = cars[i - 1];
            cars.RemoveAt(i - 1);
            Destroy(car);
        }
        cars.Clear();
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

    public CarPassportInfo GetNextPassport()
    {
        if (CountCarPassports > 0)
        {
            if ((currentCarPassport >= 0) && (currentCarPassport < CountCarPassports))
            {
                CarPassportInfo res = carPassports[currentCarPassport];
                currentCarPassport++;
                if (CountCarPassports == 1) currentCarPassport = 0;
                else
                {
                    if (CountCarPassports > 1) currentCarPassport %= CountCarPassports;
                }
                return res;
            }
        }
        /*if (CountCars > 0)
        {
            if ((currentCar >= 0) && (currentCar < CountCars))
            {
                GameObject resCar = cars[currentCar];
                currentCar++;
                if (CountCars == 1) currentCar = 0;
                else
                {
                    if (CountCars > 1) currentCar %= CountCars;
                }
                //Debug.Log($"GetNextCar CountCars={CountCars} curCar={currentCar} resCar={resCar}   cars[0]={cars[0]}");
                return resCar.GetComponent<CarPassport>();
            }
        }*/
        return null;
    }

    public string GetFreeCsvCarPassport()
    {
        if (CountCarPassports > 0)
        {
            foreach (var carPassport in carPassports)
            {
                //CarPassport carPassport = car.GetComponent<CarPassport>();
                if (carPassport != null && carPassport.IsUsing == false && carPassport.RemTrips > 0)
                {
                    carPassport.IsUsing = true;
                    return carPassport.ToCsvString();
                }
            }
        }
        return "";
        /*if (CountCars > 0)
        {
            foreach (GameObject car in cars)
            {
                CarPassport carPassport = car.GetComponent<CarPassport>();
                if (carPassport != null && carPassport.IsUsing == false && carPassport.RemainingTrips > 0)
                {
                    carPassport.IsUsing = true;
                    return carPassport.ToCsvString();
                }
            }
        }
        return "";*/
    }

    /*public GameObject GetNextCar(bool isFree = false)
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
                    if (CountCars == 1) currentCar = 0;
                    else
                    {
                        if (CountCars > 1) currentCar %= CountCars;
                    }
                    //Debug.Log($"GetNextCar CountCars={CountCars} curCar={currentCar} resCar={resCar}   cars[0]={cars[0]}");
                    return resCar;
                }
            }
        }
        return null;
    }*/

    public void RemoveCar(int passpotID)
    {
        for (int i = 0; i < carPassports.Count; i++)
        {
            if (carPassports[i] != null && carPassports[i].PassportID == passpotID)
            {
                carPassports.RemoveAt(i);
                loadingCsvGarageString = PassportsToCsvString("#");
                break;
            }
        }
        /*for (int i = 0; i < cars.Count; i++)
        {
            CarPassport carPassport = cars[i].GetComponent<CarPassport>();
            if (carPassport != null && carPassport.PassportCarID == passpotID)
            {
                GameObject car = cars[i];
                cars.RemoveAt(i);
                Destroy(car);
                loadingCsvGarageString = GarageToCsvString("#");
                break;
            }
        }*/
    }

    public void AddCar(int carID)
    {
        GameObject prefabCar = PrefabsPak.Instance.GetCarPrefab(carID - 1);
        print($"1) AddCar prefabCar=<{prefabCar}>");
        CarPassport passport = prefabCar.GetComponent<CarPassport>();
        CarPassportInfo carPassport = new CarPassportInfo(carID, -1, passport.RemainingTrips);
        carPassport.SetPassportID(GenerateNextPassportCarID());
        carPassports.Add(carPassport);
        loadingCsvGarageString = PassportsToCsvString("#");
        print($"3) AddCar CountCarPassports={CountCarPassports} CarID={carID} car={prefabCar} csv={loadingCsvGarageString}");
        /*if (CountCars == 0)
        {
            CreateAllPlayerCars();
        }
        print($"1) AddCar countCars={CountCars} CarID={carID} csv={loadingCsvGarageString}");
        GameObject prefabCar = PrefabsPak.Instance.GetCarPrefab(carID - 1);
        //print($"AddCar prefabCar={prefabCar}");
        //GameObject car = Instantiate(prefabCar);
        //print($"2) AddCar car=<{car}>");
        CarPassport carPassport = prefabCar.GetComponent<CarPassport>();
        //car.GetComponent<CarPassport>().PassportCarID = GenerateNextPassportCarID();
        //cars.Add(car);

        //loadingCsvGarageString = GarageToCsvString("#");
        loadingCsvGarageString = PassportsToCsvString("#");
        //loadingCsvGarageString += car.GetComponent<CarPassport>().ToCsvString() + "#";
        print($"3) AddCar countCars={CountCars} CarID={carID} car={cars[CountCars - 1]} csv={loadingCsvGarageString}");
        DropAllPlayerCars();*/
    }

    private int GenerateNextPassportCarID()
    {
        if (CountCarPassports > 0)
        {
            int maxNum = 0;
            print($"1) GenerateNextPassportCarID maxnum={maxNum} passports=<{CountCarPassports}>");
            foreach (var carPassport in carPassports)
            {
                print($"2) GenerateNextPassportCarID maxnum={maxNum} PassportCarID=<{carPassport.PassportID}>");
                if (carPassport == null) continue;
                //CarPassport carPassport = car.GetComponent<CarPassport>();
                if (carPassport != null)
                {
                    if (carPassport.PassportID > maxNum) maxNum = carPassport.PassportID;
                    print($"3) GenerateNextPassportCarID maxnum={maxNum} >= PassportCarID=<{carPassport.PassportID}>");
                }
            }
            return maxNum + 1;
        }
        /*if (CountCars > 0)
        {
            int maxNum = 0;
            print($"1) GenerateNextPassportCarID maxnum={maxNum} cars=<{cars.Count}>");
            foreach (GameObject car in cars)
            {
                print($"2) GenerateNextPassportCarID maxnum={maxNum} car=<{car}>");
                if (car == null) continue;
                CarPassport carPassport = car.GetComponent<CarPassport>();
                if (carPassport != null)
                {
                    if (carPassport.PassportCarID > maxNum) maxNum = carPassport.PassportCarID;
                    print($"3) GenerateNextPassportCarID maxnum={maxNum} car=<{car}> PassportCarID=<{carPassport.PassportCarID}>");
                }
            }
            return maxNum + 1;
        }*/
        return 1;
    }

    public void UsingCarTrip(int passportID)
    {
        if (CountCarPassports > 0)
        {
            foreach (var carPassport in carPassports)
            {
                if (carPassport != null)
                {
                    if (carPassport.PassportID == passportID)
                    {
                        carPassport.UsingTrip();
                        carPassport.IsUsing = false;
                        //loadingCsvGarageString = GarageToCsvString("#");
                        loadingCsvGarageString = PassportsToCsvString("#");
                        break;
                    }
                }
            }
        }
        /*if (CountCars > 0)
        {
            foreach (GameObject car in cars)
            {
                CarPassport carPassport = car.GetComponent<CarPassport>();
                if (carPassport != null)
                {
                    if (carPassport.PassportCarID == passportID)
                    {
                        carPassport.UsingTrip();
                        carPassport.IsUsing = false;
                        loadingCsvGarageString = GarageToCsvString("#");
                        break;
                    }
                }
            }
        }*/
    }

    public void RepairCar(int passportID, int maxTrips)
    {
        if (CountCarPassports > 0)
        {
            foreach (var carPassport in carPassports)
            {
                if (carPassport != null)
                {
                    print($"RepairCar passportID={passportID} carPassportID={carPassport.PassportID} maxTrips={maxTrips}");
                    if (carPassport.PassportID == passportID)
                    {
                        carPassport.SetRemainingTrips(maxTrips);
                        //loadingCsvGarageString = GarageToCsvString("#");
                        loadingCsvGarageString = PassportsToCsvString("#");
                        break;
                    }
                }
            }
        }
        /*if (CountCars > 0)
        {
            foreach (GameObject car in cars)
            {
                CarPassport carPassport = car.GetComponent<CarPassport>();
                if (carPassport != null)
                {
                    print($"RepairCar passportID={passportID} carPassportID={carPassport.PassportCarID} maxTrips={maxTrips}");
                    if (carPassport.PassportCarID == passportID)
                    {
                        carPassport.SetRemainingTrips(maxTrips);
                        loadingCsvGarageString = GarageToCsvString("#");
                        break;
                    }
                }
            }
        }*/
    }

    public string PassportsToCsvString(string sep = "#")
    {
        if (CountCarPassports > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var carPassport in carPassports)
            {
                sb.Append($"{carPassport.ToCsvString("=")}{sep}");
            }
            return sb.ToString();
        }
        return "";
    }

    public string GarageToCsvString(string sep = "#")
    {
        if ((CountCars == 0) && (loadingCsvGarageString != "")) { return loadingCsvGarageString; }
        StringBuilder sb = new StringBuilder();
        foreach (GameObject car in cars)
        {
            if (car != null)
            {
                CarPassport carPassport = car.GetComponent<CarPassport>();
                if (carPassport != null) sb.Append($"{carPassport.ToCsvString("=")}{sep}");
                else print("carPassport is null ???");
            }
            else print($"countCars={cars.Count} car is null ???");
        }
        return sb.ToString();
    }
}
