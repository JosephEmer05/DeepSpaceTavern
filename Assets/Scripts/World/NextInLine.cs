using UnityEngine;

public class NextInLine : MonoBehaviour
{
    private LineCountDown lineCountDown;
    public GameObject enterTavernChecker;

    private void Start()
    {
        lineCountDown = enterTavernChecker.GetComponent<LineCountDown>();
    }

    private void OnTriggerEnter(Collider other)
    {
        NPC_Controller nPC_Controller = other.gameObject.GetComponent<NPC_Controller>();
        if (nPC_Controller != null)
        {
            lineCountDown.startTime = true;
            //nPC_Controller.nextToEnter = true;
        }
    }
}
