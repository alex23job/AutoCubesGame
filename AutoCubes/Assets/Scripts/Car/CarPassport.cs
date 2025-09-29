using UnityEngine;

public class CarPassport : MonoBehaviour
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
    private int remainingTrips = 100;

    public int RemainingTrips { get { return remainingTrips; } }
    public int CarID { get { return carInfo.CarID; } } 
    public int PassportCarID { get; set; }

    public int PriceCar {  get { return priceCar; } }
    public int ExpForSale { get { return expForSale; } }
    public int MavVelocity { get { return maxVelocity; } }
    public int MaxCeilOrders { get { return maxCeilOrders; } }

    public bool IsUsing { get; set; } = false;

    private void Awake()
    {
        carInfo = GetComponent<CarInfo>();
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
