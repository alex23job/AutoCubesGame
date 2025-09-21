using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    [SerializeField] private GameObject[] carPrefabs;

    private bool isLeft = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (transform.position.x < 0) isLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnCar()
    {
        if (transform.position.x < 0) isLeft = false; else isLeft = true;
        int numPrefab = Random.Range(0, carPrefabs.Length);
        GameObject car = Instantiate(carPrefabs[numPrefab], transform.position, Quaternion.identity);
        Vector3 target = transform.position;
        if (isLeft) { target.x -= 22f; }
        else { target.x += 27f; }
        print($"name={transform.name}  pos={transform.position}  isLeft={isLeft}   target={target}");
        car.GetComponent<CarControl>().MoveCar(isLeft, target);
        return car;
    }
}
