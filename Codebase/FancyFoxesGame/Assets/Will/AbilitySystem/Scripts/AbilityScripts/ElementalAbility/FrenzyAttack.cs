using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzyAttack : MonoBehaviour {

    // frenzy variables
    public int frenzyTimer;
    public CanvasGroup frenzyUI;
    public ElementalAbility currentAbility;
    private UseElementalAbility useElementalAbilityScript;
    public int shotsRemaining;
    public bool frenzyActive;

    // Start is called before the first frame update
    void Start() {
        frenzyUI = GameObject.FindGameObjectWithTag("FrenzyUI").GetComponent<CanvasGroup>();
        frenzyActive = false;
    }

    // Update is called once per frame
    void Update() {
        useElementalAbilityScript = gameObject.GetComponent<UseElementalAbility>();
    }

    // turns on frenzy
    public void FrenzyModeOn(int frenzyTimer) {
        frenzyActive = true;
        frenzyUI.alpha = 1;
        /*
         * TODO: (Will)
         * A temporary band aid fix to avoid errors.
         */
        if (useElementalAbilityScript) {
            useElementalAbilityScript.shotsRemaining = 100000;
            useElementalAbilityScript.abilityCooldownScript.coolDownDuration = 0;
        }
        Invoke("FrenzyModeOff", frenzyTimer);
    }

    // turn off frenzy
    public void FrenzyModeOff() {
        while (frenzyActive) {
            frenzyActive = false;
            // reset the kill count
            useElementalAbilityScript.killCount = 0;
            // reset the kill count
            GameStats.Instance.frenzyMeterCount = 0;
            // turn the frenzy UI off
            frenzyUI.alpha = 0;
            // reset the cooldown 
            useElementalAbilityScript.abilityCooldownScript.coolDownDuration =
                useElementalAbilityScript.ability.abilityBaseCooldown;
            // set back to base ability
            useElementalAbilityScript.ability = useElementalAbilityScript.baseAbility;
        }
    }
}
