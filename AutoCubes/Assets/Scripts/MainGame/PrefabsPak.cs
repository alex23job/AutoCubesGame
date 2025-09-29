using UnityEngine;

public class PrefabsPak : MonoBehaviour
{
    [SerializeField] private GameObject[] carPrefabs;



    public static PrefabsPak Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetCarPrefab(int index)
    {
        if (index >= 0 && index < carPrefabs.Length)
        {
            return carPrefabs[index];
        }
        return null;
    }
}
