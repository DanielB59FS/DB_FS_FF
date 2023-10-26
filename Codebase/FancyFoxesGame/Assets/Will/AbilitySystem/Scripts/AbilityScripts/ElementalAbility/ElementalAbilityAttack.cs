using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalAbilityAttack : MonoBehaviour {

    [SerializeField] public ElementalAbility ability;
    float damage;
    private DisplayerDamageIndicator damageDisplayIndicator;
    [HideInInspector] public GameObject source = null;

    private void OnCollisionEnter(Collision collision) {
        if (null != source && default != source)
            if (source.CompareTag("Player") && collision.gameObject.CompareTag("Player")) return;

        /* This top level code is important because it allows the 
           projectiles to interact with any other rigidbody. Attemping
           to generify this code causes all sorts of side effects and
           incorrect interactions with other game objects. */

        // instantiate the hit effect
        GameObject explosion = Instantiate(ability.collisionGameObject, gameObject.transform.position, gameObject.transform.rotation);
        // delete the prefab
        Destroy(gameObject);
        // wait 1 second and destroy the explosion
        Destroy(explosion, 1);

        // get the damage display indicator scripts
        damageDisplayIndicator = collision.gameObject.GetComponentInChildren<DisplayerDamageIndicator>();

        // set the default damage amount
        damage = ability.abilityDamage;

        // logic for hitting a lava monsters
        if (collision.gameObject.tag == "LavaMonster") {

            // calculate elemental strength
            if (ability.strongAgainstLava) {
                damageDisplayIndicator.DisplayDamageIndicator(ability, collision.gameObject.GetComponent<UseElementalAbility>().ability);
                damage = ability.abilityDamage * 2;
            } else

            // calculate elemental weakness
            if (ability.weakAgainstLava) {
                damageDisplayIndicator.DisplayDamageIndicator(ability, collision.gameObject.GetComponent<UseElementalAbility>().ability);
                damage = ability.abilityDamage - 2;
            } else

            // lower the lava monsters health
            damageDisplayIndicator.DisplayDamageIndicator(ability, collision.gameObject.GetComponent<UseElementalAbility>().ability);
            collision.gameObject.transform.GetComponent<ExtendedStateController>().TakeDamage(damage);
        }

        // logic for hitting a ice monsters
        if (collision.gameObject.tag == "IceMonster") {

            // calculate elemental weakness
            if (ability.strongAgainstIce) {
                damageDisplayIndicator.DisplayDamageIndicator(ability, collision.gameObject.GetComponent<UseElementalAbility>().ability);
                damage = ability.abilityDamage * 2;
            } else

            // calculate elemental weakness
            if (ability.weakAgainstIce) {
                damageDisplayIndicator.DisplayDamageIndicator(ability, collision.gameObject.GetComponent<UseElementalAbility>().ability);
                damage = ability.abilityDamage - 2;
            } else

                // lower the ice monsters health
            damageDisplayIndicator.DisplayDamageIndicator(ability, collision.gameObject.GetComponent<UseElementalAbility>().ability);
            collision.gameObject.transform.GetComponent<ExtendedStateController>().TakeDamage(damage);
        }

        // logic for hitting a nature monster
        if (collision.gameObject.tag == "NatureMonster") {

            // calculate elemental weakness
            if (ability.strongAgainstNature) {
                damageDisplayIndicator.DisplayDamageIndicator(ability, collision.gameObject.GetComponent<UseElementalAbility>().ability);
                damage = ability.abilityDamage * 2;
            } else

            // calculate elemental weakness
            if (ability.weakAgainstNature) {
                damageDisplayIndicator.DisplayDamageIndicator(ability, collision.gameObject.GetComponent<UseElementalAbility>().ability);
                damage = ability.abilityDamage - 2;
            } else

            // lower the nature monsters health
            damageDisplayIndicator.DisplayDamageIndicator(ability, collision.gameObject.GetComponent<UseElementalAbility>().ability);
            collision.gameObject.transform.GetComponent<ExtendedStateController>().TakeDamage(damage);
        }

        // logic for hitting a nature monster
        if (collision.gameObject.tag == "DummyMonster") {
            // lower the nature monsters health
            //collision.gameObject.transform.GetComponent<ExtendedStateController>().TakeDamage(ability.abilityDamage);
        }

        // logic for hitting a lava monsters
        if (collision.gameObject.tag == "Player") {
            // lower the lava monsters health
            if (source) damage *= source.GetComponent<StateController>().attackDamageMultiplier;
            collision.gameObject.transform.GetComponent<Player>().TakeDamage(damage, explosion);
        }
    }

    public IEnumerator displayText(float seconds) {
        damageDisplayIndicator.canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(seconds);
        damageDisplayIndicator.canvasGroup.alpha = 0f;
    }
}
