using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScene : MonoBehaviour {

    public TMPro.TextMeshProUGUI factText;
    public TMPro.TextMeshProUGUI enemyNameText;
    public GameObject budCharacter;
    public GameObject dragonCharacter;
    public GameObject shellCharacter;
    public GameObject mageCharacter;
    public CanvasGroup defaultCanvasGroup;
    public CanvasGroup titleCanvasGroup;

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
        titleCanvasGroup.alpha = 0;
        budCharacter.SetActive(false);
        dragonCharacter.SetActive(false);
        shellCharacter.SetActive(false);
        mageCharacter.SetActive(false);
        StartCoroutine(FadeInAndOut(5, 12, 5));
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator FadeIn(float fadeTime, CanvasGroup canvasGroup) {
        for (float f = 0; f <= fadeTime; f += Time.deltaTime) {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, f / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    IEnumerator FadeOut(float fadeTime, CanvasGroup canvasGroup) {
        for (float f = fadeTime; f > 0; f -= Time.deltaTime) {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, f / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    IEnumerator FadeInAndOut(float fadeInTime, float waitTime, float fadeOutTime) {
        yield return new WaitForSeconds(6);


        StartCoroutine(FadeOut(fadeOutTime, defaultCanvasGroup));

        // Show the Bud Character
        enemyNameText.text = "Bud";
        factText.text = "Favorite Food: Salad";
        budCharacter.SetActive(true);
        dragonCharacter.SetActive(false);
        shellCharacter.SetActive(false);
        mageCharacter.SetActive(false);
        StartCoroutine(FadeOut(fadeOutTime, defaultCanvasGroup));
        StartCoroutine(FadeIn(fadeInTime, defaultCanvasGroup));

        yield return new WaitForSeconds(waitTime);

        // Show the Dragon Character
        budCharacter.SetActive(false);
        dragonCharacter.SetActive(true);
        shellCharacter.SetActive(false);
        mageCharacter.SetActive(false);
        enemyNameText.text = "Hotrod";
        factText.text = "Favorite Food: Toasted Marshmallows";
        StartCoroutine(FadeOut(fadeOutTime, defaultCanvasGroup));
        StartCoroutine(FadeIn(fadeInTime, defaultCanvasGroup));

        yield return new WaitForSeconds(waitTime);

        // Show the Shell Character
        budCharacter.SetActive(false);
        dragonCharacter.SetActive(false);
        shellCharacter.SetActive(true);
        mageCharacter.SetActive(false);
        enemyNameText.text = "Shelly";
        factText.text = "Favorite Food: Sea Salt Chips";
        StartCoroutine(FadeOut(fadeOutTime, defaultCanvasGroup));
        StartCoroutine(FadeIn(fadeInTime, defaultCanvasGroup));

        yield return new WaitForSeconds(waitTime);

        // Show the Mage Character
        budCharacter.SetActive(false);
        dragonCharacter.SetActive(false);
        shellCharacter.SetActive(false);
        mageCharacter.SetActive(true);
        enemyNameText.text = "George";
        factText.text = "Favorite Food: Pizza";
        StartCoroutine(FadeOut(fadeOutTime, defaultCanvasGroup));
        StartCoroutine(FadeIn(fadeInTime, defaultCanvasGroup));

        yield return new WaitForSeconds(waitTime);
        mageCharacter.SetActive(false);
        gameObject.SetActive(false);

        Cursor.visible = true;
    }
}
