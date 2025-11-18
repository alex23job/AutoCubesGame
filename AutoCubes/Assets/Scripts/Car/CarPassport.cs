using System;
using UnityEngine;

public class CarPassport : MonoBehaviour
//public class CarPassport
{
    public static int GetCarIDFromCsv(string csv, string sep = "=")
    {
        string[] ar = csv.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
        if (ar.Length > 0)
        {
            if (int.TryParse(ar[0], out int carID)) return carID;
        }
        return -1;
    }

    [SerializeField] private int priceCar = 1000;
    [SerializeField] private int expForSale = 0;
    [SerializeField] private int maxVelocity = 60;
    [SerializeField] private int maxCeilOrders = 15;

    private CarInfo carInfo;
    [SerializeField] private int remainingTrips = 100;

    public float PriceMult { get { return carInfo.PriceMult; } }
    public int BoxType { get { return carInfo.Termo; } }
    public int RemainingTrips { get { return remainingTrips; } }
    public int CarID { get { return carInfo.CarID; } } 
    public int PassportCarID { get; set; }

    public int PriceCar {  get { return priceCar; } }
    public int ExpForSale { get { return expForSale; } }
    public int MaxVelocity { get { return maxVelocity; } }
    public int MaxCeilOrders { get { return maxCeilOrders; } }

    public bool IsUsing { get; set; } = false;

    public CarPassport() { }
    public CarPassport(CarPassport passport)
    {
        this.carInfo = new CarInfo(passport.carInfo);
        this.priceCar = passport.priceCar;
        this.expForSale = passport.expForSale;
        this.maxVelocity = passport.maxVelocity;
        this.maxCeilOrders = passport.maxCeilOrders;
        this.remainingTrips = passport.remainingTrips;
        this.PassportCarID = passport.PassportCarID;
    }

    private void Awake()
    {
        CarInfo info = GetComponent<CarInfo>();
        if (info != null) carInfo = info;else carInfo = new CarInfo(-1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParamsFromCsv(string csv, string sep = "=")
    {
        string[] ar = csv.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
        if (ar.Length > 2)
        {
            if (int.TryParse(ar[1], out int passportCarID)) PassportCarID = passportCarID;
            if (int.TryParse(ar[2], out int remTrips)) remainingTrips = remTrips;
        }
    }

    public void SetRemainingTrips(int remainingTrips)
    {
        this.remainingTrips = remainingTrips;
    }

    public void UsingTrip()
    {
        remainingTrips--;
    }

    public string ToCsvString(string sep = "=")
    {
        return $"{CarID}{sep}{PassportCarID}{sep}{remainingTrips}{sep}";
    }
}

[Serializable]
public class CarPassportInfo
{
    private int carID = -1;
    private int passportID = -1;
    private int remTrips = -1;

    public CarPassportInfo() { }
    public CarPassportInfo(CarPassport carPassport)
    {
        carID = carPassport.CarID;
        passportID = carPassport.PassportCarID;
        remTrips = carPassport.RemainingTrips;
    }

    public CarPassportInfo(int carID, int passportID, int remTrips)
    {
        this.carID = carID;
        this.passportID = passportID;
        this.remTrips = remTrips;
    }

    public CarPassportInfo(string csv, string sep = "=")
    {
        string[] ar = csv.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
        if (ar.Length > 2)
        {
            if (int.TryParse(ar[0], out int csvCarID)) carID = csvCarID;
            if (int.TryParse(ar[1], out int passportCarID)) passportID = passportCarID;
            if (int.TryParse(ar[2], out int remainingTrips)) remTrips = remainingTrips;
        }
    }

    public int CarID { get => carID; }
    public int PassportID { get => passportID; }
    public int RemTrips { get => remTrips; }

    public bool IsUsing { get; set; } = false;
    public void UsingTrip()
    {
        remTrips--;
    }
    public void SetRemainingTrips(int remainingTrips)
    {
        this.remTrips = remainingTrips;
    }

    public void SetPassportID(int passportID)
    {
        this.passportID = passportID;
    }

    public string ToCsvString(string sep = "=")
    {
        return $"{CarID}{sep}{PassportID}{sep}{RemTrips}{sep}";
    }
}
