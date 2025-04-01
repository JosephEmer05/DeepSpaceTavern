using System;
using System.Collections.Generic;
using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public List<GameObject> chairList = new();
    private Dictionary<GameObject, bool> chairReserved = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindAllChairs();
    }

    public void FindAllChairs()
    {
        chairList.Clear();
        chairReserved.Clear();
        
        GameObject[] foundChairs = GameObject.FindGameObjectsWithTag("Chair");
        foreach (GameObject chair in foundChairs)
        {
            chairList.Add(chair);
            chairReserved[chair] = false;
        }
    }

    public GameObject FindAvailableChair()
    {
        foreach (GameObject chair in chairList)
        {
            if (!ChairIsOccupied(chair) && !chairReserved[chair])
            {
                chairReserved[chair] = true; 
                return chair;
            }
        }
        return null;
    }

    public bool ChairIsOccupied(GameObject chair)
    {
        return chair.transform.childCount > 1;
    }

    public void UnreserveChair(GameObject chair)
    {
        if (chairReserved.ContainsKey(chair))
        {
            chairReserved[chair] = false; 
        }
    }
}
