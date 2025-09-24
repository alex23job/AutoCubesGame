using System.Text;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxTrigger : MonoBehaviour
{
    [SerializeField] private int row;   //  size Y
    [SerializeField] private int col;   //  size X
    [SerializeField] private int[] numsOccupiedCells;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    private Vector3 pos00, posEND;
    private int[] pole;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GenerateGrid();
        SetBeginAndEndPos();
        CreatePole();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBeginAndEndPos()
    {
        pos00 = transform.position;
        pos00.x += 100 * offsetX + 0.7f;pos00.y += 100 * offsetY;
        posEND = pos00;
        posEND.x -= 0.8f * col;posEND.y -= 0.8f * row;
        //pos00.x += 0.3f; pos00.y += 0.3f;
    }

    private void CreatePole()
    {
        pole = new int[row * col];
        int i, j;
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                if (CheckOccupied(j, i)) pole[i * col + j] = -1;
                else pole[i * col + j] = 0;
            }
        }
        PrintPole();
    }

    private void PrintPole()
    {
        StringBuilder sb = new StringBuilder($"pole=<");
        for (int i = 0;i < row * col;i++)
        {
            if (i == 0) sb.Append($"{pole[i]}");
            else sb.Append($" {pole[i]}");
        }
        sb.Append(">");
        //print($"pole=<{pole[0]} {pole[1]} {pole[2]} {pole[3]} {pole[4]} {pole[5]} {pole[6]} {pole[7]} {pole[8]} {pole[9]} {pole[10]} {pole[11]} {pole[12]} {pole[13]} {pole[14]} {pole[15]} {pole[16]} {pole[17]} {pole[18]} {pole[19]}>");
        print(sb.ToString());
    }

    private bool CheckOccupied(int x, int y)
    {
        for (int i = 0; i < numsOccupiedCells.Length; i++) 
        {
            if (numsOccupiedCells[i] == 10 * y + x) return true;
        }
        return false;
    }

    private void GenerateGrid()
    {
        int i, j;
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.localScale = new Vector3(0.8f, 0.8f, 1.6f);
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(offsetX - j * 0.007f, offsetY - i * 0.007f, 0);
            }
        }
    }

    public bool TestPacking(GameObject go)
    {
        Vector3 goPos = go.transform.position, delta;        
        print($"Test in box => pos00=<{pos00}> goPos=<{goPos}> posEND=<{posEND}>");
        if (((goPos.x < pos00.x + 0.5f) && (goPos.x > posEND.x)) && ((goPos.y < pos00.y + 0.7f) && (goPos.y > posEND.y)))
        {
            //goPos.x -= 0.4f; goPos.y += 0.4f;
            goPos.x -= 0.2f; goPos.y += 0.2f;
            delta = pos00 - goPos;
            //int x = Mathf.RoundToInt(delta.x / 0.82f);
            //int y = Mathf.RoundToInt(delta.y / 0.82f);
            int x = Mathf.RoundToInt(delta.x / 0.8f);
            int y = Mathf.RoundToInt(delta.y / 0.8f);
            int[] shema = go.GetComponent<Order>().GetShema();
            print($"in box delta={delta} x={x} y={y}");
            if (CheckPacking(shema, x, y))
            {
                PackingToPole(shema, x, y);
                go.transform.position = new Vector3(pos00.x - 0.8f * x, pos00.y - 0.8f * y, pos00.z);
                //print($"go.transform.position={go.transform.position}");
                go.transform.parent = transform;
                go.GetComponent<Order>().SetPacking();
                //print($"go.transform.position={go.transform.position} locPos={go.transform.localPosition} parent={go.transform.parent}({go.transform.parent.name})");
                CarControl carControl = transform.parent.gameObject.GetComponent<CarControl>();
                if (carControl != null)
                {
                    carControl.PackingOrder(go, CheckFullBox());
                }
                return true;
            }
        }
        //go.transform.localPosition = new Vector3(offsetX - j * 0.007f, offsetY - i * 0.007f, 0);
        //print($"TestPacking pos00={pos00} order={go.name} delta={delta}");

        return false;
    }

    private bool CheckFullBox()
    {
        bool isFull = true;
        for (int i = 0; i < pole.Length; i++) 
        {
            if (pole[i] == 0) return false;
        }
        return isFull;
    }

    private bool CheckPacking(int[] sh, int x, int y)
    {
        int i, sx, sy, index;
        bool isPacking = true;
        for (i = 0; i < sh.Length; i++)
        {
            if (sh[i] == 1)
            {
                sx = i % 4 + x - 2;
                if ((sx < 0) || (sx >= col)) return false;
                sy = i / 4 + y - 1;
                if ((sy < 0) || (sy >= row)) return false;
                index = col * sy + sx;
                if (index < 0 || index >= col * row) return false;
                if (pole[index] != 0) return false;
            }
        }
        return isPacking;
    }

    private void PackingToPole(int[] sh, int x, int y)
    {
        int i, sx, sy;
        for (i = 0; i < sh.Length; i++)
        {
            if (sh[i] == 1)
            {
                sx = i % 4 + x - 2;
                sy = i / 4 + y - 1;
                pole[col * sy + sx] = 1;
            }
        }
        PrintPole();
    }

    public int GetFullPercent()
    {
        int i, count = 0, total = 0;
        for (i = 0; i < pole.Length; i++)
        {
            if (pole[i] == 1) count++;
            if (pole[i] != -1) total++;
        }
        return (count * 100) / total;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("order"))
        {
            Order order = other.GetComponent<Order>();
            if (order != null)
            {
                order.SetBoxTrigger(GetComponent<BoxTrigger>());
            }
        }
    }
}
