using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
//Require these components for dynamic creature building at runtime.
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]

public class AIController : MonoBehaviour {
    public enum States { Hostile, Idle, Death, Flee, Rally }

    State currentState;

    NavMeshAgent agent;
    [SerializeField] MonsterSO monster_data;
    [SerializeField] List<GameObject> nearbyAllies = new List<GameObject>();
    public GameObject smokePuff;
    private float maxHp;
    private int minHp, mana, stamina, aggroRange, teamwork, bravery, intelligence, allyCount, allyBonus; //Attributes for AI logic, will be added functionality as AI gets smarter
    public float hp;
    public GameObject target;
    SphereCollider spColl; //Collider being used for range detection
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    SkinnedMeshRenderer renderer; //user to change color for damage
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public float powerDropRate;
    public GameObject powerUp;
    [SerializeField] private float flashTime = 0.5f;
    Color origionalColor;
    private bool isDying;
    public float dot;
    public GameObject spawnpoint;

    public static int enemiesInRoom; //incremented by the spawnerManager everytime something is spawned;
    public bool hasRallied;

    #region UnityMethods
    // Start is called before the first frame update
    void Start() {
        
        CopyDataFromSO(); //Grab data from the SO
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        origionalColor = renderer.material.color;
        GetComponent<Rigidbody>().isKinematic = true; //currently has a bug where some external force is pushing the AI around, this is a temp workaround
        InitializeState();

    }

    // Update is called once per frame
    void Update() {
        if (currentState != null) //If there is a state, execute it's behavior
        {
            currentState.Execute(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) //When the AI detects player, check what state it should go in
        {
            target = other.gameObject;
            CheckForStateConditions();

        }
        else if (other.gameObject.name == "Monster" && !nearbyAllies.Contains(other.gameObject)) //When the AI detects an ally, add to the ally count.
        {
            nearbyAllies.Add(other.gameObject);
            allyCount = nearbyAllies.Count;
        }
    }

    private void OnTriggerExit(Collider other) //set ability back to layer so it can be detected by another AI.
    {
        if (other.gameObject.tag == "Ability")
        {
            other.gameObject.layer = 10;
        }
    }

    private void OnDestroy() //Make sure to clean up enemies list.
    {
        if (GameManager.Instance.Enemies.Contains(this.gameObject))
        {
            GameManager.Instance.Enemies.Remove(this.gameObject);
        }
    }
    #endregion
    
    void CopyDataFromSO() //Grabs  data from the SO and GO and replicates it on this object while setting up the components for use.
    {
        spColl = GetComponent<SphereCollider>(); //This bit of code generates a sphere collider at runtime with a radius equal to the given aggro range.
        spColl.isTrigger = true;
        spColl.radius = aggroRange;
       
        if (hp == 0)
        {
            hp = monster_data.hpMax;
        }
        
        //Just copying data from the scriptable object to manipulate later
        maxHp = hp;
        minHp = monster_data.hpMin;
        mana = monster_data.mana;
        stamina = monster_data.stamina;
        teamwork = monster_data.teamwork;
        bravery = monster_data.bravery;
        intelligence = monster_data.intelligence;
        aggroRange = monster_data.range;
        powerUp = monster_data.powerupPickup;


    }
    void InitializeState() { //This controls what the AI will start in.
        GameManager.Instance.Enemies.Add(this.gameObject);
        if (currentState == null) {
            ChangeState(States.Idle);

        }
    }

    #region StateControllingMethods
    public void ChangeState(States newState) {

        if (currentState) { //before changing states, exit the current and destroy it.
            currentState.Exit(gameObject);
            Destroy(currentState);
        }

        switch (newState) {  //Here is where we switch states, to add a new state it must be added here as well as in the States enum
            case States.Hostile:
                currentState = gameObject.AddComponent<HostileState>();
                break;
            case States.Idle:
                currentState = gameObject.AddComponent<IdleState>();
                break;
            case States.Death:
                currentState = gameObject.AddComponent<DeathState>();
                break;
            case States.Flee:
                currentState = gameObject.AddComponent<FleeState>();
                break;
            case States.Rally:
                currentState = gameObject.AddComponent<RallyState>();
                break;
            default:
                break;
        }
        currentState.Enter(this.gameObject); //initialize the selected state.
    }


    public void CheckForStateConditions()
    {
        int fleeMod, rallyMod;

        System.Random rng = new System.Random();

        allyBonus = teamwork * allyCount; //a value that can be added to state checks that are affected by having allies near the AI
        fleeMod = 100 - ((bravery * 10) + allyBonus); //the percentage change that the creature will flee.
        rallyMod = teamwork * 10; //percentage chance to go into rally mode.
        int randomValue = rng.Next(100);

        enemiesInRoom = GameManager.Instance.Enemies.Count;

        if (isDying) { return; } //If we are currently dying, dont alter state.

        if (currentState == GetComponent<IdleState>()) //Only states accessible from idle can be in this branch. so far only hostile but will change later.
        {
            ChangeState(States.Hostile);
            return;
        }

        if ( randomValue < fleeMod && hp <= (maxHp / 2) && currentState != GetComponent<FleeState>() ) //If you arent fleeing, have less than half hp and fail the flee check, then flee.
        {
            ChangeState(States.Flee);
            return; //After changing a state, dont check for other states until the next call.
        } else if (teamwork > nearbyAllies.Count && (randomValue < rallyMod) && (nearbyAllies.Count <= (enemiesInRoom - 1)) && !hasRallied)
        {
            ChangeState(States.Rally);
            return;
        }
    }

    void OnDeath() {
        OnDeath(true);
    }

    void OnDeath(bool countScore) {
        FlashRed();
        ChangeState(States.Death);
        if (countScore) GameStats.Instance.OnKill(monster_data);
    }

    #endregion


    #region Getters/Setters
    public int GetRange() { return aggroRange; }
    public int GetMana() { return mana; }
    public int GetMinHp() { return minHp; }
    public State GetCurrentState () { return currentState; }

    public void SetData(MonsterSO data) {
        monster_data = data;
        CopyDataFromSO();
    }

    public int GetIntelligence() { return intelligence; }
    public int GetBravery() { return bravery; }
    public int GetTeamwork() { return teamwork; }
    public int GetAllyCount() { return allyCount; }
    public void SetTarget(GameObject _target) { target = _target; }
    #endregion

    public void TakeDamage(float damage, GameObject source) {
        


        if (target == null && !isDying) { //If the creature is damaged but is not currently targeting anything, new target is set by damage dealer
            target = source;
        }

        if (hp > 0 && source.tag != "EnemyAbility") //If target is alive, they reduce hp.
        {
            isDying = false;
            hp -= damage;
            FlashRed();
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("damageTaken");
        }

        if (hp <= 0 && !isDying) //If the monster has 0 or less hp after damage, it dies, if not it checks if it needs to switch states
        {
            isDying = true;
            OnDeath();
        } else {
            CheckForStateConditions();
        }
    }

    void FlashRed() {
        //Note to self, investigate setting the flash time equal to the hurt animation
        renderer.material.color = Color.red;
        Invoke("ResetColor", flashTime);
    }
    void ResetColor() {
        renderer.material.color = origionalColor;
    }

    public void OnRally() //Called by the rally state to increase movement speed of rallied targets, other benefits can be added here
    {
        if (!hasRallied)
        {
            hasRallied = true;
            agent.speed += 3;
        }
        
    }

}
