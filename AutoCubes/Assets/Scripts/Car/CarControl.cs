using UnityEngine;

[RequireComponent(typeof(CarInfo))]
public class CarControl : MonoBehaviour
{
    private CarInfo carInfo;
    private Wheel[] wheels;
    private WallBox wallBox;
    private float speed;
    private Vector3 target;
    private Vector3 spawnPosition;
    private bool isMove = false;
    private bool isForward = false;
    private LevelControl levelControl;
    private int numSpawnPoint = -1;

    public int NumSpawnPoint {  get { return numSpawnPoint; } }

    private void Awake()
    {
        carInfo = GetComponent<CarInfo>();
        wheels = transform.GetComponentsInChildren<Wheel>();
        wallBox = transform.GetComponentInChildren<WallBox>();
    }
    public int CarID { get { return carInfo.CarID; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 pos = transform.position;
        pos.y -= carInfo.OffsetY;
        transform.position = pos;
        spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            Vector3 pos = transform.position;
            Vector3 delta = pos - target;
            if (delta.magnitude > 0.25f)
            {
                Vector3 movement = delta.normalized * speed * Time.deltaTime;
                delta = pos + movement - target;
                if (delta.magnitude > 0.1f) pos += movement;
                else pos = target;
            }
            else pos = target;
            transform.position = pos;
            delta = transform.position - target;
            if (delta.magnitude < 0.05f)
            {
                isMove = false;
                delta = transform.position - spawnPosition;
                if (delta.magnitude > 2f)
                {
                    wallBox.WallOpen(true);
                    BoxTrigger boxTrigger = gameObject.GetComponentInChildren<BoxTrigger>();
                    if (boxTrigger != null) boxTrigger.SetBeginAndEndPos();
                }
                TurnWheels(0);
            }
        }

    }

    public void SetLevelControl(LevelControl lc, int numPoint)
    {
        levelControl = lc;
        numSpawnPoint = numPoint;
    }

    public void PackingOrder(GameObject order, bool isFull)
    {
        levelControl.PackingOrderToCar(gameObject, order, isFull);
    }

    public void CarToWay()
    {
        isForward = !isForward;
        wallBox.WallOpen(false);
        Invoke("MoveCarWrapper", 1f);
    }

    private void MoveCarWrapper()
    {
        MoveCar(isForward, spawnPosition, false);
        Destroy(gameObject, 15f);
    }

    public void MoveCar(bool isForward, Vector3 tg, bool isTargetCorrect = true)
    {
        this.isForward = isForward;
        if ( isTargetCorrect)
        {
            tg.x -= carInfo.OffsetX;
            tg.y -= carInfo.OffsetY;
        }
        target = tg;
        isMove = true;
        if (isForward)
        {
            TurnWheels(-1);
            speed = -5f;
        }
        else
        {
            TurnWheels(1);
            speed = -5f;
        }
        //print($"isForward={isForward}  speed={speed}   tg={tg}");
    }

    public void TurnWheels(int direction)
    {
        int i;
        switch (direction)
        {
            case 0:
                for (i = 0; i < wheels.Length; i++) wheels[i].TurnStop();
                break;
            case 1:
                for (i = 0; i < wheels.Length; i++) wheels[i].TurnForward();
                break;
            case -1:
                for (i = 0; i < wheels.Length; i++) wheels[i].TurnBackward();
                break;
        }
    }

    public void WallOpen(bool isOpen)
    {
        wallBox.WallOpen(isOpen);
    }

    public int GetBoxPercent()
    {
        BoxTrigger boxTrigger = gameObject.GetComponentInChildren<BoxTrigger>();
        return boxTrigger.GetFullPercent();
    }
}
