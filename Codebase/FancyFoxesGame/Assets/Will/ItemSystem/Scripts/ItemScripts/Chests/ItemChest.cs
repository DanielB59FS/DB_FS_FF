using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class ItemChest : MonoBehaviour {

    public Dictionary<GameObject, float> possibleItems;
    public AnimationClip chestOpenClip;
    public GameObject itemSpawnEffect;
    public GameObject keyboardKey;
    private bool chestUsed;
    private bool isWithinRange;
    // declare the array of all the items
    public PossibleItem[] itemOptions;
    public string itemButtonAxisName = "Interact";

    [Serializable]
    public struct PossibleItem {

        // member variables
        public GameObject item;
        public int percentChance;

        // sort the selection
        public static void SelectionSort(PossibleItem[] input) {
            for (var i = 0; i < input.Length; i++) {
                var min = i;
                for (var j = i + 1; j < input.Length; j++) {
                    if (input[min].percentChance > input[j].percentChance) {
                        min = j;
                    }
                }

                if (min != i) {
                    var lowerValue = input[min];
                    input[min] = input[i];
                    input[i] = lowerValue;
                }
            }
        }
    }


    // Start is called before the first frame update
    void Start() {
        // we havent used this chest yet
        chestUsed = false;

        // sort the array of items
        PossibleItem.SelectionSort(itemOptions);
    }

    // Update is called once per frame
    void Update() {
        if (isWithinRange && chestUsed == false) {
            if (Input.GetButtonDown(itemButtonAxisName)) {
                GenerateRandomItem(itemOptions);
                chestUsed = true;
                keyboardKey.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        // simple ternary to check playe

        if (!chestUsed) {
            if (other.CompareTag("Player")) {
                isWithinRange = true;
                keyboardKey.SetActive(true);
            }
        }
    }


    private void OnTriggerExit(Collider other) {
        // simple ternary to check player
        if (other.gameObject.tag == ("Player")) {
            isWithinRange = false;
            keyboardKey.SetActive(false);
        }

    }

    // generates the random item 
    void GenerateRandomItem(PossibleItem[] possibleItems) {

        // generate number 1-100
        int randomNumber = UnityEngine.Random.Range(1, 101);
        int tempCounter = 0;
        List<int> tempList = new List<int>();

        // convert the percent chance to actual comparable numbers
        foreach (PossibleItem itemOption in itemOptions) {
            tempCounter += itemOption.percentChance;
            tempList.Add(tempCounter);
        }

        // get the rarest item
        if (randomNumber < tempList[0]) {
            // play the animation
            GetComponent<Animator>().SetBool("openedChest", true);
            // instantiate the item
            StartCoroutine(waitAndInstantiateItem(0, 1.19f));
            // get the most common item
        } else if (randomNumber > tempList[tempList.Count - 2]) {
            // play the animation
            GetComponent<Animator>().SetBool("openedChest", true);
            // instantiate the item
            StartCoroutine(waitAndInstantiateItem(tempList.Count - 1, 1.19f));
        } else {
            // get the other available items
            for (int i = 1; i < tempList.Count; i++) {
                // get any of the middle results
                if ((randomNumber < tempList[i]) && (randomNumber >= tempList[i - 1])) {
                    // play the animation
                    GetComponent<Animator>().SetBool("openedChest", true);
                    // instantiate the item
                    StartCoroutine(waitAndInstantiateItem(i, 1.19f));
                    break;
                }
            }
        }
    }

    // this function waits a few seconds and then instantiates the item
    IEnumerator waitAndInstantiateItem(int index, float timeToWait) {
        yield return new WaitForSeconds(timeToWait);
        InstantiateItem(index);
    }

    // instantiates a new game object and returns it
    private void InstantiateItem(int index) {

        // spawn our random item
        GameObject item = Instantiate(itemOptions[index].item,
                                      transform.position + (transform.forward * 2),
                                      Quaternion.identity);

        // generate the visual effect
        GameObject effect = Instantiate(itemSpawnEffect,
                          item.transform.position,
                          item.transform.rotation);

    }
}