using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAbilityAttack : MonoBehaviour {

    public CreatureAbility creatureAbility;
    public GameObject emptyEffect;
    float damage;

    private void OnCollisionEnter(Collision collision) {

        // set the default damage amount
        damage = creatureAbility.abilityDamage;


        // logic for hitting a lava monsters
        if (collision.gameObject.tag == "LavaMonster") {
            // lower the lava monsters health
            collision.gameObject.transform.GetComponent<ExtendedStateController>().TakeDamage(creatureAbility.abilityDamage);
        }

        // logic for hitting a ice monsters
        if (collision.gameObject.tag == "IceMonster") {
            // lower the ice monsters health
            collision.gameObject.transform.GetComponent<ExtendedStateController>().TakeDamage(creatureAbility.abilityDamage);
        }

        // logic for hitting a nature monster
        if (collision.gameObject.tag == "NatureMonster") {
            // lower the nature monsters health
            collision.gameObject.transform.GetComponent<ExtendedStateController>().TakeDamage(creatureAbility.abilityDamage);
        }

        // logic for hitting a nature monster
        if (collision.gameObject.tag == "DummyMonster") {
            // lower the nature monsters health
            collision.gameObject.transform.GetComponent<ExtendedStateController>().TakeDamage(creatureAbility.abilityDamage);
        }

        // logic for hitting a lava monsters
        if (collision.gameObject.tag == "Player") {
            // We dont want to hurt our player!!
            //collision.gameObject.transform.GetComponent<Player>().TakeDamage(damage, emptyEffect);
        }

    }
}
