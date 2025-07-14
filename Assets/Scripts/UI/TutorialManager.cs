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

    void Start()
    {
        MovementTutorial();
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

    public void MovementTutorial()
    {
        if (!movementShown)
        {
            Pause();
            movement.SetActive(true);  
        }  
    }

    public void MovementTutorialDone()
    {
        movementShown = true;
    }

    public void NPCTutorial()
    {
        if (!npcTutorialShown)
        {
            Pause();
            npc.SetActive(true);
        }
    }

    public void NPCTutorialDone()
    {
        npcTutorialShown = true;
    }


    public void KitchenTutorial()
    {
        if (!kitchenTutorialShown)
        {
            Pause();
            kitchen.SetActive(true);
        }
    }

    public void KitchenTutorialDone()
    {
        kitchenTutorialShown = true;
    }

    public void ServeNPCTutorial()
    {
        if (!serveNPCTutorialShown)
        {
            Pause();
            serve.SetActive(true);
        }
    }
    public void ServeNPCTutorialDone()
    {
        serveNPCTutorialShown = true;
        tutorialDone = true;
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
