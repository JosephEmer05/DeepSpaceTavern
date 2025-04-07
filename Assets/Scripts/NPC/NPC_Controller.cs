using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Controller : MonoBehaviour
{
    private ChairManager chairManager;
    private FoodRandomizer foodRandomizer;
    private FoodStatus foodStatus;
    private WaveManager waveManager;
    private PlayerHealth playerHealth;

    //public bool nextToEnter = false;
    public bool isWalkingToChair = false;
    public bool isSeated = false;
    public bool orderTaken = false;
    public bool foodServed = false;
    public bool isLeaving = false;

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

    public Material dissolveMat;


    private Animator anim;

    public float moveSpeed = 0.5f;
    public float rotationSpeed = 5f;
    public float eatingTime = 5f;
    public float waitingTime;
    public float angryTimeLeft = 15f;

    public ShopItem shopManager;
    public bool NPCPatienceAdded = false;

    public int bandCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopManager = GameObject.FindWithTag("NPCPatience").GetComponent<ShopItem>();
        chairManager = UnityEngine.Object.FindAnyObjectByType<ChairManager>();
        foodRandomizer = UnityEngine.Object.FindAnyObjectByType<FoodRandomizer>();
        waveManager = UnityEngine.Object.FindAnyObjectByType<WaveManager>();
        playerHealth = UnityEngine.Object.FindAnyObjectByType<PlayerHealth>();
        anim = GetComponent<Animator>();
        waitingTime = waveManager.NPCSeatedWaitTime;
        exitPoint = GameObject.FindWithTag("Exit");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NPCPatienceAdded && shopManager.currentStock == 1)
        {
            NPCPatienceBoost();
        }

        if (isSeated)
        {
            waitingTime -= Time.deltaTime;
            if (waitingTime <= angryTimeLeft && !foodServed)
            {
                FoodShake foodShake1 = food1Slot.GetComponent<FoodShake>();
                FoodShake foodShake2A = food2ASlot.GetComponent<FoodShake>();
                FoodShake foodShake2B = food2BSlot.GetComponent<FoodShake>();
                foodShake1.FoodShaking(food1Slot);
                foodShake2A.FoodShaking(food2ASlot);
                foodShake2B.FoodShaking(food2BSlot);

                anim.SetBool("Tantrum", true);
                if (waitingTime <= 0)
                {
                    playerHealth.LoseLife();
                    anim.SetBool("Tantrum", false);
                    GetOffChair();
                    food1Slot.SetActive(false);
                    food2Slot.SetActive(false);
                    LeaveTavern();
                }
            }
            
        }

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
        Vector3 direction = (targetChair.transform.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetChair.transform.position, moveSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, targetChair.transform.position);
        if (distance < 1.5f)
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
        isWalkingToChair = false;
        isSeated = true;
        anim.SetTrigger("Sit");
        transform.position = targetChair.transform.position;
        transform.SetParent(targetChair.transform);
    }

    public void OrderFood()
    {
        if (WaveManager.waveNumber <= 2)
        {
            SpawnAndSetupFood(food1Slot.transform);
        }
        else
        {
            if (UnityEngine.Random.Range(0f, 1f) <= waveManager.comboMealChance / 100)
            {
                SpawnAndSetupFood(food2ASlot.transform);
                SpawnAndSetupFood(food2BSlot.transform);
            }
            else
            {
                SpawnAndSetupFood(food1Slot.transform);
            }
        }

        anim.SetTrigger("OrderTaken");
        anim.SetBool("Tantrum", false);
        orderTaken = true;
    }

    public IEnumerator WrongFood()
    {
        anim.SetBool("Tantrum", true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("Tantrum", false);
    }

    private void SpawnAndSetupFood(Transform parentSlot)
    {
        npcFood = Instantiate(foodRandomizer.PickFood());
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
            if(foodServedCount >=2)
            {
                foodServed= true;
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
        //waveManager.UpdateCustomerLeft();
        //Debug.Log("Stood Up");
    }

    public void LeaveTavern()
    {
        agent.enabled = true;

        Vector3 direction = (exitPoint.transform.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, exitPoint.transform.position, moveSpeed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, exitPoint.transform.position);

        StartCoroutine(Poof());
    }

    public IEnumerator Poof()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            // Replace all materials with the dissolveMat
            Material[] newMaterials = new Material[rend.materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = dissolveMat;
            }

            rend.materials = newMaterials;
        }
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public void NPCPatienceBoost()
    {
        waitingTime += 10f;
        NPCPatienceAdded = true;
    }

    public void NPCBandBoost(int num)
    {
        for(int i = 0; i<num; i++)
        {
            if (bandCount <= i)
            {
                waitingTime += 7.5f;
                bandCount++;
            }
        }        
    }

}
