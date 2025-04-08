using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private float fixedZ;  // Keep original z position
    private bool isFollowingMouse = true;

    private void Start()
    {
        fixedZ = transform.position.z;
    }

    private void Update()
    {
        if (isFollowingMouse)
        {
            FollowMouse();

            Input.GetMouseButtonDown(0);

        }
    }

    private void FollowMouse()
    {
        transform.position = GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = fixedZ;
        return worldPos;
    }

    private void OnMouseDown()
    {
        // Toggle follow mode when clicked
        isFollowingMouse = !isFollowingMouse;
    }
}