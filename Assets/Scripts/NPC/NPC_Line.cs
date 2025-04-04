using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_LineManager : MonoBehaviour
{
    public Transform targetLinePoint;
    private bool isMoving;

    public void CheckCurrentLocation()
    {
        if (isMoving) return;

        LineOrganizer[] lineOrganizers = UnityEngine.Object.FindObjectsByType<LineOrganizer>(UnityEngine.FindObjectsSortMode.None);

        foreach (var organizer in lineOrganizers)
        {
            // Check if this NPC is in any of the linedNPC arrays
            for (int i = 0; i < organizer.linedNPC.Length; i++)
            {
                if (organizer.linedNPC[i] == this.gameObject)
                {
                    // Found the organizer this NPC belongs to!
                    targetLinePoint = organizer.transform;

                    Debug.Log($"{gameObject.name} is in line at: {targetLinePoint.name}");
                    return;
                }
            }
        }

        // If not found in any line
        targetLinePoint = null;
        Debug.Log($"{gameObject.name} is not currently in line.");
    }
}
