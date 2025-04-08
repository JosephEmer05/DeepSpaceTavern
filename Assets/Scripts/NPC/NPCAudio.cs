using UnityEngine;

public class NPCAudio : MonoBehaviour
{
    public AudioSource npcPlayer; 
    public AudioClip eating;      
    public AudioClip impatient;   
    public AudioClip poof;        
    public AudioClip wrongDish;   
    public AudioClip order;      

    public void WrongDish()
    {
        PlayAudioClip(wrongDish);
    }

    public void Eating()
    {
        PlayAudioClip(eating);
    }

    public void Impatient()
    {
        PlayAudioClip(impatient);
    }

    public void Poof()
    {
        PlayAudioClip(poof);
    }

    public void Order()
    {
        PlayAudioClip(order);
    }

    private void PlayAudioClip(AudioClip clip)
    {
        npcPlayer.PlayOneShot(clip);
    }
}
