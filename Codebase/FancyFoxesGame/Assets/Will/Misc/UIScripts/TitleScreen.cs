using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    public CanvasGroup fancyFoxesCanvasGroup;
    public CanvasGroup fullsailCanvasGroup;
    public TMPro.TextMeshProUGUI canvasText;
    public string skipButtonAxisName = "Cancel";


    private void Start() {
        Cursor.visible = false;
        StartCoroutine(FadeInAndOut(2, 2, 2));
    }

    private void Update() {
        if (Input.GetButtonDown(skipButtonAxisName)) {
            SceneManager.LoadScene(1);
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

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FadeIn(fadeInTime, fancyFoxesCanvasGroup));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FadeOut(fadeOutTime, fancyFoxesCanvasGroup));

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FadeIn(fadeInTime, fullsailCanvasGroup));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FadeOut(fadeOutTime, fullsailCanvasGroup));

        yield return new WaitForSeconds(2);
        Cursor.visible = true;
        SceneManager.LoadScene(1);
    }

}
