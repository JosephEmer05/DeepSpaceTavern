using UnityEngine;

public class LineCountDown : MonoBehaviour
{
    public float lineWaitingTime = 120f;

    // Update is called once per frame
    void Update()
    {
        lineWaitingTime -= Time.deltaTime;
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
