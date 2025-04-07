using UnityEngine;

public class TableShop : MonoBehaviour
{
    public GameObject[] table;
    public GameObject[] navMesh;

    public GameObject tableItem;
    private ShopItem shopManager;
    public int numSlots = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopManager = tableItem.GetComponent<ShopItem>();

    }

    // Update is called once per frame
    void Update()
    {
        numSlots = shopManager.currentStock;
        AddTable();
    }

    public void AddTable()
    {
        tableItem.SetActive(false);
        if (WaveManager.waveNumber >= 5)
        {
            tableItem.SetActive(true);
            for (int i = 0; i < numSlots; i++)
            {
                table[i].SetActive(true);
                navMesh[i].SetActive(false);
                navMesh[numSlots].SetActive(true);
            }
        }
        
    }
}
