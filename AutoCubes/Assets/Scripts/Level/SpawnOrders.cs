using UnityEngine;

public class SpawnOrders : MonoBehaviour
{
    [SerializeField] private GameObject[] orderPrefabs;

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
        GameObject order = Instantiate(orderPrefabs[0], transform.position, Quaternion.identity);
        order.GetComponent<Order>().SetTarget(target);
        return order;
    }
}
