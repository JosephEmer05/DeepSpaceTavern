using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOrganizer : MonoBehaviour
{
    NPC_Controller npcController;

    public GameObject[] linedNPC; 

    private void Awake()
    {
        npcController = UnityEngine.Object.FindAnyObjectByType<NPC_Controller>();
        linedNPC = new GameObject[1];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            var npc = other.GetComponent<NPC_Controller>();
            if (npc != null && !npc.isLeaving)
            {
                linedNPC[0] = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            var npc = other.GetComponent<NPC_Controller>();
            if (npc != null && !npc.isLeaving)
            {
                // Only clear if the exiting NPC is the same one in the array
                if (linedNPC[0] == other.gameObject)
                {
                    linedNPC[0] = null;
                }
            }
        }
    }
}
