using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleFade : MonoBehaviour {

    public CanvasGroup canvasGroup;
    public GameObject character;
    public string skipButtonAxisName = "Cancel";

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(FadeInAndOut(1, 57, 1));
    }

    // Update is called once per frame
    void Update() {
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



        //StartCoroutine(FadeOut(fadeOutTime, canvasGroup));

        yield return new WaitForSeconds(waitTime);
        character.SetActive(false);


    StartCoroutine(FadeIn(fadeInTime, canvasGroup));

        yield return new WaitForSeconds(25);
        StartCoroutine(FadeOut(2, canvasGroup));
        Cursor.visible = true;
        SceneManager.LoadScene(2);



    }
}
