using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable Item")]
public class ItemConsumablePickup : Item
{
    [HideInInspector] public Player playerScript;
    [HideInInspector] private GameObject player;

    public void Initialize(GameObject gameObject) {
        // get the player from the game manager
        player = GameManager.Instance.PlayerObject;

        // get the script from the player object
        playerScript = player.GetComponent<Player>();
    }
}
