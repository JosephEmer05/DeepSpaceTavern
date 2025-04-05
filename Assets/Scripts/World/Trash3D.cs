using UnityEngine;

public class Trash3D : MonoBehaviour
{
    public GameObject tray0;

    public void Clear()
    {
        
            Transform child = tray0.transform.GetChild(0);
            Destroy(child.gameObject);

    }
    
}
