using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Creature Ability")]
public class CreatureAbility : Ability {

    public float abilityDamage = 1;
    public float abilityRange = 50f;
    public float hitForce = 100f;
    public Color abilityColor = Color.white;
    public GameObject projectilePrefab;
    public GameObject AdditionalProjectilePrefab;
    public float speed;
    public float cooldownDuration;
    public float useDuration;
    public float rotationAngleX;
    public float rotationAngleY;
    public float rotationAngleZ;
    public bool rotationOverride;
    public float spawnFromAdjustmentX;
    public float spawnFromAdjustmentY;
    public float spawnFromAdjustmentZ;
    public bool spawnFromProjectileSpawnPoint;
    public bool isProjectile;
    public bool offensiveType;
    public bool defensiveType;

    private UseCreatureAbility useCreatureAbility;

    public override void Initialize(GameObject gameObject) {
        useCreatureAbility = gameObject.GetComponent<UseCreatureAbility>();
    }

    public override void TriggerAbility() {
        useCreatureAbility.ActiveCreatureAbility();
    }
}
