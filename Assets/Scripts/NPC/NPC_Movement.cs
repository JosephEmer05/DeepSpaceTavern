using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour
{
    //Components
    private NPC npc;
    private Animator anim;
    private NavMeshAgent agent;

    //Booleans for checking NPC's state
    public bool isWalkingToChair = false;
    public bool isSeated = false;
    public bool isLeaving = false;

    //Objects
    public GameObject targetChair;


    //NPC stats
    public float rotationSpeed = 5f;
    public float moveSpeed = 0.5f;


    private void Start()
    {
        npc = GetComponent<NPC>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isSeated && !isWalkingToChair && !isLeaving)
        {
            FindSeat();
        }

        if (isWalkingToChair && targetChair != null)
        {
            WalkToChair();
        }
    }
    public void FindSeat()
    {
        targetChair = npc.Chair.FindAvailableChair();
        if (targetChair != null)
        {
            isWalkingToChair = true;
            anim.SetTrigger("StartWalking");
        }
    }

    public void WalkToChair()
    {
        //Vector3 direction = (targetChair.transform.position - transform.position).normalized;
        //Debug.Log(targetChair.transform.position);
        //Debug.Log(transform.position);

        //if (direction != Vector3.zero)
        //{
        //    Quaternion lookRotation = Quaternion.LookRotation(direction);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        //}

        //transform.position = Vector3.MoveTowards(transform.position, targetChair.transform.position, moveSpeed * Time.deltaTime);
        agent.SetDestination(targetChair.transform.position);
        //float distance = Vector3.Distance(transform.position, targetChair.transform.position);
        //if (distance < 1.5f)
        //{
        //    Quaternion chairRotation = targetChair.transform.rotation;
        //    transform.rotation = Quaternion.Slerp(transform.rotation, chairRotation, rotationSpeed * Time.deltaTime);

        //    agent.enabled = false;

        //    if (Quaternion.Angle(transform.rotation, chairRotation) < 1f)
        //    {
        //        //SitOnChair();
        //    }
        //}
    }
}
