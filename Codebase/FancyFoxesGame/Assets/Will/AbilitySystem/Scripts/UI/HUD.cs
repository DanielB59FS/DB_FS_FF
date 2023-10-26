using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [HideInInspector] public GameObject player;
    private UseElementalAbility useElementalAbilityScript;
    public TMPro.TextMeshProUGUI shotCounter;
    private FrenzyAttack frenzyAttackScript;

    void Start() {

        // get the scripts data from the player
        player = GameManager.Instance.PlayerObject;
        useElementalAbilityScript = player.GetComponent<UseElementalAbility>();
        frenzyAttackScript = player.GetComponent<FrenzyAttack>();
    }

    void Update() {
        // check if we have the unlimited base ability
        if (useElementalAbilityScript.ability == useElementalAbilityScript.baseAbility || frenzyAttackScript.frenzyActive) {
            shotCounter.text = ('\u221e').ToString();
        } else {
            // otherwise use the counter for the shots
            shotCounter.text = useElementalAbilityScript.shotsRemaining.ToString();
        }
    }
}
