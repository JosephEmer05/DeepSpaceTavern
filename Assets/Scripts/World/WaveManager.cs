using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int waveNumber = 1;
    public int customerNumber = 10;
    public int customerLeft;
    public float lineWaitTime = 120f;
    public float NPCSeatedWaitTime = 120f;
    public float comboMealChance = 3;

    private int score = 0;
    private int goldOwned = 0;
    private int goldEarned = 0;

    public PlayerLook playerLook;

    public GameObject endOfWaveUI;
    public TextMeshProUGUI waveNumText;
    public TextMeshProUGUI goldEarnedText;
    public TextMeshProUGUI goldOwnedText;

    public GameObject[] navMeshArray;
    public GameObject[] brokenArray;
    public GameObject[] fixedArray;

    LineCountDown lineCountDown;
    ShopManager shopManager;
    NPC_Spawner nPC_Spawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customerLeft = customerNumber;
        endOfWaveUI.gameObject.SetActive(false);
        lineCountDown = UnityEngine.Object.FindAnyObjectByType<LineCountDown>();
        shopManager = UnityEngine.Object.FindAnyObjectByType<ShopManager>();
        nPC_Spawner = UnityEngine.Object.FindAnyObjectByType<NPC_Spawner>();


    }

    // Update is called once per frame
    void Update()
    {
        if (lineCountDown.startTime)
        {
            UpdateCustomerLeft();
        }
        
        if (customerLeft == 0)
        {
            lineCountDown.startTime = false;
            WaveDone();
        }
    }

    public void UpdateCustomerLeft()
    {
        customerLeft = GameObject.FindGameObjectsWithTag("NPC").Length;
    }

    public void WaveDone()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerLook.enabled = false;
        endOfWaveUI.gameObject.SetActive(true);
        waveNumText.text = "WAVE" + waveNumber;
        goldEarnedText.text = goldEarned.ToString();
        goldOwnedText.text = goldOwned.ToString();
    }

    public void CloseWaveUI()
    {
        WaveReset();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerLook.enabled = true;
        endOfWaveUI.gameObject.SetActive(false);
 
    }

    public void ScoreUpdater()
    {
        score += goldEarned; 
    }

    public void GoldEarnedUpdater(int foodValue)
    {
        goldEarned += foodValue;
    }

    public void WaveReset()
    {
        waveNumber++;
        goldEarned = 0;
        customerNumber += 4;
        customerLeft = customerNumber;
        if (waveNumber <= 2)
        {
            comboMealChance = 0;
            nPC_Spawner.canSpawn = true;
        }
        else
        {
            comboMealChance += 3;
            shopManager.ShopReveal();
        }
        if (waveNumber <= 4)
        {
            LevelChange();
        }
    }

    public void LevelChange()
    {
        navMeshArray[waveNumber - 1].SetActive(true);
        navMeshArray[waveNumber - 2].SetActive(false);


        if ((waveNumber-2) >= 0)
        {
            brokenArray[waveNumber - 2].SetActive(false);
            fixedArray[waveNumber - 2].SetActive(true);
        }       
    }
}
