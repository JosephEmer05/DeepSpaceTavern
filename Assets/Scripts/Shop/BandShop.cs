using System.Collections.Generic;
using UnityEngine;

public class BandShop : MonoBehaviour
{

    public GameObject[] bandMembers;

    ShopItem shopManager;
    public GameObject bandItem;
    public int numSlots = 0;

    public List<NPC_Controller> npcControllers = new List<NPC_Controller>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopManager =  bandItem.GetComponent<ShopItem>();
    }

    // Update is called once per frame
    void Update()
    {
        npcControllers.Clear();
        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
        {
            NPC_Controller controller = npc.GetComponent<NPC_Controller>();
            if (controller != null)
            npcControllers.Add(controller);
        }
        numSlots = shopManager.currentStock;
        HireBandMember();
    }

    public void HireBandMember()
    {
        for (int i = 0; i < numSlots; i++)
        {
            bandMembers[i].SetActive(true);
            foreach (NPC_Controller controller in npcControllers)
            {
                controller.NPCBandBoost(numSlots);
            }
        }
    }
}
