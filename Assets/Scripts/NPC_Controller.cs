using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    private ChairManager chairManager;
    private FoodRandomizer foodRandomizer;
    private FoodStatus foodStatus;

    public bool isWalkingToChair = false;
    public bool isSeated = false;
    public bool orderTaken = false;
    public bool foodServed = false;
    public bool isLeaving = false;
    
    public GameObject targetChair;
    public GameObject foodSlot;
    public GameObject npcFood;

    private Animator anim;

    public float moveSpeed = 0.5f;
    public float rotationSpeed = 5f;
    public float eatingTime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chairManager = UnityEngine.Object.FindAnyObjectByType<ChairManager>();
        foodRandomizer = UnityEngine.Object.FindAnyObjectByType<FoodRandomizer>();
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

        if (foodServed)
        {
            StartCoroutine(EatFood());
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

    public void OrderFood()
    {
        orderTaken = true;
        anim.SetTrigger("OrderTaken");
        Debug.Log("Order was taken");
        npcFood = Instantiate(foodRandomizer.PickFood());
        npcFood.GetComponent<FoodStatus>().transparentFood = true;
        npcFood.transform.SetParent(foodSlot.transform);
        npcFood.transform.localPosition = Vector3.zero;
        npcFood.transform.localEulerAngles = Vector3.zero;
        npcFood.transform.localScale = Vector3.one;

    }

    public void FoodServed()
    {
        foodServed = true;
        Destroy(npcFood);
        
    }

    private IEnumerator EatFood()
    {
        anim.SetTrigger("Eat");
        Transform foodParent = transform.Find("Food1");
        if (foodParent == null || foodParent.childCount == 0)
        {
            Debug.Log("No food to eat!");
            yield break;
        }

        while (foodParent.childCount > 0)
        {
            Transform eatFood = foodParent.GetChild(0);
            Vector3 originalScale = eatFood.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < eatingTime)
            {
                float scaleFactor = 1 - (elapsedTime / eatingTime);
                eatFood.localScale = originalScale * scaleFactor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            eatFood.localScale = Vector3.zero;
            Destroy(eatFood.gameObject);
            
            yield return new WaitForSeconds(1f);
        }

        LeaveChair();
        Debug.Log("All food has been eaten!");
    }

    public void LeaveChair()
    {
        isLeaving = true;
        anim.SetTrigger("Leave");
        if (targetChair != null)
        {
            chairManager.UnreserveChair(targetChair);
            targetChair = null;
            isSeated = false;
            isWalkingToChair = false;
        }
    }
}
