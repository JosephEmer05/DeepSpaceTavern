using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Soup : MonoBehaviour
{
    public Sprite empty;
    public Sprite half;
    public Sprite full;

    public GameObject soupPrefab;
    public Transform spawnPoint;
    public Button serveButton;

    private SpriteRenderer spriteRenderer;
    public int soupFill = 0;
    private List<string> ingredients = new List<string>();

    public AudioPlayer player; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        serveButton.gameObject.SetActive(false);
        serveButton.onClick.AddListener(ServeSoup);
    }

    void OnTriggerEnter(Collider other)
    {
        string ingredientName = other.gameObject.name;

        if (ingredientName.Contains("Potatoes") || ingredientName.Contains("Carrot"))
        {
            if (!ingredients.Contains(ingredientName))
            {
                ingredients.Add(ingredientName);
                soupFill++;
                UpdateSoupSprite();
                Destroy(other.gameObject);
            }
        }
    }

    void UpdateSoupSprite()
    {
        player.PlayOneShotClip();
        if (soupFill == 1)
            spriteRenderer.sprite = half;
        else if (soupFill >= 2)
        {
            spriteRenderer.sprite = full;
            serveButton.gameObject.SetActive(true);
        }
    }

   public void ServeSoup()
    {
        player.StopAudio();
        if (soupFill >= 2)
        {
            GameObject newSoup = Instantiate(soupPrefab, spawnPoint.position, Quaternion.identity);

            ingredients.Clear();
            soupFill = 0;
            spriteRenderer.sprite = empty;
            serveButton.gameObject.SetActive(false);
        }
    }
}
