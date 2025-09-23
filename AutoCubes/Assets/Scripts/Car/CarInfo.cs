using System;
using UnityEngine;

[Serializable]
public class CarInfo : MonoBehaviour
{
    [SerializeField] private int carID;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private float priceMult = 1;
    [SerializeField] private int termo = 0;

    public int CarID { get { return carID; } }
    public float OffsetX { get => offsetX; }
    public float OffsetY { get => offsetY; }

    public float PriceMult { get => priceMult; }
    public int Termo { get => termo; }

    public CarInfo() { }
    public CarInfo(int id)
    {
        this.carID = id;
    }
}
