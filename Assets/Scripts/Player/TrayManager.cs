using UnityEngine;

public class TrayManager : MonoBehaviour
{
    public int numSlots = 1;
    public GameObject[] traySlots;
    public GameObject trayShop;
    private ShopItem shopManager;
    private void Start()
    {
        shopManager = trayShop.GetComponent<ShopItem>();
    }

    private void Update()
    {
        numSlots = shopManager.currentStock+1;
        for (int i = 0; i < numSlots; i++)
        {
            traySlots[i].SetActive(true);
        }
    }
    public GameObject CheckTraySlot()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount == 0 && child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }

        Debug.Log("Your Tray is full");
        return null;
    }

    public GameObject CheckTrayFood(string foodTag)
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                Transform grandchild = child.GetChild(0);
                if (grandchild.tag == foodTag)
                {
                    return grandchild.gameObject;
                }
            }
        }
        return null;
    }

    public void AddTraySlot()
    {
        numSlots++;

        for (int i = 0; i < numSlots; i++)
        {
            traySlots[i].SetActive(true);
        }
    }
}
