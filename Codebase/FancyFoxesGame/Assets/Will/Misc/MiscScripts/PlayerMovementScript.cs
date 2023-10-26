using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour {

    private void Start() { }

    void Update() {

        float speed;

        if (Input.GetButton("Fire3")) {
            speed = Time.deltaTime * 4;
        } else {
            speed = Time.deltaTime * 6;
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            this.transform.Translate(Vector3.forward * speed);
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            this.transform.Translate(Vector3.back * speed);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.Rotate(Vector3.down, 0.5f);
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            this.transform.Rotate(Vector3.up, 0.5f);
        }


    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "FireElementPickup") {
            //abilityCooldown.ability = GameObject.Find(projectileAbility.name = "");
        }
    }
}
