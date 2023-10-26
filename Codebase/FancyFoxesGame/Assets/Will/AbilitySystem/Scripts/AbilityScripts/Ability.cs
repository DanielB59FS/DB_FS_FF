using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script defines common attributes for all abilities
public abstract class Ability : ScriptableObject {

    public string abilityName;
    public Sprite abilityIcon;
    public Sprite weakAgainstIcon;
    public Sprite strongAgainstIcon;
    public AudioClip abilitySound;
    public float abilityBaseCooldown;
    public float timeBeforeDespawn;

    public abstract void Initialize(GameObject gameObject);
    public abstract void TriggerAbility();

}
