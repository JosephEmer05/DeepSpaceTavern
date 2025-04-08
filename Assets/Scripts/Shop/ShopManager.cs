using TMPro;
using UnityEngine;
using UnityEngine.LowLevel;

public class ShopManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerTPPoint;
    public GameObject shopFloor1;
    public GameObject shopFloor2;
    public GameObject shopIntroUI;
    public GameObject shoppingTimeUI;
    public GameObject shopTimerUI;
    public TextMeshProUGUI shopTimerText;

    NPC_Spawner nPC_Spawner;
    WaveManager waveManager;


    public bool startShopTime = false;
    public float shopTimer = 30f;

    private void Start()
    {
        nPC_Spawner = UnityEngine.Object.FindAnyObjectByType<NPC_Spawner>();
        waveManager = UnityEngine.Object.FindAnyObjectByType<WaveManager>();
    }

    private void Update()
    {
        if (startShopTime)
        {
            shopTimerText.text = shopTimer.ToString("F0");
            shopTimer -= Time.deltaTime;
        }

        if (shopTimer <= 0f) 
        { 
            ShopHide();
        }
    }

    public void ShopUIIntroOpen()
    {
        shopIntroUI.SetActive(true);
    }

    public void ShopUIIntroClose()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        waveManager.playerLook.enabled = true;
        shopIntroUI.SetActive(false);
        ShopReveal();
    }

    public void ShopReveal()
    {
        startShopTime = true;
        shopFloor1.SetActive(false);
        shopFloor2.SetActive(false);
        shoppingTimeUI.SetActive(true);
        shopTimerUI.SetActive(true);
    }

    public void ShopHide()
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;
        player.transform.position = playerTPPoint.transform.position;
        controller.enabled = true;
        shopFloor1.SetActive(true);
        shopFloor2.SetActive(true);
        shoppingTimeUI.SetActive(false);
        shopTimerUI.SetActive(false);
        startShopTime =false;
        shopTimer = 30f;
        nPC_Spawner.canSpawn = true;
        waveManager.UpdateWaveNumText();
    }
}
