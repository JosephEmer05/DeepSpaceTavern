using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class TutorialManager : MonoBehaviour
{
    public GameObject movement;
    public bool npcTutorialShown = false;
    public GameObject npc;
    public bool kitchenTutorialShown = false;
    public GameObject kitchen;
    public PauseMenu pauseMenu;
    public EnterTavern enterTavern;
    public CameraSwitcher cameraSwitcher;

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

    public void KitchenTutorial()
    {
        if (!kitchenTutorialShown)
        {
            Pause();
            kitchen.SetActive(true);
            kitchenTutorialShown = true;
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
    public void ResumeKitchen()
    {
        pauseMenu.isPaused = false;
        pauseMenu.SetPauseState(false);
        cameraSwitcher.SwitchToFPS();
        cameraSwitcher.SwitchToKitchen();
        //pauseMenu.SetCursorState(false);
        //cameraSwitcher.SwitchToKitchen();
    }
}
