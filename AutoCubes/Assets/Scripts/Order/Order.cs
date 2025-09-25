using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Order : MonoBehaviour
{
    [SerializeField] private bool isTurn = false;
    [SerializeField] private int shemaID = 0;
    [SerializeField] private int countCeils = 1;

    private ShemaOrder shemaOrder;
    private Vector3 target;
    private Vector3 startPos;
    private Vector3 deltaPos = Vector3.zero;
    private bool isMove = false;
    private bool isMovement = false;
    private float moveSpeed = 10f;
    private BoxTrigger boxTrigger = null;
    private bool isPacking = false;
    private int znak = 0;
    private int termo = 0;

    public bool IsPacking { get { return isPacking; } }
    public int CountCeils { get { return countCeils; } }
    public int Znak { get => znak; }
    public int Termo { get => termo; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shemaOrder = new ShemaOrder(ShemaOrder.GetShemaOrder(shemaID).GetShema());
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovement)
        {
            if (isPacking) print($"isMovement={isMovement}");
            Vector3 delta = transform.position - target;
            if (delta.magnitude > 0.2f)
            {
                Vector3 movement = delta.normalized * moveSpeed * Time.deltaTime;
                Vector3 dm = transform.position - movement - target;
                if (dm.magnitude > 0.2f) transform.position -= movement;
                else
                {
                    transform.position = target;
                    isMovement = false;
                }
            }
            else
            {
                transform.position = target;
                isMovement = false;
            }
        }

        if (isMove)
        {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 figPos = transform.position;
            //figPos.x += mp.x - deltaPos.x; figPos.z += 1.35f * (mp.z - deltaPos.z);
            figPos.x += mp.x - deltaPos.x; figPos.y += mp.y - deltaPos.y;
            transform.position = figPos;
            deltaPos = mp;
        }
    }

    public int[] GetShema()
    {
        return shemaOrder.GetShema();
    }

    public void SetTarget(Vector3 tg)
    {
        target = tg;
        isMovement = true;
    }

    public void SetZnak(int zn, int termo)
    {
        znak = zn;
        this.termo = termo;
    }

    public void SetPacking()
    {
        isPacking = true;
    }

    public void SetBoxTrigger(BoxTrigger bt)
    {
        boxTrigger = bt;
    }

    private void OnMouseDown()
    {
        if (isPacking) return;
        if (Input.GetMouseButtonDown(0))
        {
            startPos = transform.position;
            isMove = true;
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            deltaPos = mp;
        }
    }

    private void OnMouseUp()
    {
        if (isPacking) return;
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 delta = startPos - transform.position;
            if (delta.magnitude < 0.1f && isTurn)
            {
                shemaOrder.Rotate90();
                transform.Rotate(0, 0, 90, Space.World);
                transform.position = startPos;
                //return;
                /*Vector3 rot = transform.rotation.eulerAngles;
                rot.z += 90f;rot.z = Mathf.RoundToInt(rot.z) % 360;
                transform.rotation = Quaternion.Euler(rot);*/
            }
            //print($"OnMouseUp   isMovement={isMovement}");
            if (isMove)
            {
                if (boxTrigger != null && boxTrigger.TestPacking(gameObject))
                {
                    isPacking = true;isMovement = false;
                }
                else
                {
                    print($"OnMouseUp   isPacking={isPacking}");
                    if (isPacking == false) transform.position = startPos;
                }
                isMove = false;
                boxTrigger = null;
            }
        }
    }
}

public class ShemaOrder
{
    public static List<ShemaOrder> Orders = new List<ShemaOrder>();
    public static ShemaOrder GetShemaOrder(int id)
    {
        if (Orders.Count == 0)
        {
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0 }));
            Orders.Add(new ShemaOrder(new int[16] { 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }));
        }
        if ((id >= 0) && (id < Orders.Count))
        {
            return Orders[id];
        }
        return null;
    }
    private int[] shema = new int[16];
    private int angle = 0;

    public int Angle { get => angle; }

    public ShemaOrder() { }
    public ShemaOrder(int[] shemaArr)
    {
        for (int i = 0; i < shemaArr.Length; i++)
        {
            shema[i] = shemaArr[i];
        }
    }

    public int[] Rotate90()
    {
        angle += 90;
        angle %= 360;
        int[] tmp = new int[16];
        int i, x, y;
        for (i = 0; i < 16; i++) 
        {
            x = i % 4; y = i / 4;
            tmp[i] = shema[4 * (3 - x) + y];
        }
        for (i = 0; i < 16; i++) shema[i] = tmp[i];
        return tmp;
    }

    public int[] GetShema()
    {
        int[] tmp = new int[16];
        for (int i = 0; i < 16; i++) tmp[i] = shema[i];
        return tmp;
    }
}
