using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class TutorialManager : MonoBehaviour
{
    public static bool movementShown = false;
    public GameObject movement;
    public static bool npcTutorialShown = false;
    public GameObject npc;
    public static bool kitchenTutorialShown = false;
    public GameObject kitchen;
    public static bool serveNPCTutorialShown = false;
    public GameObject serve;
    public static bool tutorialDone = false;

    public PauseMenu pauseMenu;
    public EnterTavern enterTavern;
    public CameraSwitcher cameraSwitcher;

    private bool coroutineStarted = false;

    public bool inTutorial = false;

    public GameObject controlsPrompt;

    void Start()
    {
        MovementTutorial();
        Pause();
        controlsPrompt.SetActive(false);
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

    public void MovementTutorial()
    {
        if (!movementShown)
        {
            inTutorial = true;
            Pause();
            movement.SetActive(true);  
        }  
    }

    public void MovementTutorialDone()
    {
        movementShown = true;
        inTutorial = false;
    }

    public void NPCTutorial()
    {
        if (!npcTutorialShown)
        {
            inTutorial = true;
            Pause();
            npc.SetActive(true);
        }
    }

    public void NPCTutorialDone()
    {
        npcTutorialShown = true;
        inTutorial = false;
        controlsPrompt.SetActive(true);
    }


    public void KitchenTutorial()
    {
        if (!kitchenTutorialShown)
        {
            inTutorial = true;
            Pause();
            kitchen.SetActive(true);
        }
    }

    public void KitchenTutorialDone()
    {
        kitchenTutorialShown = true;
        inTutorial = false;
    }

    public void ServeNPCTutorial()
    {
        if (!serveNPCTutorialShown)
        {
            inTutorial = true;
            Pause();
            serve.SetActive(true);
        }
    }
    public void ServeNPCTutorialDone()
    {
        serveNPCTutorialShown = true;
        tutorialDone = true;
        inTutorial = false;
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
