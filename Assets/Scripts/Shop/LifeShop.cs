using TMPro;
using UnityEngine;

public class LifeShop : MonoBehaviour
{
    public int maxStock;
    public int currentStock = 0;
    public int itemCost;
    public int incrementCost;
    public bool IsSoldOut => currentStock >= maxStock;

    public GameObject info;
    public GameObject soldOut;

    PlayerHealth playerHealth;
    public GameObject player;

    public WaveManager waveManagerScript;
    public GameObject waveManagerObject;

    public TextMeshProUGUI itemCostText;
    void Start()
    {
        soldOut.SetActive(false);
        playerHealth = player.GetComponent<PlayerHealth>();
        waveManagerScript = waveManagerObject.GetComponent<WaveManager>();

    }

    public void CheckGoldOwned()
    {
        if (waveManagerScript.goldOwned >= itemCost)
        {
            BuyItem();
        }
    }

    public void BuyItem()
    {
        if (IsSoldOut) return;

        currentStock++;
        playerHealth.GainLife();
        waveManagerScript.GoldOwnedUpdater(-itemCost);
        IncrementItemCost();

        if (IsSoldOut)
        {
            DisableItem();
        }
    }

    public void IncrementItemCost()
    {
        itemCost += incrementCost;
        itemCostText.text = itemCost + "g";

    }

    private void DisableItem()
    {
        info.SetActive(false);
        soldOut.SetActive(true);
    }
}
