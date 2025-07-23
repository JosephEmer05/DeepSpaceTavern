using UnityEngine;

public class ShopAudio : MonoBehaviour
{
    public AudioSource scifi;
    public AudioSource tavern;
    public AudioSource shop;

    public float fadeSpeed = 1f;

    private float scifiTargetVolume;
    private float tavernTargetVolume;
    private float shopTargetVolume;

    public bool playerInShop = false;

    void Start()
    {
        scifiTargetVolume = scifi.volume;
        tavernTargetVolume = tavern.volume;
        shopTargetVolume = shop.volume;
    }

    void Update()
    {
        scifi.volume = Mathf.Lerp(scifi.volume, scifiTargetVolume, Time.deltaTime * fadeSpeed);
        tavern.volume = Mathf.Lerp(tavern.volume, tavernTargetVolume, Time.deltaTime * fadeSpeed);
        shop.volume = Mathf.Lerp(shop.volume, shopTargetVolume, Time.deltaTime * fadeSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShop = true;
            scifiTargetVolume = 0f;
            tavernTargetVolume = 0f;
            shopTargetVolume = 1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShop = false;
            scifiTargetVolume = 0f;
            tavernTargetVolume = 0.5f;
            shopTargetVolume = 0f;
        }
    }

}
