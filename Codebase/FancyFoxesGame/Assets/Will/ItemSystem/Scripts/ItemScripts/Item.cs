using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// holds common data for all items
public abstract class Item : ScriptableObject {

    public string itemName;
    public string itemText;
    public Sprite ItemIcon;
    public Sprite EmptyItemIcon;
    public AudioClip itemPickupSound;
    public float timeBeforeDespawn;
    public bool doesItemFloat;
    public float floatSpeed = 2f;
    public float floatHeight = 0.2f;
}
