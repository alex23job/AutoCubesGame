using UnityEngine;

public class CarPassport : MonoBehaviour
{
    [SerializeField] private int priceCar = 1000;
    [SerializeField] private int expForSale = 0;
    [SerializeField] private int maxVelocity = 60;

    private CarInfo carInfo;
    private int remainingTrips = 100;

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
}
