using System;
using UnityEngine;

[Serializable]
public class CarInfo : MonoBehaviour
{
    [SerializeField] private int carID;

    public int CarID { get { return carID; } }

    public CarInfo() { }
    public CarInfo(int id)
    {
        this.carID = id;
    }
}
