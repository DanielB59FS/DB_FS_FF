using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AbilityCooldown : MonoBehaviour {

    [HideInInspector] [SerializeField] public bool abilityUsed;
    [HideInInspector] [SerializeField] public Ability ability;
    [HideInInspector] public Image abilityIcon;
    [HideInInspector] public UseElementalAbility useElementalAbilityScript;
    [HideInInspector] public float coolDownDuration;
    [HideInInspector] public GameObject playerObject;
    private float nextReadyTime;
    private float coolDownTimeLeft;
    public Image darkMask;
    public Text coolDownTextDisplay;
    public CanvasGroup abilityCanvasGroup;

    // Start is called before the first frame update
    void Start() {
        Initialize(ability);
        abilityUsed = false;

        // get the player from the game manager
        playerObject = GameManager.Instance.PlayerObject;
    }

    public void Initialize(Ability selectedAbility) {
        ability = selectedAbility;
        //abilityIcon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {

        // find the player object
        useElementalAbilityScript = playerObject.GetComponent<UseElementalAbility>();

        // checks if the cooldown is ready
        bool coolDownComplete = (Time.time > nextReadyTime);

        // if the timer is done
        if (coolDownComplete) {
            GameManager.Instance.PlayerScript.GetComponent<Animator>().SetBool("Cooldown", false);
            useElementalAbilityScript.isAbilityReady = true;
            AbilityReady();
            Hide();
            // its ready now, and checks for player input
            if (useElementalAbilityScript.abilityUsed) {
                ButtonTriggered();
            }
        } else {
            // ability is not ready
            GameManager.Instance.PlayerScript.GetComponent<Animator>().SetBool("Cooldown", true);
            useElementalAbilityScript.isAbilityReady = false;
            CoolDown();
        }
    }

    // displays correctly when the ability is ready
    public void AbilityReady() {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }

    /* This function handles the games clock for
    * the ability system. It displays the icon in
    * the hud showing the ability is ready.  */
    private void CoolDown() {
        Show();
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCoolDown = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCoolDown.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    // Handles what happens when player/enemy triggers the ability
    public void ButtonTriggered() {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;
    }

    // hides the canvas group
    void Hide() {
        if (abilityCanvasGroup != null) {
            abilityCanvasGroup.alpha = 0f; //this makes everything transparent
        }
    }

    void Show() {
        if (abilityCanvasGroup != null) {
            abilityCanvasGroup.alpha = 1f; //this makes everything transparent
        }
    }
}
