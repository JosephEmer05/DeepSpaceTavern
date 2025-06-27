using UnityEngine;
using UnityEngine.UI;

public class SetActiveCanvas : MonoBehaviour
{
    public TextManager textManager;
    public GameObject tutorialCanvas;
    public GameObject movementCanvas;

    public void ActivateTutorialCanvas()
    {
        tutorialCanvas.SetActive(true);
    }

    public void DeactivateTutorialCanvas()
    {
        tutorialCanvas.SetActive(false);
    }

    public void CustomerEnter()
    {
        movementCanvas.SetActive(true);
        for (int i = 0; i < textManager.MovementSegment.Length; i++)
        {
            textManager.MovementSegment[i].SetActive(i == 0);
        }
    }
    
}
