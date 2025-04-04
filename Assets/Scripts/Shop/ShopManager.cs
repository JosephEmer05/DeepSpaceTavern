using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerTPPoint;
    public GameObject shopFloor1;
    public GameObject shopFloor2;
    NPC_Spawner nPC_Spawner;

    public bool startShopTime = false;
    public float shopTimer = 30f;

    private void Start()
    {
        nPC_Spawner = UnityEngine.Object.FindAnyObjectByType<NPC_Spawner>();

    }

    private void Update()
    {
        if (startShopTime)
        {
            shopTimer -= Time.deltaTime;
        }

        if (shopTimer <= 0f) 
        { 
            ShopHide();
        }
    }

    public void ShopReveal()
    {
        startShopTime = true;
        shopFloor1.SetActive(false);
        shopFloor2.SetActive(false);
    }

    public void ShopHide()
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;
        player.transform.position = playerTPPoint.transform.position;
        controller.enabled = true;
        shopFloor1.SetActive(true);
        shopFloor2.SetActive(true);
        startShopTime=false;
        shopTimer = 30f;
        nPC_Spawner.canSpawn = true;
    }
}
