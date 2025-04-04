using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public GameObject[] starsLife;
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("Game Over Screen");
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
        for (int i = 0; i < starsLife.Length; i++)
        {
            starsLife[i].SetActive(false);
        }
    }

    public void GainLife()
    {
        health ++;
    }

    public void DisplayLife()
    {
        for (int i = 0; i<health; i++)
        {
            starsLife[i].SetActive(true);
        }
        
    }

}
