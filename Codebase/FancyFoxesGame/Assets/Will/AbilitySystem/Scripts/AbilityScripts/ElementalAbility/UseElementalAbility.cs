using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is responsible for instantiating and moving forward
// elemental ability.


public class UseElementalAbility : MonoBehaviour {

    [SerializeField] public ElementalAbility ability;

    [HideInInspector] public float abilitySpeed;
    [HideInInspector] public GameObject abilityCooldownUI;
    [HideInInspector] public bool abilityUsed;
    [HideInInspector] public bool isAbilityReady;
    [HideInInspector] public AbilityCooldown abilityCooldownScript;
    [HideInInspector] public FrenzyAttack frenzyAttackScript;
    private int shotsMax;
    public int shotsRemaining;
    public string abilityButtonAxisName = "Fire1";
    public ElementalAbility baseAbility;
    public GameObject spawnPoint;
    GameObject activatedElementalAbility;
    public int killCount;
    public int killsNeededForFrenzy;

    void Awake() {
        if (GlobalSettings.Instance.loadPlayer && CompareTag("Player")) {
            ability = GlobalSettings.Instance.playerAbility;
        }
    }

    private void Start() {

        // get the max shots
        shotsMax = ability.maximumShots;

        frenzyAttackScript = gameObject.GetComponent<FrenzyAttack>();

        // gets the gameobject that has the cooldown script on it
        if (this.gameObject.CompareTag("Player")) {
            abilityCooldownUI = GameObject.FindWithTag("ElementalAbilityUI");
            abilityCooldownScript = abilityCooldownUI.GetComponent<AbilityCooldown>();
        }

        // set initial shot count
        if (GlobalSettings.Instance.loadPlayer)
            shotsRemaining = GlobalSettings.Instance.shotsRemaining;
        else
            shotsRemaining = shotsMax;
    }

    void Update() {

        // keep the icon and cooldown correct on update
        if (this.gameObject.CompareTag("Player")) {
            abilityCooldownScript.coolDownDuration = ability.abilityBaseCooldown;
        }

        // listens for triggering of the ability
        ActiveElementalAbility();

        // if we run out of shots, default to base ability
        if (shotsRemaining <= 0) {
            ability = baseAbility;
            // this number is abritrary, could be set to anything
            shotsRemaining = 10;
        }

        // keep the frenzy count updated
        killCount = GameStats.Instance.frenzyMeterCount;

        // checks how many kills we have
        if (gameObject.CompareTag("Player")) {
            if (killCount >= killsNeededForFrenzy) {
                frenzyAttackScript.FrenzyModeOn(5);
            } 
        }
    }

    // instantiate and push the projectile to fire
    public void ActiveElementalAbility() {

        // for player input
        if (gameObject.tag == "Player" && !GameManager.Instance.isPaused) {
            // checks for keypress and makes sure the ability is ready
            if (Input.GetButtonDown(abilityButtonAxisName) && isAbilityReady && 0 < GameManager.Instance.PlayerScript.healthValue) {

                // sends the message to the cooldown script
                abilityUsed = true;

                // decrement total shot limit
                if (ability != baseAbility) {
                    shotsRemaining -= 1;
                }

                // set the x, y, and z positions so we can adjust the positions
                float x = spawnPoint.transform.position.x;
                float y = spawnPoint.transform.position.y + 1f;
                float z = spawnPoint.transform.position.z;
                Vector3 spawnPosition = new Vector3(x, y, z);

                // use the elemental ability
                activatedElementalAbility = Instantiate(ability.projectilePrefab, spawnPosition, spawnPoint.transform.rotation);
                activatedElementalAbility.GetComponent<ElementalAbilityAttack>().source = gameObject;

                // add force and move the projectile
                activatedElementalAbility.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, ability.abilitySpeed));

            } else {
                // waiting for cooldown script to tell us the ability is ready
                abilityUsed = false;
            }
        }
    }

    // place holder for monster AI call
    public void AlternateUseAbility(GameObject altSpawnPoint, Collider toIgnore) {

        // use the elemental ability
        GameObject activatedElementalAbility = Instantiate(ability.projectilePrefab, altSpawnPoint.transform.position, altSpawnPoint.transform.rotation);
        activatedElementalAbility.GetComponent<ElementalAbilityAttack>().source = gameObject;
        activatedElementalAbility.tag = "EnemyAbility"; //allows AI to ignore their own abilities, ignorecollision method not working.
        // add force and move the projectile
        activatedElementalAbility.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, ability.abilitySpeed));
    }

    // destroys projectile after it leaves the view
    public void OnBecameInvisible() {
        Destroy(activatedElementalAbility);
    }
}