using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera mainCamera;

    // Update is called once per frame
    void Update()
    {
        mainCamera = Camera.main;

        Vector3 targetPosition = mainCamera.transform.position;
        transform.LookAt(2 * transform.position - targetPosition);
    }
}
