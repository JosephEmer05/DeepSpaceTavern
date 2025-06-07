using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPC_Movement npcMovement;
    public NPC_Movement Movement => npcMovement;

    
    [SerializeField] private ChairManager chairManager;
    public ChairManager Chair => chairManager;

    private void Awake()
    {
        // Automatically assign NPC_Movement if not set in Inspector
        if (npcMovement == null)
        {
            npcMovement = GetComponent<NPC_Movement>();
        }

        if (chairManager == null)
        {
            chairManager = FindFirstObjectByType<ChairManager>();
        }
    }
}
