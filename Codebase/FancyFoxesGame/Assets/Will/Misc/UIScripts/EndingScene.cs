using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public GameObject character;
    public string skipButtonAxisName = "Cancel";

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
        StartCoroutine(FadeInAndOut(1, 92, 1));
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown(skipButtonAxisName)) {
            SceneManager.LoadScene(13);
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

        StartCoroutine(FadeOut(3, canvasGroup));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FadeIn(3, canvasGroup));
        yield return new WaitForSeconds(3);
        Cursor.visible = true;
        SceneManager.LoadScene(13);
    }
}
