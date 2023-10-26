using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutdoorScene : MonoBehaviour {

    public CanvasGroup canvasGroup;
    public string skipButtonAxisName = "Cancel";


    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
        StartCoroutine(FadeInAndOut(1, 42, 3));
    }

    private void Update() {
        if (Input.GetButtonDown(skipButtonAxisName)) {
            Cursor.visible = true;
            SceneManager.LoadScene(2);
        }
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
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, f / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    IEnumerator Wait(float timeToWait) {
        yield return new WaitForSeconds(timeToWait);

    }

    IEnumerator FadeInAndOut(float fadeInTime, float waitTime, float fadeOutTime) {

        StartCoroutine(FadeOut(fadeOutTime, canvasGroup));

        yield return new WaitForSeconds(waitTime);

        StartCoroutine(FadeIn(fadeInTime, canvasGroup));

        yield return new WaitForSeconds(1);

        Cursor.visible = true;

        SceneManager.LoadScene(2);

    }
}
