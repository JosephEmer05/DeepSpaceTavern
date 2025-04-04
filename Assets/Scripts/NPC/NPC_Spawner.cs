using UnityEngine;
using System.Collections;
using System;

public class NPC_Spawner : MonoBehaviour
{
    private WaveManager waveManager;
    public GameObject[] NPCTypes;
    public GameObject spawnPoint;
    public bool canSpawn = true;



    private void Awake()
    {
        waveManager = UnityEngine.Object.FindAnyObjectByType<WaveManager>();

        canSpawn = true;
    }

    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            Debug.Log("NPC Spawned");
            StartCoroutine(SpawnNPC());
        }
    }

    private IEnumerator SpawnNPC()
    {
        for (int i = 0; i < waveManager.customerNumber; i++)
        {
            int randomNPC = UnityEngine.Random.Range(0, NPCTypes.Length);
            Instantiate(NPCTypes[randomNPC], spawnPoint.transform.position, Quaternion.identity, this.transform);
            
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 4f));
        }
    }
}
 