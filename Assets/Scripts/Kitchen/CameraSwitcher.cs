using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    public Camera kitchenCam;
    public Camera fpsCam;
    public GameObject playerCharacter;
    public Canvas ui;
    public GameObject Crosshair;
    public AudioSource tavern;
    public AudioSource kitchen;
    public float fadeDuration = 1.5f;

    private Coroutine tavernFadeCoroutine;
    private Coroutine kitchenFadeCoroutine;

    void Start()
    {
        SwitchToFPS();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (kitchenCam.enabled)
                SwitchToFPS();
            else
                SwitchToKitchen();
        }
    }

    void SwitchToKitchen()
    {
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

    void SwitchToFPS()
    {
        kitchenCam.enabled = false;
        fpsCam.enabled = true;
        playerCharacter.SetActive(true);
        ui.enabled = false;
        Crosshair.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (tavernFadeCoroutine != null) StopCoroutine(tavernFadeCoroutine);
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
}
