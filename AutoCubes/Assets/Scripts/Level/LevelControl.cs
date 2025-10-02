using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private UI_Control ui_Control;
    //[SerializeField] private Yandex yandex;
    [SerializeField] private SpawnCars[] spawnCars;
    [SerializeField] private GameObject[] clocks;
    [SerializeField] private GameObject[] terminals;
    [SerializeField] private SpawnOrders spawnOrders;
    [SerializeField] private Transform firstOrderPoint;
    [SerializeField] private int delayCarToWay = 150;

    private List<GameObject> cars = new List<GameObject>();
    private List<GameObject> orders = new List<GameObject>();

    private List<int> nextSpawnCarPoint = new List<int>();

    private LevelInfo levelInfo = new LevelInfo(10, 100, true);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelInfo = LevelInfo.GetLevel(GameManager.Instance.currentPlayer.currentLevel - 1);
        levelInfo.ViewStartCarsAndOrders(ui_Control);
        int i;
        for (i = 0; i < spawnCars.Length; i++)
        {
            spawnCars[i].SetLevelControl(this, i);
        }
        for (i = 0; i < clocks.Length; i++)
        {
            clocks[i].GetComponent<ClockControl>().SetNumPoint(i);
            clocks[i].SetActive(false);
        }
        PlayersGarage.Instance.CreateAllPlayerCars();
        //foreach(SpawnCars spawnCar in spawnCars) 
        SpawnCar(0);
        /*SpawnCar(1);
        SpawnCar(2);
        SpawnCar(3);
        Invoke("CarsToWay", 30f);*/
        
        int termo = UnityEngine.Random.Range(-30, 50);
        ui_Control.ViewTermo(termo);
        spawnOrders.SetMarkering(levelInfo.IsMarkering, termo);  //  true пока - надо из инфы про уровень
        //for (int j = 0; j < 7; j++) SpawnOrder();
        for (int j = 0; j < 7; j++) Invoke("SpawnOrder", j * 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenTerminal(int numTerminal)
    {
        foreach (GameObject terminal in terminals)
        {
            int numSpawnCar = terminal.GetComponent<TerminalControl>().NumberTerminal;
            if (numSpawnCar == numTerminal)
            {
                terminal.SetActive(false);
                SpawnCar(numSpawnCar);
            }
        }
    }

    public void OpenTerminal(GameObject terminal)
    {
        int numSpawnCar = terminal.GetComponent<TerminalControl>().NumberTerminal;
        terminal.SetActive(false);
        SpawnCar(numSpawnCar);
    }

    public void CarGO(int numSpawnPoint)
    {
        foreach(GameObject car in cars)
        {
            CarControl carControl = car.GetComponent<CarControl>();
            if ((carControl != null) && (carControl.NumSpawnPoint == numSpawnPoint))
            {
                carControl.CarToWay();
                levelInfo.AddCars(1, ui_Control);
                levelInfo.AddExp(carControl.ExpCar, ui_Control);
                levelInfo.AddMany(carControl.PriceCar, ui_Control);
                clocks[carControl.NumSpawnPoint].GetComponent<ClockControl>().StopTimer();
                clocks[numSpawnPoint].SetActive(false);
                nextSpawnCarPoint.Add(carControl.NumSpawnPoint);
                PlayersGarage.Instance.UsingCarTrip(car.GetComponent<CarPassport>().PassportCarID);
                Invoke("SpawnCarWrapper", 4f);
            }
        }
    }

    public void CarDestroing(GameObject car)
    {
        cars.Remove(car);
    }

    private void SpawnCarWrapper()
    {
        int nextSpawnPoint;
        if (nextSpawnCarPoint.Count > 0)
        {
            nextSpawnPoint = nextSpawnCarPoint[0];
            nextSpawnCarPoint.RemoveAt(0);
            if (nextSpawnPoint >= 0 && nextSpawnPoint < spawnCars.Length) SpawnCar(nextSpawnPoint);
        }
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
        levelInfo.AddOrders(1, ui_Control);
        CarControl carControl = car.GetComponent<CarControl>();
        if (carControl != null)
        {
            if (isFull)
            {
                carControl.CarToWay();
                nextSpawnCarPoint.Add(carControl.NumSpawnPoint);
                PlayersGarage.Instance.UsingCarTrip(car.GetComponent<CarPassport>().PassportCarID);
                Invoke("SpawnCarWrapper", 4f);
                clocks[carControl.NumSpawnPoint].GetComponent<ClockControl>().StopTimer();
                clocks[carControl.NumSpawnPoint].SetActive(false);
                levelInfo.AddCars(1, ui_Control);
                levelInfo.AddExp(carControl.ExpCar, ui_Control);
                levelInfo.AddMany(carControl.PriceCar, ui_Control);
            }
            else
            {
                if (carControl.GetBoxPercent() > 50)
                {
                    clocks[carControl.NumSpawnPoint].SetActive(true);
                    clocks[carControl.NumSpawnPoint].GetComponent<ClockControl>().StartTimer(delayCarToWay);
                }
            }
        }
        if (levelInfo.TestFinish())
        {
            GameManager.Instance.currentPlayer.sessionGold = levelInfo.Many;
            GameManager.Instance.currentPlayer.sessionScore = levelInfo.Exp;
            GameManager.Instance.currentPlayer.LevelExpAndManyUpdate();
            GameManager.Instance.currentPlayer.LevelComplete();
            GameManager.Instance.SaveGame();
            PlayersGarage.Instance.SetGarageCsvString(PlayersGarage.Instance.GarageToCsvString());
            ui_Control.ViewEndLevelPanel();
        }
        SpawnOrder();
    }

    public void AddRewardedGold(int value)
    {
        GameManager.Instance.currentPlayer.totalGold += value;
        GameManager.Instance.SaveGame();
        ui_Control.ViewEndMany(levelInfo.Many * 2);
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
