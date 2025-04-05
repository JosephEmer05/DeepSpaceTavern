using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public GameObject[] starsLife;

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("Game Over Screen");
            SceneManager.LoadScene("EndScene");
        }
        if (health >= 5)
        {
            health = 5;
        }
        
        DisplayLife();
    }

    public void LoseLife()
    {
        health -= 1;
    }

    public void GainLife()
    {
        health += 1;
    }

    public void DisplayLife()
    {
        for (int i = 0; i < starsLife.Length; i++)
        {
            starsLife[i].SetActive(false);
        }
        for (int i = 0; i < health; i++)
        {
            starsLife[i].SetActive(true);
        }
    }
}
