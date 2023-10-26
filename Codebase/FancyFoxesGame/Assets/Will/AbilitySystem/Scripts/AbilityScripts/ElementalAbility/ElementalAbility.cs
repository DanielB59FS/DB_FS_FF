using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the creation of new elemental abilities.
// This can be done under the Assets -> Create menu in unity. 

[CreateAssetMenu(menuName = "Abilities/Elemental Ability")]
public class ElementalAbility : Ability {

    public int abilityDamage;
    public int powerLevel;
    public float abilitySpeed;
    public float hitForce;
    public int maximumShots;
    public AudioSource abilityAudioSource;
    public AudioClip abilityAudioClip;
    public GameObject collisionGameObject;
    public GameObject projectilePrefab;
    private UseElementalAbility useElementalAbility;
    public bool strongAgainstLava;
    public bool strongAgainstNature;
    public bool strongAgainstIce;
    public bool weakAgainstLava;
    public bool weakAgainstNature;
    public bool weakAgainstIce;
    public bool isLavaType;
    public bool isIceType;
    public bool isNatureType;


    public override void Initialize(GameObject gameObject) {
        // does nothing, could possible be removed
    }

    // instantiate the projectile to fire
    public override void TriggerAbility() {
        useElementalAbility.ActiveElementalAbility();
    }
}
