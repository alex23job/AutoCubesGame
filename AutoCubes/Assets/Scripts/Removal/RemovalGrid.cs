using UnityEngine;

public class RemovalGrid : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("order"))
        {
            other.gameObject.GetComponent<Order3D>().SetBoxTrigger(transform.parent.GetComponent<RemovalBox>(), gameObject);
        }
    }
}
