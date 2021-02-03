using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [System.Serializable]
    public class Trigger
    {
        public string key;
        public bool triggered;
    }

    public Trigger[] TriggerList;
    public GameObject[] ObjectsToEnable;

    private bool m_activated;

    void Activate()
    {
        if (m_activated)
            return;
        else
            m_activated = true;

        foreach(GameObject obj in ObjectsToEnable)
        {
            obj.SetActive(true);
        }
    }

    bool CheckAllTriggered()
    {
        foreach(Trigger t in TriggerList)
        {
            if(!t.triggered)
            {
                return false;
            }
        }
        return true;
    }

    public void ActivateTrigger(string key)
    {
        foreach (Trigger t in TriggerList)
        {
            if (t.key == key)
            {
                t.triggered = true;
                break;
            }
        }
        if (CheckAllTriggered())
        {
            Activate();
        }
    }
}
