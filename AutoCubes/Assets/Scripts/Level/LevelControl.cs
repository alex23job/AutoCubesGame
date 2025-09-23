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
        for (int i = 0; i < spawnCars.Length; i++)
        {
            spawnCars[i].SetLevelControl(this, i);
        }
        //foreach(SpawnCars spawnCar in spawnCars) 
        SpawnCar(0);
        /*SpawnCar(1);
        SpawnCar(2);
        SpawnCar(3);
        Invoke("CarsToWay", 30f);*/
        for (int j = 0; j < 7; j++) SpawnOrder();
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
        Vector3 pos = firstOrderPoint.position;
        float offset = MoveOrders();
        pos.x -= offset;
        orders.Add(spawnOrders.SpawnOrder(pos));
    }

    public void PackingOrderToCar(GameObject car, GameObject order, bool isFull)
    {
        if (isFull)
        {
            CarControl carControl = car.GetComponent<CarControl>();
            carControl.CarToWay();
            if (carControl.NumSpawnPoint != -1) SpawnCar(carControl.NumSpawnPoint);
        }
        SpawnOrder();
    }

    private float MoveOrders()
    {
        int i;
        for (i = 0; i < orders.Count; i++)
        {
            if (orders[i].GetComponent<Order>().IsPacking)
            {
                orders.RemoveAt(i);
                break;
            }
        }
        Vector3 nextPos = firstOrderPoint.position;
        Vector3 delta;
        for (i = 0; i < orders.Count; i++) 
        {
            delta = nextPos - orders[i].transform.position;
            if (delta.magnitude > 0.5f)
            {
                orders[i].GetComponent<Order>().SetTarget(nextPos);
            }
            nextPos.x -= 3.65f;
        }
        return 3.65f * i;
    }
}
