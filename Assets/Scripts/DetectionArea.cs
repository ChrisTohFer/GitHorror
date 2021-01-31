using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    public DetectionArea[] SubAreas;

    bool containsPlayer = false;
    public bool ContainsPlayer
    {
        get {
            if (containsPlayer)
                return true;
            foreach(DetectionArea area in SubAreas)
            {
                if (area.ContainsPlayer)
                    return true;
            }
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6) //Magic numbers SUCK
            containsPlayer = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) //Magic numbers SUCK
            containsPlayer = false;
    }
}
