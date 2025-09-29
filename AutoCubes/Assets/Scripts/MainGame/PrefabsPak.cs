using UnityEngine;

public class PrefabsPak : MonoBehaviour
{
    [SerializeField] private GameObject[] carPrefabs;

    private string[] carNamesEn = { "Rheinmax HeavyDuty", "SilverWolf Freighter", "TerraGuard CargoPro", "Granite Rock", "Atlas Prime", "Northern Bear", "Voltair Electric" };
    private string[] carNamesRu = { "Сверхпрочный феникс", "Серебряный волк", "Норвежский тюлень", "Гранитная скала", "Серый буйвол", "Белый медведь", "Быстрый электрон" };
    private string[] itemNamesEn = { "Title", "Body type", "Max.velocity", "Tonnage", "Resource (trips)" };
    private string[] itemNamesRu = { "Название", "Тип кузова", "Макс.скорость", "Вместимость", "Ресурс (рейсы)" };
    private string[] boxTypeEn = { "not airtight", "airtight", "refrigerator" };
    private string[] boxTypeRu = { "не герметичный", "герметичный", "холодильник" };


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

    public string GetCarName(int index, string lang)
    {
        if (index >= 0 && index <= carNamesEn.Length)
        {
            return (lang == "ru") ? carNamesRu[index] : carNamesEn[index]; 
        }
        return "------- -------";
    }

    public string GetItemName(int index, string lang)
    {
        if (index >= 0 && index <= itemNamesEn.Length)
        {
            return (lang == "ru") ? itemNamesRu[index] : itemNamesEn[index];
        }
        return "-------";
    }

    public string GetBoxType(int index, string lang)
    {
        if (index >= 0 && index <= boxTypeEn.Length)
        {
            return (lang == "ru") ? boxTypeRu[index] : boxTypeEn[index];
        }
        return "-------";
    }
}
