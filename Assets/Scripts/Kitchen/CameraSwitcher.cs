using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera kitchenCam;
    public Camera fpsCam;
    public GameObject playerCharacter;
    public Canvas ui;
    public GameObject Crosshair;
    public AudioSource tavern;
    public AudioSource kitchen;
    public ShopAudio shop;
    public float fadeDuration = 1.5f;

    private Coroutine tavernFadeCoroutine;
    private Coroutine kitchenFadeCoroutine;

    public TutorialManager tutorialManager;

    public bool inKitchen = false;

    public RawImage tvOn;
    public RawImage tvOff;
    public float tvOnDuration = 1.5f;
    public float tvOffDuration = 1f;

    public GameObject tvPivot;
    public Animator anim;

    void Start()
    {
        SwitchToFPS();
        anim = tvPivot.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && TutorialManager.npcTutorialShown)
        {
            if (shop != null && shop.playerInShop)
                return;


            if (kitchenCam.enabled)
            {
                anim.SetTrigger("TVOut");
            }
            else
            {
                anim.SetTrigger("TVIn");
            }
        }
    }

    public void TriggerSwitchToKitchen()
    {
        StartCoroutine(PlayTransitionAndSwitch(SwitchToKitchen, tvOn, tvOnDuration));
    }

    public void TriggerSwitchToFPS()
    {
        StartCoroutine(PlayTransitionAndSwitch(SwitchToFPS, tvOff, tvOffDuration));
    }

    public void SwitchToKitchen()
    {
        inKitchen = true;

        tutorialManager.KitchenTutorial();

        kitchenCam.enabled = true;
        fpsCam.enabled = false;
        playerCharacter.SetActive(false);
        ui.enabled = true;
        Crosshair.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (tavernFadeCoroutine != null) StopCoroutine(tavernFadeCoroutine);
        if (kitchenFadeCoroutine != null) StopCoroutine(kitchenFadeCoroutine);

        tavernFadeCoroutine = StartCoroutine(FadeAudio(tavern, 0f));
        kitchenFadeCoroutine = StartCoroutine(FadeAudio(kitchen, 0.5f));
    }

    public void SwitchToFPS()
    {
        inKitchen = false;

        if (TutorialManager.tutorialDone)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (TutorialManager.kitchenTutorialShown)
        {
            tutorialManager.ServeNPCTutorial();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        kitchenCam.enabled = false;
        fpsCam.enabled = true;
        playerCharacter.SetActive(true);
        ui.enabled = false;
        Crosshair.SetActive(true);

        if (!FindAnyObjectByType<ShopAudio>().playerInShop)
        {
            if (tavernFadeCoroutine != null) StopCoroutine(tavernFadeCoroutine);
            tavernFadeCoroutine = StartCoroutine(FadeAudio(tavern, inKitchen ? 0f : 0.5f));
        }

        if (kitchenFadeCoroutine != null) StopCoroutine(kitchenFadeCoroutine);

        tavernFadeCoroutine = StartCoroutine(FadeAudio(tavern, 0.5f));
        kitchenFadeCoroutine = StartCoroutine(FadeAudio(kitchen, 0f));
    }

    IEnumerator FadeAudio(AudioSource audioSource, float targetVolume)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    IEnumerator WaitForAnimationAndThenPlay(string triggerName, System.Action switchAction, RawImage transitionImage, float showDuration)
    {
        anim.SetTrigger(triggerName);

        yield return null; 
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsName(triggerName))
        {
            yield return null;
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        }

        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        yield return StartCoroutine(PlayTransitionAndSwitch(switchAction, transitionImage, showDuration));
    }

    IEnumerator PlayTransitionAndSwitch(System.Action switchAction, RawImage transitionImage, float showDuration)
    {

        transitionImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(showDuration);

        switchAction?.Invoke();

        transitionImage.gameObject.SetActive(false);
    }

}
