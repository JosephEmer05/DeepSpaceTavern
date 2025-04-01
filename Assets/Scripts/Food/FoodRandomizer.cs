using System;
using UnityEngine;


public class FoodRandomizer : MonoBehaviour
{
    public GameObject[] foodItems;

    public GameObject PickFood()
    {
        int randomNumber = UnityEngine.Random.Range(0, 4); 
        return foodItems[randomNumber];
    }
}
