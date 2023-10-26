using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardKey : MonoBehaviour {

    public GameObject keyboardLetterHolder;
    private TextMeshPro text;
    public string chosenKey;


    // Start is called before the first frame update
    void Start() {
        keyboardLetterHolder.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = (chosenKey);

    }

    // Update is called once per frame
    void Update() {
       
    }
}
