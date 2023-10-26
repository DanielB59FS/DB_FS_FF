using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Elemental Ability Item")]
public class ItemElementalAbility : Item {

    [HideInInspector] public UseElementalAbility useElementalAbilityScript;
    public ElementalAbility ability;
    [HideInInspector] private GameObject player;

    public void Initialize(GameObject gameObject) {
        // get the player from the game manager
        player = GameManager.Instance.PlayerObject;

        // get the script from the player object
        useElementalAbilityScript = player.GetComponent<UseElementalAbility>();
    }

    public void PickupItem() {
        // obtain the item
        useElementalAbilityScript.ability = ability;
    }
}
