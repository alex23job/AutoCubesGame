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

    private int nextSpawnCarPoint = -1;

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

    private void SpawnCarWrapper()
    {
        if (nextSpawnCarPoint >= 0 && nextSpawnCarPoint < spawnCars.Length) SpawnCar(nextSpawnCarPoint);
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
            nextSpawnCarPoint = carControl.NumSpawnPoint;
            Invoke("SpawnCarWrapper", 4f);
            //if (carControl.NumSpawnPoint != -1) SpawnCar(carControl.NumSpawnPoint);
            //if (carControl.NumSpawnPoint != -1) Invoke(() => SpawnCar(carControl.NumSpawnPoint), 2f);
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
                print($"MoveOrders count={orders.Count} remove i={i}");
                break;
            }
        }
        Vector3 nextPos = firstOrderPoint.position;
        Vector3 delta;
        if (orders.Count >= 7) print($"MoveOrders count={orders.Count} <{orders[0]}> <{orders[1]}> <{orders[2]}> <{orders[3]}> <{orders[4]}> <{orders[5]}> <{orders[6]}>");
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
