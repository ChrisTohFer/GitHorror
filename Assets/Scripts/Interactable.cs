using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public GameObject Hightlight;

    public void Interact()
    {
        OnInteract.Invoke();
    }
    public void HighlightActive(bool value)
    {
        if (Hightlight != null)
            Hightlight.SetActive(value);
    }
}
