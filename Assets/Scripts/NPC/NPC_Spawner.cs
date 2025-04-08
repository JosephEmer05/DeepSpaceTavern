using UnityEngine;
using System.Collections;
using System;

public class NPC_Spawner : MonoBehaviour
{
    private WaveManager waveManager;
    private ChairManager chairManager;
    public GameObject[] NPCTypes;
    public GameObject spawnPoint;
    public bool canSpawn = true;



    private void Awake()
    {
        waveManager = UnityEngine.Object.FindAnyObjectByType<WaveManager>();
        chairManager = UnityEngine.Object.FindAnyObjectByType<ChairManager>();

        canSpawn = true;
    }

    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            StartCoroutine(SpawnNPC());
        }
    }

    private IEnumerator SpawnNPC()
    {
        int totalNPCSpawn = waveManager.customerNumber;
        int totalSpawned = 0;

        while (totalSpawned < totalNPCSpawn)
        {
            if (chairManager.CheckAvailableChairToSpawn())  
            {
                int randomNPC = UnityEngine.Random.Range(0, NPCTypes.Length);
                Instantiate(NPCTypes[randomNPC], spawnPoint.transform.position, Quaternion.identity, this.transform);
                totalSpawned++;

                yield return new WaitForSeconds(UnityEngine.Random.Range(6f, 7f));
            }
            else
            {
                yield return null;
            }
        }

        canSpawn = false;
    }

}
