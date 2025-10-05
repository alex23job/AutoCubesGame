using UnityEngine;

public class Order3D : MonoBehaviour
{
    [SerializeField] private bool isTurn = false;
    [SerializeField] private int shemaID = 0;
    [SerializeField] private int countCeils = 1;
    [SerializeField] private int cx = 1;
    [SerializeField] private int cy = 1;

    private ShemaOrder shemaOrder;
    private Vector3 target;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 deltaPos = Vector3.zero;
    private bool isMove = false;
    private bool isMovement = false;
    private float moveSpeed = 10f;
    private RemovalBox boxTrigger = null;
    private GameObject gridBox = null;
    private bool isPacking = false;
    private Rigidbody rb;

    public bool IsPacking { get { return isPacking; } }
    public int CountCeils { get { return countCeils; } }

    public int CX { get { return cx; } }
    public int CY { get { return cy; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shemaOrder = new ShemaOrder(ShemaOrder.GetShemaOrder(shemaID).GetShema());
        Invoke("SetIsKinematic", 0.5f);
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
                    //transform.position = target;
                    transform.position = startPos;
                    isMovement = false;
                }
            }
            else
            {
                //transform.position = target;
                transform.position = startPos;
                isMovement = false;
            }
        }

        if (isMove)
        {
            Vector3 mip = Input.mousePosition;
            mip.z = 5f; // mip.y; mip.y = 5f;
            Vector3 mp = Camera.main.ScreenToWorldPoint(mip);
            //Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 figPos = transform.position;
            //figPos.x += mp.x - deltaPos.x; figPos.z += 1.35f * (mp.z - deltaPos.z);
            figPos.x += 2f * (mp.x - deltaPos.x); figPos.z += 2f * (mp.z - deltaPos.z);
            transform.position = figPos;
            deltaPos = mp;
        }

    }

    private void SetIsKinematic()
    {
        if (startPos == Vector3.zero) startPos = transform.position;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void ResetIsKinematic(Vector3 endPos)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        isPacking = true;
        this.endPos = endPos;
        Invoke("CorecturePosition", 1.05f);
        Invoke("SetIsKinematic", 1f);
    }

    private void CorecturePosition()
    {
        transform.position = endPos;
    }

    public int[] GetShema()
    {
        return shemaOrder.GetShema();
    }

    public void Restart()
    {
        Vector3 delta = transform.position - startPos;
        if (delta.magnitude < 0.1f) return;
        target = new Vector3(transform.position.x, startPos.y, transform.position.z);
        isMovement = true;
        isPacking = false;
    }

    public void SetTarget(Vector3 tg)
    {
        target = tg;
        isMovement = true;
    }

    public void SetBoxTrigger(RemovalBox bt, GameObject grid)
    {
        boxTrigger = bt;
        gridBox = grid;
    }


    private void OnMouseDown()
    {
        if (isPacking)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            startPos = transform.position;
            isMove = true;
            Vector3 mip = Input.mousePosition;
            mip.z = 5f; // mip.y; mip.y = 5f;
            Vector3 mp = Camera.main.ScreenToWorldPoint(mip);
            //print($"mp={mp}  inputMousePos={Input.mousePosition}");
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
                transform.Rotate(0, 90, 0, Space.World);
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
                    isPacking = true; isMovement = false;
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
