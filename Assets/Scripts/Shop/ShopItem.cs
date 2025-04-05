using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int maxStock;
    public int currentStock = 0;
    public bool IsSoldOut => currentStock >= maxStock;

    public GameObject info;
    public GameObject soldOut;

    void Start()
    {
        soldOut.SetActive(false);
    }

    public void BuyItem()
    {
        if (IsSoldOut) return;

        currentStock++;


        if (IsSoldOut)
        {
            DisableItem();
        }
    }

    private void DisableItem()
    {
        info.SetActive(false);
        soldOut.SetActive(true);
    }
}
