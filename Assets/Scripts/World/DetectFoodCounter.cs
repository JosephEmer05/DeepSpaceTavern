using UnityEngine;

public class DetectFoodCounter : MonoBehaviour
{
    public GameObject foodOnCounter;
    public void OnTriggerEnter(Collider other)
    {
        foodOnCounter = other.gameObject;
    }
}
