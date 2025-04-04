using UnityEngine;

public class FoodStatus : MonoBehaviour
{
    public bool onTray = false;
    public bool onTable = false;
    public bool transparentFood = false;
    public void GrabFood()
    {
        onTray = true;
    }

    public void ServeFood()
    {
        Debug.Log("Served Food");
        onTable = true;
    }

    public void TransparentFood()
    {
        transparentFood = true;
    }
}
