using UnityEngine;

public class SpawnOrders : MonoBehaviour
{
    [SerializeField] private GameObject[] orderPrefabs;
    [SerializeField] private GameObject[] znakPrefabs;

    private bool isMarkerZnak = false;
    private int termo = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnOrder(Vector3 target)
    {
        int num = Random.Range(0, orderPrefabs.Length);        
        GameObject order = Instantiate(orderPrefabs[num], transform.position, Quaternion.identity);
        if (isMarkerZnak)
        {
            int numZnak = Random.Range(0, 8);
            if (numZnak < 2)
            {
                GameObject znak = Instantiate(znakPrefabs[numZnak]);
                znak.transform.parent = order.transform;
                znak.transform.localPosition = new Vector3(-0.4f, 0.4f, 0.797f);
                znak.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                order.GetComponent<Order>().SetZnak(numZnak + 1, termo);
            }
        }
        order.GetComponent<Order>().SetTarget(target);
        return order;
    }

    public void SetMarkering(bool zn, int termo)
    {
        isMarkerZnak = zn;
        this.termo = termo;
    }
}
