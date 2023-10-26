using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour
{

    public GameObject textHolder;
    private TextMeshPro text;

    //TextAreaAttribute(int minLines, int maxLines);
    [TextArea(10,10)]
    public string displayText;

    // Start is called before the first frame update
    void Start()
    {
        textHolder.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = (displayText);
        textHolder.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // simple ternary to check player 
        if (other.gameObject.tag == ("Player"))
        {
            textHolder.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // simple ternary to check player 
        if (other.gameObject.tag == ("Player"))
        {
            textHolder.SetActive(false);
        }
    }
}
