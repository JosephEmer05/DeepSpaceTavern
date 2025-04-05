using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI finalWave;

    void Start()
    {
        finalScore.text = GameData.score.ToString();
        finalWave.text = GameData.finalWave.ToString();
    }
}