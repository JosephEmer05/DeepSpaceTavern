using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    private ChairManager chairManager;

    private bool isWalkingToChair = false;
    private bool isSeated = false;
    private bool isLeaving = false;
    public GameObject targetChair;

    private Animator anim;

    public float moveSpeed = 0.5f;
    public float rotationSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chairManager = UnityEngine.Object.FindAnyObjectByType<ChairManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
        targetChair = chairManager.FindAvailableChair();
        if (targetChair != null)
        {
            isWalkingToChair = true;
            anim.SetTrigger("StartWalking");
        }
    }

    public void WalkToChair()
    {
        Vector3 direction = (targetChair.transform.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetChair.transform.position, moveSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, targetChair.transform.position);
        if (distance < 0.1f)
        {
            Quaternion chairRotation = targetChair.transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, chairRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, chairRotation) < 1f) 
            {
                SitOnChair();
            }
        }
    }

    public void SitOnChair()
    {
        isWalkingToChair = false;
        isSeated = true;
        anim.SetTrigger("Sit");
        transform.position = targetChair.transform.position;
        transform.SetParent(targetChair.transform);
    }

    public void LeaveChair()
    {
        isLeaving = true;
        if (targetChair != null)
        {
            chairManager.UnreserveChair(targetChair);
            targetChair = null;
            isSeated = false;
            isWalkingToChair = false;
        }
    }
}
