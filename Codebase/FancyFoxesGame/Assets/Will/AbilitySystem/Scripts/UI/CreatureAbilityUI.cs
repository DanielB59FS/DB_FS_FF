using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureAbilityUI : MonoBehaviour {

    public Image creatureAbilityImage;
    public TMPro.TextMeshProUGUI creatureAbilityText;
    private GameObject player;
    private UseCreatureAbility useCreatureAbilityScript;
    public CanvasGroup creatureAbilityKey;
    public Image darkMask;
    public Text countDownText;


    // Start is called before the first frame update
    void Start() {
        player = GameManager.Instance.PlayerObject;
        useCreatureAbilityScript = player.GetComponent<UseCreatureAbility>();
    }

    // Update is called once per frame
    void Update() {
        if (useCreatureAbilityScript.creatureAbility != null) {
            creatureAbilityImage.sprite = useCreatureAbilityScript.creatureAbility.abilityIcon;
            creatureAbilityText.text = useCreatureAbilityScript.creatureAbility.abilityName;
            creatureAbilityKey = useCreatureAbilityScript.creatureAbilityKey;
        }
    }
}
