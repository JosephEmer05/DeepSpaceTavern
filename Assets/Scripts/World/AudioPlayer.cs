using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip oneShotClip;
    public AudioSource oneShotSource;

    public void PlayOneShotClip()
    {
        oneShotSource.PlayOneShot(oneShotClip);
    }
    public void StopAudio()
    {
        oneShotSource.Stop();
    }
}
