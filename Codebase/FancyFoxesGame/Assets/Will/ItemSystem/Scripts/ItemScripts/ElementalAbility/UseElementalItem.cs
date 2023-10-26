using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseElementalItem : MonoBehaviour {

    public ItemElementalAbility item;
    private Vector3 pos;
    private float itemFloatSpeed;
    private float itemFloatHeight;
    public float itemStartOffset = 2;
    private bool doesItFloat;
    private GameObject childsTextObject;
    private GameObject player;
    private UseElementalAbility useElementalAbilityScript;
    private bool interactable;
    public string itemButtonAxisName = "Interact";
    public bool isWithinRange;
    public GameObject keyboardKey;

    void Start() {

        pos = Vector3.zero;

        // set the floating/bobbing stats
        doesItFloat = item.doesItemFloat;
        itemFloatSpeed = item.floatSpeed;
        itemFloatHeight = item.floatHeight;

        // get the player object
        player = GameManager.Instance.PlayerObject;

        // get the child component, so we can adjust the text
        childsTextObject = this.gameObject.transform.GetChild(1).gameObject;
        childsTextObject.GetComponent<TextMesh>().text = item.itemText;

        // gets reference to players script 
        useElementalAbilityScript = player.GetComponent<UseElementalAbility>();
    }

    void Update() {

        // checking if the item bobs up and down
        if (doesItFloat == true) { ItemBob(); }
        // text faces the camera
        childsTextObject.transform.rotation = (Camera.main.transform.rotation);

        // if player chooses, they pickup the item
        if (interactable && Input.GetButtonDown(itemButtonAxisName)) {
            PickupItem();
        }
    }

    // controlls the item bobbing up and down in place
    private void ItemBob() {
        float newY = Mathf.Sin(Time.time * itemFloatSpeed) * itemFloatHeight + pos.y;
        transform.position = new Vector3(transform.position.x, (newY + itemStartOffset), transform.position.z);
    }

    // determines what happens when the player touches object
    private void PickupItem() {
        useElementalAbilityScript.ability = item.ability;
        useElementalAbilityScript.shotsRemaining = item.ability.maximumShots;
        if(!this.CompareTag("InSandboxRoom"))
            this.gameObject.SetActive(false);
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
