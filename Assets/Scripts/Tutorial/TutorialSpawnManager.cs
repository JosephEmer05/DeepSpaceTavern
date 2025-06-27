using UnityEngine;
using System.Collections;
using System;

public class TutorialSpawnManager : MonoBehaviour
{
    private WaveManager waveManager;
    private ChairManager chairManager;
    public GameObject[] NPCTypes;
    public GameObject spawnPoint;
    public bool canSpawn;

    private Coroutine spawnCoroutine;




    private void Awake()
    {
        chairManager = UnityEngine.Object.FindAnyObjectByType<ChairManager>();
        canSpawn = false;
}

    void Update()
    {
        if (canSpawn && spawnCoroutine == null)
        {
            canSpawn = false;
            spawnCoroutine = StartCoroutine(SpawnNPC());
        }
    }

    private IEnumerator SpawnNPC()
    {
        int totalNPCSpawn = 4;
        int totalSpawned = 0;

        while (totalSpawned < totalNPCSpawn)
        {
            if (chairManager.CheckAvailableChairToSpawn())
            {
                int randomNPC = UnityEngine.Random.Range(0, NPCTypes.Length);
                Instantiate(NPCTypes[randomNPC], spawnPoint.transform.position, Quaternion.identity, this.transform);
                totalSpawned++;

                yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 0.5f));
            }
            else
            {
                yield return null;
            }
        }

        canSpawn = false;
    }
}
