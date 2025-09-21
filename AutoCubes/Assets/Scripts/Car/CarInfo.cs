using System;
using UnityEngine;

[Serializable]
public class CarInfo : MonoBehaviour
{
    [SerializeField] private int carID;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    public int CarID { get { return carID; } }
    public float OffsetX { get => offsetX; }
    public float OffsetY { get => offsetY; }

    public CarInfo() { }
    public CarInfo(int id)
    {
        this.carID = id;
    }
}
