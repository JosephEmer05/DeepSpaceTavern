using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public TutorialSpawnManager tutorialSpawnManager;
    public GameObject[] IntroSegment;
    public GameObject[] MovementSegment;
    public int currentIndex = 0;
    public Button next;
    public Animator anim;


    public void Intro()
    {
        if (IntroSegment.Length == 0) return;

        for (int i = 0; i < IntroSegment.Length; i++)
        {
            IntroSegment[i].SetActive(i == currentIndex + 1);
        }

        IntroSegment[currentIndex].SetActive(false);
        currentIndex++;

        if (currentIndex >= IntroSegment.Length)
        {
            next.interactable = false;
            tutorialSpawnManager.canSpawn = true;
            anim.SetTrigger("ZoomToCustomers");
            currentIndex = 0;
        }

    }
    public void Movement()
    {

        if (MovementSegment.Length == 0) return;

        for (int i = 0; i < MovementSegment.Length; i++)
        {
            MovementSegment[i].SetActive(i == currentIndex + 1);
        }

        MovementSegment[currentIndex].SetActive(false);
        currentIndex++;

        if (currentIndex >= MovementSegment.Length)
        {
            next.interactable = false;
            currentIndex = 0;
        }
    }

    void Start()
    {
        for (int i = 0; i < IntroSegment.Length; i++)
        {
            IntroSegment[i].SetActive(i == 0);
        }
        tutorialSpawnManager = GetComponent<TutorialSpawnManager>();

    }

}
