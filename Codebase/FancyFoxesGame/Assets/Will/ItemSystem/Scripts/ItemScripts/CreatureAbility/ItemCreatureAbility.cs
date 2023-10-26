using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Creature Ability Item")]
public class ItemCreatureAbility : Item {

    [HideInInspector] public UseCreatureAbility useCreatureAbilityScript;
    [HideInInspector] private GameObject player;
    public CreatureAbility ability;

    public void Initialize(GameObject gameObject) {
        // get the player from the game manager
        player = GameManager.Instance.PlayerObject;

        // get the script from the player object
        useCreatureAbilityScript = player.GetComponent<UseCreatureAbility>();
    }

    public void PickupItem() {
        // obtain the item
        //useElementalAbilityScript.ability = ability;
    }
}
