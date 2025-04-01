using UnityEngine;

public class NPC_Spawner : MonoBehaviour
{
    WaveManager waveManager;
    public GameObject[] NPCTypes;
    public GameObject spawnPoint;
    public bool spawn = false;


    void Awake()
    {
        waveManager = UnityEngine.Object.FindAnyObjectByType<WaveManager>();
        spawn = true;
    }
    void Update()
    {
        if (spawn)
        {
            spawn = false;
            Debug.Log("NPC Spawned");
            SpawnNPCs();   
        }
    }

    public void SpawnNPCs()
    {
        if (spawnPoint == null || NPCTypes.Length == 0 || waveManager == null)
        {
            Debug.LogError("Missing references! Check spawnPoint, NPCTypes, or waveManager.");
            return;
        }

        for (int i = 0; i < waveManager.customerNumber; i++)
        {
            Debug.Log(waveManager.customerNumber);
            int randomNPC = Random.Range(0, NPCTypes.Length);
            Instantiate(NPCTypes[randomNPC], spawnPoint.transform.position, Quaternion.identity, this.transform);
        }
    }
}
