using Unity.VisualScripting;
using UnityEngine;

public class LineCountDown : MonoBehaviour
{
    public float lineWaitingTime = 120f;
    public bool startTime = false;
    // Update is called once per frame
    void Update()
    {
        if (startTime)
        {
            lineWaitingTime -= Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        NPC_Controller nPC_Controller = other.gameObject.GetComponent<NPC_Controller>();
        if (nPC_Controller != null)
        {
            if (!nPC_Controller.isLeaving)
            {
                lineWaitingTime += 15f;
            }
        }
    }
}
