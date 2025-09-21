using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private SpawnCars[] spawnCars;
    [SerializeField] private SpawnOrders spawnOrders;
    [SerializeField] private Transform firstOrderPoint;

    private List<GameObject> cars = new List<GameObject>();
    private List<GameObject> orders = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnCar(0);
        SpawnCar(1);
        SpawnCar(2);
        SpawnCar(3);
        Invoke("CarsToWay", 30f);
        SpawnOrder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnCar(int numSpawnPoint)
    {
        GameObject car = spawnCars[numSpawnPoint].SpawnCar();
        if (car != null) cars.Add(car); 
    }

    private void CarsToWay()
    {
        foreach (GameObject car in cars)
        {
            CarControl carControl = car.GetComponent<CarControl>();
            if (carControl != null)
            {
                carControl.CarToWay();
            }
        }
    }

    private void SpawnOrder()
    {
        spawnOrders.SpawnOrder(firstOrderPoint.position);
    }
}
