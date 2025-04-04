using UnityEngine;

public class NPC_TurnCheck : MonoBehaviour
{

    public float rotationAngle = 45f; // Amount to rotate
    public Vector3 rotationAxis = Vector3.up; // Axis of rotation (default is Y-axis)

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has a collider (optional filter)
        if (other.CompareTag("Obstacle")) // Ensure colliding objects have this tag
        {
            transform.Rotate(rotationAxis * rotationAngle);
        }
    }
}

