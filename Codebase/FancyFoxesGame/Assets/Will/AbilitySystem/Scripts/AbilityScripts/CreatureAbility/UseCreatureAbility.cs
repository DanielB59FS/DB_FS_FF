using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseCreatureAbility : MonoBehaviour {

    public CreatureAbility creatureAbility;
    public CreatureAbility newCreatureAbility;
    private GameObject activatedCreatureAbility;
    public GameObject explosionPrefab;
    public GameObject spawnPoint;
    public Vector3 spawnPointStartLocation;
    public float distance;
    private float usageTimeLeft;
    private float nextReadyTime;
    private float coolDownTimeLeft;
    bool coolDownComplete;
    float tempTime;
    [HideInInspector] public float coolDownDuration;
    //[HideInInspector] public float coolDownDuration;
    public string abilityButtonAxisName = "Fire2";
    public bool abilityReady;
    public bool abilityUsed;
    public bool abilityEnded;
    //public bool isActive;
    public Image darkMask;
    public Text coolDownTextDisplay;
    public CanvasGroup creatureAbilityKey;
    private GameObject creatureAbilityUI;
    private CreatureAbilityUI creatureAbilityUIScript;
    private Quaternion creatureAbilityRotation;
    private Vector3 creatureAbilitySpawnPoint;
    public GameObject spawnStartingPoint;
    public bool inUse;

    void Awake() {
        if (GlobalSettings.Instance.loadPlayer && CompareTag("Player")) {
            creatureAbility = GlobalSettings.Instance.creatureAbility;
        }
    }

    private void Start() {
        abilityUsed = false;
        spawnPointStartLocation = spawnPoint.transform.position;
        creatureAbilityUI = GameObject.FindGameObjectWithTag("CreatureAbilityUI");
        creatureAbilityUIScript = creatureAbilityUI.GetComponent<CreatureAbilityUI>();
        darkMask = creatureAbilityUIScript.darkMask;
        coolDownTextDisplay = creatureAbilityUIScript.countDownText;
        creatureAbilityKey = creatureAbilityUIScript.creatureAbilityKey;
        spawnStartingPoint.transform.position = spawnPointStartLocation;
    }

    private void Update() {

        bool coolDownComplete = (Time.time > nextReadyTime);
        ActiveCreatureAbility();

        if (coolDownComplete) {
            abilityReady = true;
            AbilityReady();
            if (abilityUsed) {
                ButtonTriggered();
            }
        } else {
            abilityReady = false;
            CoolDown();
        }
    }

    public void ActiveCreatureAbility() {

        if (gameObject.tag == "Player" && !GameManager.Instance.isPaused) {
            if (creatureAbility != null) {
                if (Input.GetButtonDown(abilityButtonAxisName) && abilityReady) {

                    abilityUsed = true;

                    // set the rotation for the creature ability
                    // some abilites need manual rotatation as
                    // specified in the scriptable object
                    if (creatureAbility.rotationOverride) {
                        creatureAbilityRotation = Quaternion.Euler(
                            new Vector3(creatureAbility.rotationAngleX,
                                        creatureAbility.rotationAngleY,
                                        creatureAbility.rotationAngleZ));
                    } else {
                        // no custom rotation is needed
                        creatureAbilityRotation = transform.rotation;
                    }

                    // set the spawn point for the creature ability
                    // some abilites need manual positioned as
                    // specified in the scriptable object
                    if (!creatureAbility.spawnFromProjectileSpawnPoint) {
                        creatureAbilitySpawnPoint = new Vector3(transform.position.x,
                                                         transform.position.y,
                                                         transform.position.z);
                    } else {
                        creatureAbilitySpawnPoint = spawnPoint.transform.position;
                    }

                    // instantiate the shield
                    activatedCreatureAbility =
                    Instantiate(creatureAbility.projectilePrefab,
                                creatureAbilitySpawnPoint,
                                creatureAbilityRotation);

                    // set it to the parent object
                    activatedCreatureAbility.transform.parent = gameObject.transform;

                    darkMask.enabled = false;
                    coolDownTextDisplay.enabled = false;

                    // move the spawn point for the projectiles outward
                    spawnPoint.transform.position += spawnPoint.transform.forward * creatureAbility.spawnFromAdjustmentZ;
                    spawnPoint.transform.position += spawnPoint.transform.up * creatureAbility.spawnFromAdjustmentY;

                } else {
                    abilityUsed = false;
                }
            }
        }
    }

    // displays correctly when the ability is ready
    public void AbilityReady() {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
        abilityReady = true;
        abilityEnded = false;
    }

    void ButtonTriggered() {
        nextReadyTime = creatureAbility.cooldownDuration + creatureAbility.useDuration + Time.time;
        coolDownTimeLeft = (creatureAbility.cooldownDuration) + creatureAbility.useDuration;
        usageTimeLeft = creatureAbility.useDuration;
    }

    private void CoolDown() {
        if (creatureAbility != null) {

            // destroy the prefab
            Destroy(activatedCreatureAbility, creatureAbility.useDuration);
            coolDownTimeLeft -= Time.deltaTime;
            if (coolDownTimeLeft < creatureAbility.cooldownDuration) {
                darkMask.enabled = true;
                coolDownTextDisplay.enabled = true;
                spawnPoint.transform.position = spawnStartingPoint.transform.position;
            }
            float roundedCoolDown = Mathf.Round(coolDownTimeLeft);
            coolDownTextDisplay.text = roundedCoolDown.ToString();
            darkMask.fillAmount = (coolDownTimeLeft / creatureAbility.cooldownDuration);
        }
    }
}