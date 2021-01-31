using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyItem", menuName = "ScriptableObjects/KeyItem", order = 1)]
public class KeyItem : ScriptableObject
{
    public string       Name;
    public Texture2D    Image;
}
