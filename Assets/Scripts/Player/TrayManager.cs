using UnityEngine;

public class TrayManager : MonoBehaviour
{
    public GameObject CheckTraySlot()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount == 0 && child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }

        Debug.Log("Your Tray is full");
        return null;
    }

    public GameObject CheckTrayFood(string foodTag)
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                Transform grandchild = child.GetChild(0);
                if (grandchild.tag == foodTag)
                {
                    return grandchild.gameObject;
                }
            }
        }

        Debug.Log("Your Tray is full");
        return null;
    }
}
