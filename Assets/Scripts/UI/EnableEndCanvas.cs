using TMPro;
using UnityEngine;

public class EnableEndCanvas : MonoBehaviour
{
    public GameObject endCanvas;
    public TextMeshProUGUI score;
    public TextMeshProUGUI waveNum;

    void Start()
    {
        endCanvas.SetActive(false);
        ShowScoreAndWave();
    }
    public void EnableEnd()
    {
        endCanvas.SetActive(true); 
    }

    public void ShowScoreAndWave()
    {
        score.text = WaveManager.score.ToString();
        int finalwavenum = WaveManager.waveNumber - 1;
        waveNum.text = finalwavenum.ToString() ;

    }
}
