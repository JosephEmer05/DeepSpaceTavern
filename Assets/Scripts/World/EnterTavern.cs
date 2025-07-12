using UnityEngine;

public class EnterTavern : MonoBehaviour
{
    public bool enteredTavern = false;
    public Animator anim;

    TutorialManager tutorialManager;
    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("OpenDoor");
        NPC_Controller nPC_Controller = other.gameObject.GetComponent<NPC_Controller>();
        if (nPC_Controller != null)
        {
            if (!nPC_Controller.isLeaving)
            {
                enteredTavern = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        anim.SetTrigger("CloseDoor");
    }
}
