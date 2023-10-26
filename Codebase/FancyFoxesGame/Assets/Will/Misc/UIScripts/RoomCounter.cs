using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCounter : MonoBehaviour {

    public TMPro.TextMeshProUGUI roomCounterText;

    private GameObject roomCounterObject;
    private LoadLevelScript loadLevelScript;

    // Start is called before the first frame update
    void Start() {
        roomCounterObject = GameObject.FindGameObjectWithTag("SceneTriggerCube");
        loadLevelScript = roomCounterObject.GetComponent<LoadLevelScript>();
    }

    // Update is called once per frame
    void Update() {
        if (LoadLevelScript.currLevel == 0) {
            roomCounterText.text = "Tutorial";
        } else if (LoadLevelScript.currLevel == 8) {
            roomCounterText.text = "Final Room!";
        } else {
            roomCounterText.text = LoadLevelScript.currLevel + " of 8";
        }
    }
}
