using UnityEngine;

public class ReachedExit : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private void OnTriggerEnter(Collider other)
    {
        playerHealth.LoseLife();
        Destroy(other.gameObject);
    }
}
