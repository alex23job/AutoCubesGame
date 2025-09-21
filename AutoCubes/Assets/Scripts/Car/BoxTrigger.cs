using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TestPacking(GameObject go)
    {
        print($"TestPacking order={go.name}");
        return false;
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
