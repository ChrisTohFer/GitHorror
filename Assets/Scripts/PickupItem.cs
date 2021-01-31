using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //
    public KeyItem Item;
    public string Name;
    public float Quantity;

    public void Pickup()
    {
        if (Item != null)
        {
            PlayerStats.AddKeyItem(Item);
            gameObject.SetActive(false);
            return;
        }
        else if (Name != "" && !PlayerStats.StatMaxed(Name))
        {
            PlayerStats.ChangeStat(Name, Quantity);
            gameObject.SetActive(false);
            return;
        }
    }
}
