using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseConsumableItem : MonoBehaviour {

    public ItemConsumablePickup item;
    private Vector3 pos;
    private float itemFloatSpeed;
    private float itemFloatHeight;
    public float itemStartOffset = 2;
    private bool doesItFloat;
    private GameObject childsTextObject;
    private bool interactable;
    public string itemButtonAxisName = "Interact";
    public bool isWithinRange;
    public GameObject keyboardKey;

    // script references
    private GameObject player;
    private Player playerScript;

    public GameObject itemUI;
    private ItemUI itemUIScript;


    public void Initialize(ItemConsumablePickup selectedItem) {
        item = selectedItem;
    }

    void Start() {

        itemUI = GameObject.FindGameObjectWithTag("ItemUI");
        itemUIScript = itemUI.GetComponent<ItemUI>();

        pos = Vector3.zero;

        // set the floating/bobbing stats
        doesItFloat = item.doesItemFloat;
        itemFloatSpeed = item.floatSpeed;
        itemFloatHeight = item.floatHeight;

        // get the player object
        player = GameManager.Instance.PlayerObject;
        // get the player script
        playerScript = player.GetComponent<Player>();
        // get the child component, so we can adjust the text
        childsTextObject = this.gameObject.transform.GetChild(1).gameObject;
    }

    void Update() {

        // checking if the item bobs up and down
        if (doesItFloat == true) { ItemBob(); }

        if (interactable && Input.GetButtonDown(itemButtonAxisName)) {
            PickupItem();
        }
    }

    // controlls the item bobbing up and down in place
    private void ItemBob() {
        float newY = Mathf.Sin(Time.time * itemFloatSpeed) * itemFloatHeight + pos.y;
        transform.position = new Vector3(transform.position.x, (newY + itemStartOffset), transform.position.z);
    }

    private void PickupItem() {

        player.GetComponent<Player>().hasItem = true;
        gameObject.SetActive(false);
    }

    // determines what happens when the player touches object
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            interactable = true;
            isWithinRange = true;
            keyboardKey.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other) {
        // simple ternary to check player 
        if (other.gameObject.tag == ("Player")) {
            interactable = false;
            isWithinRange = false;
            keyboardKey.SetActive(false);
        }
    }
}
