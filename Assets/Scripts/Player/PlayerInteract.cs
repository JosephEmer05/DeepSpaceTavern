using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Transform playerCameraTransform;
    private LayerMask highlightableLayerMask;
    
    private RaycastHit hit;
    public float hitRange = 10;

    public TrayManager trayManager;
    public PlateStacker plateStacker;
    public WaveManager waveManager;
    public FoodValueManager foodValueManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highlightableLayerMask = LayerMask.GetMask("Highlightable");
        playerCameraTransform = Camera.main.transform;
        trayManager = UnityEngine.Object.FindAnyObjectByType<TrayManager>();
        plateStacker = UnityEngine.Object.FindAnyObjectByType<PlateStacker>();
        waveManager = UnityEngine.Object.FindAnyObjectByType<WaveManager>();
        foodValueManager = UnityEngine.Object.FindAnyObjectByType<FoodValueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Highlight();   
    }

    public void Highlight()
    {
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
        }

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, highlightableLayerMask))
        {
            ShopItem shopItem = hit.collider.GetComponent<ShopItem>();
            if (shopItem != null && !shopItem.IsSoldOut)
            {
                hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    shopItem.BuyItem();
                }

                return;
            }

            NPC_Controller npc = hit.collider.GetComponent<NPC_Controller>();
            if (npc != null && npc.isSeated && !npc.orderTaken)
            {
                hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
                TakeOrder(npc);
                return;
            }

            FoodStatus food = hit.collider.GetComponent<FoodStatus>();
            if (food != null)
            {
                hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);

                if (!food.onTray && !food.transparentFood)
                {
                    GrabFood(food);
                }

                if (food.transparentFood)
                {
                    string foodTag = food.gameObject.tag;
                    GameObject tableSlot = food.gameObject.transform.parent?.gameObject;
                    ServeFood(foodTag, tableSlot);
                }

                return;
            }

            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
        }
    }


    public void TakeOrder(NPC_Controller npc)
    {
        if (npc != null && Input.GetKeyDown(KeyCode.E) && !npc.orderTaken)
        {
            npc.OrderFood();
        }
    }

    public void GrabFood(FoodStatus food)
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject traySlot = trayManager.CheckTraySlot();
            if (traySlot != null)
            {
                food.transform.SetParent(traySlot.transform);
                plateStacker.RemoveDish(food.gameObject);
                food.transform.localPosition = Vector3.zero;
                food.transform.localScale = Vector3.one;
                food.transform.localRotation = Quaternion.Euler(0, food.transform.localRotation.eulerAngles.y, 0);
                food.GrabFood();
            }
        }
    }

    public void ServeFood(string foodTag, GameObject tableSlot)
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject foodToTable = trayManager.CheckTrayFood(foodTag);
            
            if (foodToTable == null)
            {
                Transform NPCParent = tableSlot.transform;
                while (NPCParent != null)
                {
                    if (NPCParent.CompareTag("NPC"))
                    {
                        NPC_Controller nPC_Controller = NPCParent.GetComponent<NPC_Controller>();
                        nPC_Controller.StartCoroutine(nPC_Controller.WrongFood());
                        break;
                    }
                    NPCParent = NPCParent.parent;
                }
            }
            else
            {
                waveManager.GoldEarnedUpdater(foodValueManager.GetFoodValue(foodToTable));

                foodToTable.transform.SetParent(tableSlot.transform);
                foodToTable.layer = LayerMask.NameToLayer("Default");
                foodToTable.transform.localPosition = Vector3.zero;
                foodToTable.transform.localScale = Vector3.one;
                foodToTable.transform.localRotation = Quaternion.Euler(0, 0, 0);



                Transform NPCParent = foodToTable.transform;
                while (NPCParent != null)
                {
                    if (NPCParent.CompareTag("NPC"))
                    {
                        NPC_Controller nPC_Controller = NPCParent.GetComponent<NPC_Controller>();
                        nPC_Controller.FoodServed(tableSlot);
                        break;
                    }
                    NPCParent = NPCParent.parent;
                }  
            }   
        }
    }

}
