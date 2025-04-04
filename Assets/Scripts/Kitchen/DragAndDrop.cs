using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private float zCoordinate;

    private void OnMouseDown()
    {
        // Get the Z position of the object in world space
        zCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;

        // Convert mouse position to world position and calculate offset
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        // Update object position based on mouse position
        transform.position = GetMouseWorldPos() + offset;
    }

    private Vector3 GetMouseWorldPos()
    {
        // Get current mouse position in screen space
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = zCoordinate;  // Maintain the original Z position

        // Convert screen position to world position
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}
