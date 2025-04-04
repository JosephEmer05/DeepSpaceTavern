using UnityEngine;

public class FoodValueManager : MonoBehaviour
{
    public int beerCost = 1;
    public int stewCost = 2;
    public int burgerCost = 3;
    public int meatCost = 4;

    public int GetFoodValue(GameObject food)
    {
        if (food.CompareTag("Beer"))
        {
            return beerCost;
        }
        if (food.CompareTag("Stew"))
        {
            return stewCost;
        }
        if (food.CompareTag("Burger"))
        {
            return burgerCost;
        }
        if (food.CompareTag("Meat"))
        {
            return meatCost;
        }
        else
        {
            return 0;
        }
    }
}
