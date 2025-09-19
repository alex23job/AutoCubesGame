using UnityEngine;

[RequireComponent(typeof(CarInfo))]
public class CarControl : MonoBehaviour
{
    private CarInfo carInfo;

    private void Awake()
    {
        carInfo = GetComponent<CarInfo>();
    }
    public int CarID { get {  return carInfo.CarID; } } 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
