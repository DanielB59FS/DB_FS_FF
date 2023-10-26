using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {


    public ItemConsumablePickup consumableItem;
    public Image itemIcon;
    public Image defaultIcon;
    private GameObject player;
    public GameObject healEffect;
    public GameObject itemText;
    public string itemButtonAxisName = "Potion";
    public int healhRestoreAmount = 5;


    // Start is called before the first frame update
    void Start() {
        player = GameManager.Instance.PlayerObject;

        // get the child component, so we can adjust the text
        itemText = gameObject.transform.GetChild(0).gameObject;
      
        // setup the icon
        itemIcon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {

        // only if not paused
        if (!GameManager.Instance.isPaused) {
            UseItem();
        }

        // update the hud with the item
        if (player.GetComponent<Player>().hasItem) {
            itemText.GetComponent<TMPro.TextMeshProUGUI>().text = consumableItem.itemName;
            itemIcon.sprite = consumableItem.ItemIcon;
        } else {
            itemText.GetComponent<TextMeshProUGUI>().text = "No Item";
        }

    }

    private void UseItem() {
        if (player.GetComponent<Player>().hasItem && Input.GetButtonDown(itemButtonAxisName)) {
            GameObject healVfx = Instantiate(healEffect, player.transform.position, player.transform.rotation);
            Destroy(healVfx, 1);
            player.GetComponent<Player>().RestoreHealth(healhRestoreAmount);
            player.GetComponent<Player>().hasItem = false;
            itemIcon.sprite = consumableItem.EmptyItemIcon;
        }
    }
}
