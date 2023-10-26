using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementalAbilityUI : MonoBehaviour {


    // ability fields
    private ElementalAbility ability;
    private UseElementalAbility useElementalAbilityScript;
    private GameObject player;
    private ElementalAbility currentAbility;
    public TMPro.TextMeshProUGUI currentAbilityName;
    private int currentAbilityPowerLevel;
    public GameObject[] powerBars;
    public CanvasGroup tier1CanvasGroup;
    public CanvasGroup tier2CanvasGroup;
    public CanvasGroup tier3CanvasGroup;
    public CanvasGroup weakAgainstCanvasGroup;
    public CanvasGroup strongAgainstCanvasGroup;
    public CanvasGroup dividerCanvasGroup;


    // UI fields
    public Image currentAbilityImage;
    public Image currentAbilityWeaknessIcon;
    public Image currentAbilityStrengthIcon;

    // Start is called before the first frame update
    void Start() {

        // gets the current player
        player = GameManager.Instance.PlayerObject;
        useElementalAbilityScript = player.GetComponent<UseElementalAbility>();

        // get the player elemental ability
        currentAbility = useElementalAbilityScript.ability;

        UpdatePowerIcons();
    }

    // Update is called once per frame
    void Update() {
        // get the player elemental ability
        currentAbility = useElementalAbilityScript.ability;
        currentAbilityImage.sprite = currentAbility.abilityIcon;
        currentAbilityWeaknessIcon.sprite = currentAbility.weakAgainstIcon;
        currentAbilityStrengthIcon.sprite = currentAbility.strongAgainstIcon;
        currentAbilityName.text = currentAbility.abilityName;

        // if we have the base ability there are no
        // strength and weakness
        if(currentAbility.abilityName == "Default") {
            Hide(strongAgainstCanvasGroup);
            Hide(weakAgainstCanvasGroup);
            Hide(dividerCanvasGroup);
        } else {
            Show(strongAgainstCanvasGroup);
            Show(weakAgainstCanvasGroup);
            Show(dividerCanvasGroup);
        }
        
        UpdatePowerIcons();
    }

    private void UpdatePowerIcons() {
        // tier 0 ability
        if (currentAbility.powerLevel == 0) {
            Hide(tier1CanvasGroup);
            Hide(tier2CanvasGroup);
            Hide(tier3CanvasGroup);
            // tier 1 ability
        } else if (currentAbility.powerLevel == 1) {
            Show(tier1CanvasGroup);
            Hide(tier2CanvasGroup);
            Hide(tier3CanvasGroup);
            // tier 2 ability
        } else if (currentAbility.powerLevel == 2) {
            Show(tier1CanvasGroup);
            Show(tier2CanvasGroup);
            Hide(tier3CanvasGroup);
            // tier 3 ability
        } else if (currentAbility.powerLevel == 3) {
            Show(tier1CanvasGroup);
            Show(tier2CanvasGroup);
            Show(tier3CanvasGroup);
        }
    }

    // hides the canvas group
    void Hide(CanvasGroup canvasGroup) {
        canvasGroup.alpha = 0f; //this makes everything transparent
    }

    void Show(CanvasGroup canvasGroup) {
        canvasGroup.alpha = 1f; //this makes everything transparent
    }
}
