using UnityEngine;

public class Recipe : MonoBehaviour
{
    public bool recipeOpen = false;
    public GameObject recipe;
    public CameraSwitcher carmeraSwitcher;
    public TutorialManager tutorialManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        carmeraSwitcher = UnityEngine.Object.FindAnyObjectByType<CameraSwitcher>();
    }

    private void Update()
    {
        ShowRecipe();
    }
    public void ShowRecipe()
    {
        if (carmeraSwitcher.inKitchen)
        {
            if (Input.GetKeyDown(KeyCode.R) && !recipeOpen)
            {
                tutorialManager.Pause();
                recipeOpen = true;
                recipe.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.R) && recipeOpen) 
            {
                tutorialManager.Resume();
                recipeOpen = false;
                recipe.SetActive(false);
            }
        }
    }
}
