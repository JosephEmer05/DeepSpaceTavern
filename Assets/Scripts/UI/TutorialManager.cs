using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public GameObject movement;
    public bool npcTutorialShown = false;
    public GameObject npc;
    public PauseMenu pauseMenu;
    public EnterTavern enterTavern;

    private bool coroutineStarted = false;

    void Start()
    {
        movement.SetActive(true);
        Pause();
    }

    void Update()
    {
        if (enterTavern.enteredTavern && !npcTutorialShown && !coroutineStarted)
        {
            StartCoroutine(WaitAndShowNPCTutorial());
            coroutineStarted = true;
        }
    }

    IEnumerator WaitAndShowNPCTutorial()
    {
        yield return new WaitForSeconds(3f);
        NPCTutorial();
    }

    public void NPCTutorial()
    {
        if (!npcTutorialShown)
        {
            Pause();
            npc.SetActive(true);
            npcTutorialShown = true;
        }
    }

    public void Pause()
    {
        pauseMenu.Pause();
        pauseMenu.pauseMenuUI.SetActive(false);
    }

    public void Resume()
    {
        pauseMenu.isPaused = false;
        pauseMenu.SetPauseState(false);
    }
}
