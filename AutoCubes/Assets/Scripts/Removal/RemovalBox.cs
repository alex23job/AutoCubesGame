using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RemovalBox : MonoBehaviour
{
    [SerializeField] private int row;   //  size Z
    [SerializeField] private int col;   //  size X
    [SerializeField] private int layer; //  size Y
    [SerializeField] private int[] numsOccupiedCells;

    private Vector3 pos00, posEND, posGridCube11;
    private int[] pole3d = null;
    private int packingLayer = -1;
    private Transform grid = null;
    private RemovalControl removalControl = null;
    private int countCurrentCeils = 0;
    private GameObject currentOrder = null;
    private List<GameObject> undoOrders = new List<GameObject>();

    public int CountCeils { get { return row * col * layer - numsOccupiedCells.Length; } }
    public int CountCurrentCeils { get { return countCurrentCeils; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = transform.GetChild(1);
        pos00 = grid.position;
        pos00.x -= col / 2.0f;
        pos00.z += row / 2.0f;
        posEND = pos00;
        posEND.x += col;
        posEND.z -= row;
        posGridCube11 = grid.GetChild(0).position;
        CreatePole();
        removalControl.TranslateOrders(countCurrentCeils, CountCeils, false);
    }

    public void SetRemovalControl(RemovalControl remoControl)
    {
        removalControl = remoControl;
    }

    public void Restart()
    {
        countCurrentCeils = 0;
        removalControl.TranslateOrders(countCurrentCeils, CountCeils, false);
        undoOrders.Clear();
        currentOrder = null;
        CreatePole();
    }

    public void Undo()
    {
        if (currentOrder == null && undoOrders.Count > 0) 
        {
            currentOrder = undoOrders[undoOrders.Count - 1];            
        }
        if (currentOrder != null)
        {
            Vector3 delta = currentOrder.transform.position - posGridCube11;
            int x = Mathf.RoundToInt(delta.x + 0.5f);
            int y = Mathf.Abs(Mathf.RoundToInt(delta.z + 0.5f));
            //print($"Undo x={x} y={y} pos={currentOrder.transform.position} delta={delta}");
            Order3D order3D = currentOrder.GetComponent<Order3D>();
            UnPackingToPole(order3D.GetShema(), x, y);
            countCurrentCeils -= order3D.CountCeils;
            order3D.Restart();
            removalControl.TranslateOrders(countCurrentCeils, CountCeils);
            currentOrder = null;
            undoOrders.RemoveAt(undoOrders.Count - 1);
        }
        if (undoOrders.Count == 0) removalControl.ResetUndoButton();
    }

    public bool TestPacking(GameObject order)
    {
        Vector3 ordPos = order.transform.position;
        float dopOfs = 0.5f;
        if (((ordPos.x > pos00.x - dopOfs) && (ordPos.x < posEND.x + dopOfs)) && ((ordPos.z < pos00.z + dopOfs) && (ordPos.z > posEND.z - dopOfs)))
        {
            ordPos.x += 0.5f;ordPos.z += 0.5f;
            Vector3 delta = ordPos - posGridCube11;
            int x = Mathf.RoundToInt(delta.x);
            int y = Mathf.Abs(Mathf.RoundToInt(delta.z));
            int[] shema = order.GetComponent<Order3D>().GetShema();

            //print($"in box3D posCube11={posGridCube11} ordPos={ordPos} delta={delta} x(col)={x} y(row)={y}");
            if (CheckPacking(shema, x, y))
            {
                PackingToPole(shema, x, y);
                countCurrentCeils += order.GetComponent<Order3D>().CountCeils;
                removalControl.TranslateOrders(countCurrentCeils, CountCeils);
                ordPos = posGridCube11;
                ordPos.x += x - 0.5f;ordPos.z -= y + 0.5f;
                order.transform.position = ordPos;
                ordPos.y = packingLayer + 2.1f;
                order.GetComponent<Order3D>().ResetIsKinematic(ordPos);
                currentOrder = order;
                undoOrders.Add(currentOrder);
                if (removalControl != null && CheckFullBox())
                {
                    removalControl.BoxIsFull(CountCeils);
                }
            }
        }
        return false;
    }

    private bool CheckPacking(int[] sh, int x, int y)
    {
        int i, j, sx, sy, index, sz_layer = col * row;
        packingLayer = -1;
        bool isPacking = true;
        for (j = 0; j < layer; j++)
        {
            isPacking = true;
            for (i = 0; i < sh.Length; i++)
            {
                if (sh[i] == 1)
                {
                    sx = i % 4 + x - 2;
                    if ((sx < 0) || (sx >= col)) return false;
                    sy = i / 4 + y - 1;
                    if ((sy < 0) || (sy >= row)) return false;
                    index = col * sy + sx;
                    if (index < 0 || index >= sz_layer) return false;
                    if (pole3d[j * sz_layer + index] != 0) isPacking = false;
                }
            }
            if (isPacking)
            {
                packingLayer = j;
                break;
            }
        }
        return isPacking;
    }

    private void PackingToPole(int[] sh, int x, int y)
    {
        if (packingLayer == -1) return;
        int i, sx, sy, sz_layer = row * col;
        for (i = 0; i < sh.Length; i++)
        {
            if (sh[i] == 1)
            {
                sx = i % 4 + x - 2;
                sy = i / 4 + y - 1;
                pole3d[packingLayer * sz_layer + col * sy + sx] = 1;
            }
        }
        //PrintPole();
    }

    private void UnPackingToPole(int[] sh, int x, int y)
    {
        if (packingLayer == -1) return;
        int i, sx, sy, sz_layer = row * col;
        for (i = 0; i < sh.Length; i++)
        {
            if (sh[i] == 1)
            {
                sx = i % 4 + x - 2;
                sy = i / 4 + y - 1;
                pole3d[packingLayer * sz_layer + col * sy + sx] = 0;
            }
        }
        //PrintPole();
    }

    private bool CheckFullBox()
    {
        bool isFull = true;
        for (int i = 0; i < pole3d.Length; i++)
        {
            if (pole3d[i] == 0) return false;
        }
        return isFull;
    }


    private void CreatePole()
    {        
        int i, j, l, sz = row * col;
        if (pole3d == null) pole3d = new int[sz * layer];
        for (l = 0; l < layer; l++)
        {
            for (i = 0; i < row; i++)
            {
                for (j = 0; j < col; j++)
                {
                    if (CheckOccupied(j, i, l)) pole3d[l * sz + i * col + j] = -1;
                    else pole3d[l * sz + i * col + j] = 0;
                }
            }
        }
        //PrintPole();
    }

    private bool CheckOccupied(int x, int z, int y)
    {
        for (int i = 0; i < numsOccupiedCells.Length; i++)
        {
            if (numsOccupiedCells[i] == 100 * y + 10 * z + x) return true;
        }
        return false;
    }
    private void PrintPole()
    {
        StringBuilder sb = new StringBuilder($"pole=<");
        int sz = row * col;
        for (int i = 0; i < sz * layer; i++)
        {
            if (i % sz == 0) sb.Append($" L{i / sz} {pole3d[i]}");
            else if (i % col == 0) sb.Append($"  R{(i % sz) / col} {pole3d[i]}");
            else sb.Append($" {pole3d[i]}");
        }
        sb.Append(">");
        print(sb.ToString());
    }

}
