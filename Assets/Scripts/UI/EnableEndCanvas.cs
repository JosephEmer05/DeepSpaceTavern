using UnityEngine;

public class EnableEndCanvas : MonoBehaviour
{
    public GameObject endCanvas;

    void Start()
    {
        endCanvas.SetActive(false);
    }
    public void EnableEnd()
    {
        endCanvas.SetActive(true); 
    }
}
