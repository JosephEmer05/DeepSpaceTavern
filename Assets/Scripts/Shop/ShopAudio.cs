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
    public bool shopControlsAudio = false;

    void Start()
    {
        scifiTargetVolume = scifi.volume;
        tavernTargetVolume = tavern.volume;
        shopTargetVolume = shop.volume;
    }

    void Update()
    {
        if (!shopControlsAudio) return;

        scifi.volume = Mathf.Lerp(scifi.volume, scifiTargetVolume, Time.deltaTime * fadeSpeed);
        tavern.volume = Mathf.Lerp(tavern.volume, tavernTargetVolume, Time.deltaTime * fadeSpeed);
        shop.volume = Mathf.Lerp(shop.volume, shopTargetVolume, Time.deltaTime * fadeSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShop = true;
            shopControlsAudio = true;

            scifiTargetVolume = 0f;
            tavernTargetVolume = 0f;
            shopTargetVolume = 1f;

            Debug.Log("Player entered shop");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetShopAudio();
        }
    }

    public void ResetShopAudio()
    {
        playerInShop = false;
        shopControlsAudio = false;

        scifiTargetVolume = 0f;
        tavernTargetVolume = 0.5f;
        shopTargetVolume = 0f;

        scifi.volume = scifiTargetVolume;
        tavern.volume = tavernTargetVolume;
        shop.volume = shopTargetVolume;

        Debug.Log("Shop audio reset.");
    }
}