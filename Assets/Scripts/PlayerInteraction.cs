using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //
    public float InteractionRange;
    public KeyCode InteractionKey;

    //
    Interactable m_interactTarget;

    private void Update()
    {
        UpdateInteractableTarget();
        if (Input.GetKeyDown(InteractionKey))
            Interact();
    }

    public void Interact()
    {
        if (m_interactTarget != null)
            m_interactTarget.Interact();
    }

    //Raycast from the player to see if they are looking at something interactable
    void UpdateInteractableTarget()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)), out hit, InteractionRange, ~LayerMask.GetMask("Player")))
        {
            //Note that this might still be null if the target is non-interactable
            m_interactTarget = hit.transform.GetComponent<Interactable>();
            return;
        }
        //We didn't hit anything in range
        m_interactTarget = null;
    }
}
