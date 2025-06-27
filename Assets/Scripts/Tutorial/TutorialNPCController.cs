using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class TutorialNPCController : MonoBehaviour
{
    private ChairManager chairManager;
    private FoodStatus foodStatus;

    //public bool nextToEnter = false;
    public bool isWalkingToChair = false;
    public bool isSeated = false;
    public bool orderTaken = false;
    public bool foodServed = false;
    public bool isLeaving = false;
    public bool loseLife = false;
    public bool lostLife = false;

    private GameObject targetChair;
    public GameObject food1Slot;
    public GameObject food2Slot;
    public GameObject food2ASlot;
    public GameObject food2BSlot;
    public GameObject handSlot;
    private GameObject npcFood;
    private int foodServedCount = 0;
    public GameObject exitPoint;
    private NavMeshAgent agent;

    private Animator anim;

    public float moveSpeed = 0.1f;
    public float rotationSpeed = 5f;
    public float eatingTime = 5f;

 


    public NPCAudio player;
    public bool impatient = false;
    public bool poof = false;

    public VisualEffect poofEffect;


    FoodShake foodShake1;
    FoodShake foodShake2A;
    FoodShake foodShake2B;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chairManager = UnityEngine.Object.FindAnyObjectByType<ChairManager>();
        anim = GetComponent<Animator>();
        exitPoint = GameObject.FindWithTag("Exit");
        agent = GetComponent<NavMeshAgent>();
        poofEffect = GetComponent<VisualEffect>();

        foodShake1 = food1Slot.GetComponent<FoodShake>();
        foodShake2A = food2ASlot.GetComponent<FoodShake>();
        foodShake2B = food2BSlot.GetComponent<FoodShake>();
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

        if (isLeaving)
        {
            LeaveTavern();
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

        agent.SetDestination(targetChair.transform.position);
        float distance = Vector3.Distance(transform.position, targetChair.transform.position);
        if (distance < 0.1f)
        {
            Quaternion chairRotation = targetChair.transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, chairRotation, rotationSpeed * Time.deltaTime);

            agent.enabled = false;

            if (Quaternion.Angle(transform.rotation, chairRotation) < 1f)
            {
                SitOnChair();
            }
        }
    }

    public void SitOnChair()
    {
        player.Order();
        isWalkingToChair = false;
        isSeated = true;
        anim.SetTrigger("Sit");
        transform.position = targetChair.transform.position;
        transform.SetParent(targetChair.transform);
    }

    public void OrderFood()
    {
        SpawnAndSetupFood(food1Slot.transform);
        
        anim.SetTrigger("OrderTaken");
        anim.SetBool("Tantrum", false);
        orderTaken = true;
    }

    public IEnumerator WrongFood()
    {
        player.WrongDish();
        anim.SetBool("Tantrum", true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("Tantrum", false);
    }

    private void SpawnAndSetupFood(Transform parentSlot)
    {
        npcFood.GetComponent<FoodStatus>().transparentFood = true;
        npcFood.transform.SetParent(parentSlot);
        npcFood.transform.localPosition = Vector3.zero;
        npcFood.transform.localEulerAngles = Vector3.zero;
        npcFood.transform.localScale = Vector3.one;
    }


    public void FoodServed(GameObject tableSlot)
    {
        if (FoodSlotUsed() == food1Slot)
        {
            foodServed = true;
            Destroy(npcFood);
        }
        else
        {
            Transform food = tableSlot.transform.GetChild(0);
            if (food != null)
            {
                foodServedCount++;
                Destroy(food.gameObject);
            }
            if (foodServedCount >= 2)
            {
                foodServed = true;
            }
        }

    }

    private IEnumerator EatFood()
    {
        GameObject foodObject = FoodSlotUsed();
        if (foodObject == null)
        {
            Debug.Log("No food to eat!");
            yield break;
        }

        Transform foodParent = foodObject.transform;

        while (foodParent.childCount > 0)
        {
            Transform foodHolder = foodParent.GetChild(0);
            Transform actualFood = (foodHolder.childCount > 0) ? foodHolder.GetChild(0) : foodHolder;
            Vector3 originalScale = foodHolder.localScale;
            float elapsedTime = 0f;
            bool isDrink = actualFood.CompareTag("Beer");

            if (isDrink)
            {
                anim.ResetTrigger("Eat");
                anim.SetTrigger("Drink");
            }
            else
            {
                anim.ResetTrigger("Drink");
                anim.SetTrigger("Eat");
            }

            while (elapsedTime < eatingTime)
            {
                if (!isDrink)
                {
                    float scaleFactor = 1 - (elapsedTime / eatingTime);
                    foodHolder.localScale = originalScale * scaleFactor;
                }
                else
                {
                    foodHolder.transform.position = handSlot.transform.position;
                    foodHolder.transform.rotation = handSlot.transform.rotation;
                    foodHolder.transform.localScale = handSlot.transform.lossyScale;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            foodHolder.localScale = Vector3.zero;
            Destroy(foodHolder.gameObject);
            yield return new WaitForSeconds(0.5f);
        }
        GetOffChair();
        Debug.Log("All food has been eaten!");
    }


    public GameObject FoodSlotUsed()
    {
        if (food1Slot.transform.childCount > 0)
        {
            return food1Slot;
        }
        else if (food2ASlot.transform.childCount > 0 && food2BSlot.transform.childCount > 0)
        {
            return food2Slot;
        }
        else
        {
            Debug.Log("No food in food slots");
            return null;
        }
    }

    public void DrinkBeer(Transform foodHolder)
    {
        anim.SetTrigger("Drink");
        foodHolder.transform.SetParent(handSlot.transform);
        foodHolder.transform.localPosition = Vector3.zero;
        foodHolder.transform.localScale = Vector3.one;
        foodHolder.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }


    public void GetOffChair()
    {
        isLeaving = true;
        anim.SetTrigger("Leave");
        if (targetChair != null)
        {
            transform.SetParent(null);
            chairManager.UnreserveChair(targetChair);
            targetChair = null;
            isSeated = false;
            isWalkingToChair = false;
        }
    }

    public void LeaveTavern()
    {
        agent.enabled = true;
        agent.SetDestination(exitPoint.transform.position);
        if (!poof)
        {
            StartCoroutine(Poof());
        }
    }

    public IEnumerator Poof()
    {
        poof = true;
        yield return new WaitForSeconds(5f);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        player.Poof();
        poofEffect.enabled = true;

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);

    }

    public IEnumerator PlayImpatient()
    {
        impatient = true;
        player.Impatient();
        yield return new WaitForSeconds(0.5f);

    }
}
