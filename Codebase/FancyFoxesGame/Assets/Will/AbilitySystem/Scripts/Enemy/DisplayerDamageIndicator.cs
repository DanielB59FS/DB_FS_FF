using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayerDamageIndicator : MonoBehaviour {

    public ElementalAbility elementalAbilityHit;
    public TMPro.TextMeshProUGUI damageIndicatorText;
    public CanvasGroup canvasGroup;

    //private void Update() {
    //    canvasGroup = gameObject.transform.GetComponent<CanvasGroup>();
    //}

    public void DisplayDamageIndicator(ElementalAbility ability, ElementalAbility enemyAbility) {

        // Lava Ability
        if (ability.isLavaType) {
            if (enemyAbility.isIceType) {
                damageIndicatorText.text = "Not Effective";
                canvasGroup.alpha = 1f;
                StartCoroutine(Hide());
            } else if (enemyAbility.isNatureType) {
                canvasGroup.alpha = 1f;
                damageIndicatorText.text = "Super Effective!!";
                StartCoroutine(Hide());
            } else {
                canvasGroup.alpha = 1;
                damageIndicatorText.text = "Effective";
                StartCoroutine(Hide());
            }
        }

        // Ice Ability
        if (ability.isIceType) {
            if (enemyAbility.isNatureType) {
                canvasGroup.alpha = 1f;
                damageIndicatorText.text = "Not Effective";
                StartCoroutine(Hide());
            } else if (enemyAbility.isLavaType) {
                canvasGroup.alpha = 1f;
                damageIndicatorText.text = "Super Effective!!";
                StartCoroutine(Hide());
            } else {
                canvasGroup.alpha = 1f;
                damageIndicatorText.text = "Effective";
                StartCoroutine(Hide());
            }
        }

        // Nature Ability
        if (ability.isNatureType) {
            if (enemyAbility.isLavaType) {
                canvasGroup.alpha = 1f;
                damageIndicatorText.text = "Not Effective";
                StartCoroutine(Hide());
            } else if (enemyAbility.isIceType) {
                canvasGroup.alpha = 1f;
                damageIndicatorText.text = "Super Effective!!";
                StartCoroutine(Hide());
            } else {
                canvasGroup.alpha = 1f;
                damageIndicatorText.text = "Effective";
                StartCoroutine(Hide());
            }
        }
    }

    private IEnumerator Hide() {
        yield return new WaitForSeconds(1);
        canvasGroup.alpha = 0f;
    }


}
