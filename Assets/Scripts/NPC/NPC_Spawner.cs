using UnityEngine;
using System.Collections;

public class NPC_Spawner : MonoBehaviour
{
    private WaveManager waveManager;
    private ChairManager chairManager;
    public GameObject[] NPCTypes;
    public GameObject spawnPoint;
    public bool canSpawn = false;  // default to false until a wave starts

    private Coroutine spawnCoroutine;

    private void Awake()
    {
        waveManager = UnityEngine.Object.FindAnyObjectByType<WaveManager>();
        chairManager = UnityEngine.Object.FindAnyObjectByType<ChairManager>();
    }

    void Update()
    {
        // Only start spawning if allowed and not already running
        if (canSpawn && spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnNPC());
        }
    }

    private IEnumerator SpawnNPC()
    {
        int totalNPCSpawn = waveManager.customerNumber;
        int totalSpawned = 0;

        while (totalSpawned < totalNPCSpawn)
        {
            if (!canSpawn)
            {
                Debug.Log("Spawning interrupted. Exiting early.");
                spawnCoroutine = null;
                yield break;
            }

            yield return null;

            if (chairManager.CheckAvailableChairToSpawn())
            {
                // Add another check after delay before instantiating
                yield return new WaitForSeconds(Random.Range(6f, 7f));

                if (!canSpawn)
                {
                    Debug.Log("Spawning interrupted right before instantiation.");
                    spawnCoroutine = null;
                    yield break;
                }

                int randomNPC = UnityEngine.Random.Range(0, NPCTypes.Length);
                Instantiate(NPCTypes[randomNPC], spawnPoint.transform.position, Quaternion.identity, this.transform);
                totalSpawned++;
            }
            else
            {
                yield return null;
            }
        }

        canSpawn = false;
        spawnCoroutine = null;
    }



    public void ResetSpawner()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        canSpawn = true;
    }
}
