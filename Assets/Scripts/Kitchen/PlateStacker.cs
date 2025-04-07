using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlateStacker : MonoBehaviour
{
    public float stackHeightOffset = 0.15f;

    private List<Transform> stackedIngredients = new List<Transform>();

    private Dictionary<string, List<string>> recipes = new Dictionary<string, List<string>>()
    {
        { "Burger", new List<string> { "BottomBun", "Patty", "Cheese", "TopBun" } },
        { "Lamb", new List<string> { "Lamb", "Peas", "Mash" } },
        { "Stew", new List<string> { "Soup" } },
        { "Beer", new List<string> { "Mug" } } 
    };

    public GameObject burgerPrefab;
    public GameObject lambPrefab;
    public GameObject stewPrefab;
    public GameObject beerPrefab;

    public GameObject[] spawnPoints;
    public List<GameObject> spawnedDishes = new List<GameObject>();

    public Image resultImage; 

    public Sprite correctImage;
    public Sprite wrongOrderImage;
    public Sprite mugNotFilledImage;
    public Sprite unknownDishImage;
    public Sprite counterFullImage; 
    public Sprite rawFoodImage;

    public Transform[] ingredientPositions;

    public int numSlots = 4;
    public GameObject counterShop;
    private ShopItem shopManager;

    public AudioPlayer player;
    private void Start()
    {
        shopManager = counterShop.GetComponent<ShopItem>();
    }
    private void Update()
    {
        numSlots = shopManager.currentStock + 4;
        for (int i = 0; i < numSlots; i++)
        {
            spawnPoints[i].SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            StackIngredient(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ingredient") && stackedIngredients.Contains(other.transform))
        {
            stackedIngredients.Remove(other.transform);
            other.transform.SetParent(null);
        }
    }

    void StackIngredient(Transform ingredient)
    {
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        float newY = stackedIngredients.Count == 0
            ? transform.position.y + stackHeightOffset
            : stackedIngredients[stackedIngredients.Count - 1].position.y + stackHeightOffset;

        Vector3 newPos = new Vector3(transform.position.x, newY, transform.position.z);
        ingredient.position = newPos;
        ingredient.SetParent(transform);

        SpriteRenderer spriteRenderer = ingredient.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = stackedIngredients.Count;
        }

        stackedIngredients.Add(ingredient);
        Debug.Log("Stacked: " + ingredient.name);
    }

    public void CheckStack()
    {
        bool hasRawFood = false;

        foreach (Transform ingredient in stackedIngredients)
        {
            FoodItem foodItem = ingredient.GetComponent<FoodItem>();
            if (foodItem != null && foodItem.foodType == FoodItem.FoodType.Ingredient && !foodItem.isCooked)
            {
                hasRawFood = true;
                break;
            }
        }

        if (hasRawFood)
        {
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = rawFoodImage;
            StartCoroutine(HideResultImageAfterDelay(2f));
            return;
        }

        string detectedDish = DetectDishType();
        if (detectedDish == "")
        {
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = unknownDishImage; 
            StartCoroutine(HideResultImageAfterDelay(2f));
            return;
        }

        if (detectedDish == "Beer" && !CheckBeer())
        {
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = mugNotFilledImage;
            StartCoroutine(HideResultImageAfterDelay(2f));
            return;
        }

        if (detectedDish == "Burger" && !CheckBurgerOrder())
        {
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = wrongOrderImage;
            StartCoroutine(HideResultImageAfterDelay(2f));
            return;
        }

        if (GetNextAvailableSlot() == -1)
        {
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = counterFullImage;
            StartCoroutine(HideResultImageAfterDelay(2f));
            return;
        }

        resultImage.gameObject.SetActive(true);
        resultImage.sprite = correctImage;
        StartCoroutine(HideResultImageAfterDelay(2f));
        SpawnCompletedDish(detectedDish);
        player.PlayOneShotClip();
        ClearPlate();
    }

    IEnumerator HideResultImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultImage.gameObject.SetActive(false);
    }


    public void ShowResultImage()
    {
        resultImage.enabled = true;
    }


    string DetectDishType()
    {
        foreach (var recipe in recipes)
        {
            List<string> requiredIngredients = recipe.Value;
            List<string> plateIngredients = new List<string>();

            foreach (Transform ingredient in stackedIngredients)
            {
                plateIngredients.Add(ingredient.name);
            }

            if (plateIngredients.Count != requiredIngredients.Count)
                continue;

            bool allMatch = true;
            foreach (string required in requiredIngredients)
            {
                if (!plateIngredients.Exists(ing => ing.Contains(required)))
                {
                    allMatch = false;
                    break;
                }
            }

            if (allMatch)
                return recipe.Key;
        }
        return "";
    }

    bool CheckBurgerOrder()
    {
        List<string> correctOrder = recipes["Burger"];
        List<Transform> incorrectIngredients = new List<Transform>();
        List<int> incorrectIndices = new List<int>();

        for (int i = 0; i < stackedIngredients.Count; i++)
        {
            if (!stackedIngredients[i].name.Contains(correctOrder[i]))
            {
                for (int j = i; j < stackedIngredients.Count; j++)
                {
                    if (!incorrectIngredients.Contains(stackedIngredients[j]))
                    {
                        incorrectIngredients.Add(stackedIngredients[j]);
                        incorrectIndices.Add(j);
                    }
                }
                break;
            }
        }

        if (incorrectIngredients.Count > 0)
        {
            RandomizeIncorrectIngredientPositions(incorrectIngredients, incorrectIndices);
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = wrongOrderImage;
            StartCoroutine(HideResultImageAfterDelay(2f));
            return false;
        }
        return true;
    }

    void RandomizeIncorrectIngredientPositions(List<Transform> incorrectIngredients, List<int> incorrectIndices)
    {
        if (ingredientPositions.Length == 0) return;

        Shuffle(incorrectIngredients);
        for (int i = 0; i < incorrectIngredients.Count; i++)
        {
            int targetIndex = i % ingredientPositions.Length;
            StartCoroutine(MoveIngredientSmoothly(incorrectIngredients[i], ingredientPositions[targetIndex].position));
        }

        foreach (Transform ingredient in incorrectIngredients)
        {
            stackedIngredients.Remove(ingredient);
        }
    }

    IEnumerator MoveIngredientSmoothly(Transform ingredient, Vector3 targetPos)
    {
        Vector3 startPos = ingredient.position;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            ingredient.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ingredient.position = targetPos;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }


    bool CheckBeer()
    {
        foreach (Transform ingredient in stackedIngredients)
        {
            FoodItem foodItem = ingredient.GetComponent<FoodItem>();
            if (foodItem != null && foodItem.foodType == FoodItem.FoodType.Mug && foodItem.isCooked)
            {
                return true;
            }
        }
        return false;
    }

    void SpawnCompletedDish(string dish)
    {
        GameObject dishPrefab = null;
        if (dish == "Burger") dishPrefab = burgerPrefab;
        if (dish == "Lamb") dishPrefab = lambPrefab;
        if (dish == "Stew") dishPrefab = stewPrefab;
        if (dish == "Beer") dishPrefab = beerPrefab;

        if (dishPrefab != null)
        {
            int availableSlot = GetNextAvailableSlot();

            if(spawnPoints[availableSlot].activeSelf == true)
            {
                GameObject newDish = Instantiate(dishPrefab, spawnPoints[availableSlot].transform.position, Quaternion.identity);
                spawnedDishes.Insert(availableSlot, newDish);
            }
        }
    }

    int GetNextAvailableSlot()
    {
        int activeSlot = GameObject.FindGameObjectsWithTag("Counter Slot").Length;
        for (int i = 0; i < activeSlot; i++)
        {
            if (i >= spawnedDishes.Count || spawnedDishes[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public void RemoveDish(GameObject dish)
    {
        int index = spawnedDishes.IndexOf(dish);
        if (index != -1)
        {
            spawnedDishes[index]=null;
            spawnedDishes.RemoveAt(index);
            ShiftDishesForward();
        }
    }

    public void ShiftDishesForward()
    {
        for (int i = 0; i < spawnedDishes.Count; i++)
        {
            if (spawnedDishes[i] != null)
            {
                if (spawnPoints[i].activeSelf == true)
                {
                    spawnedDishes[i].transform.position = spawnPoints[i].transform.position;
                }
                
            }
        }
    }

    public void ClearPlate()
    {
        foreach (Transform ingredient in stackedIngredients)
        {
            ingredient.SetParent(null);
            Destroy(ingredient.gameObject);
        }
        stackedIngredients.Clear();
    }
}
