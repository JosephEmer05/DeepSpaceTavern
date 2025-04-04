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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highlightableLayerMask = LayerMask.GetMask("Highlightable");
        playerCameraTransform = Camera.main.transform;
        trayManager = UnityEngine.Object.FindAnyObjectByType<TrayManager>();
        plateStacker = UnityEngine.Object.FindAnyObjectByType<PlateStacker>();

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
            NPC_Controller npc = hit.collider.GetComponent<NPC_Controller>();
            if (npc != null && npc.isSeated && !npc.orderTaken)
            {
                hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
                TakeOrder(npc);
            }

            FoodStatus food = hit.collider.GetComponent<FoodStatus>();
            if (food != null)
            {
                //for normal food
                hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
                
                if (!food.onTray && !food.transparentFood)
                {
                    GrabFood(food);
                }

                //for transparent food
                if (food.transparentFood)
                {
                    string foodTag = food.gameObject.tag;
                    GameObject tableSlot = food.gameObject.transform.parent?.gameObject;
                    ServeFood(foodTag, tableSlot);
                }
                
            }
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
