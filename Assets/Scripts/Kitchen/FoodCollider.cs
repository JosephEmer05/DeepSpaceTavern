using UnityEngine;

public class FoodCollider : MonoBehaviour
{
    public string colliderIngredient;
    public FoodSpawner foodSpawner;

    private void OnTriggerExit(Collider other)
    {
        FoodIngredientName foodIngredientName = other.gameObject.GetComponent<FoodIngredientName>();
        if (foodIngredientName != null)
        {
            string ingredientName = foodIngredientName.ingredientName;
            if (ingredientName == colliderIngredient)
            {
                foodSpawner.SpawnIngredient(ingredientName);
            }
        }
    }
}
