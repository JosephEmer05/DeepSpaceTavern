using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Ingredient
    {
        public string name;
        public GameObject prefab;
        public Transform spawnPoint;
    }

    public Ingredient[] ingredients;

    public void SpawnIngredient(string ingredientName)
    {
        foreach (var ingredient in ingredients)
        {
            if (ingredient.name == ingredientName && ingredient.prefab != null && ingredient.spawnPoint != null)
            {
                Instantiate(ingredient.prefab, ingredient.spawnPoint.position, Quaternion.identity);
                return;
            }
        }
        Debug.LogWarning("Ingredient not found or not set properly: " + ingredientName);
    }
}
