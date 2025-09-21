using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private SpawnCars[] spawnCars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnCar(0);
        SpawnCar(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnCar(int numSpawnPoint)
    {
        spawnCars[numSpawnPoint].SpawnCar();
    }
}
