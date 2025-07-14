using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = mainCamera.transform.position;
        transform.LookAt(2 * transform.position - targetPosition);
    }
}
