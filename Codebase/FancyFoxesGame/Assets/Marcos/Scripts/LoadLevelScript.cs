using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevelScript : MonoBehaviour
{

    private FrenzyAttack frenzyAttackScript;
    public GameObject player;
    public static int currLevel = 0; // Current level
    public static List<int> roomList = new List<int>(); // List to choose levels
    private static bool roomListInstantiated = false;
    bool roomCleared = false; // Are all enemies defeated?

    public GameObject[] allMonsters; // Array to keep monsters for randomizing
    public List<Spawnpoint> spawnPoints; // List of spawn points to randomize
    public static int EnemiesAmountToSpawn = 3; // Base amount to spawn
    private int enemiesAmountSpawned; // How many enemies spawned
    private bool keyRoomEnemiesAlreadySpawned = false;

    public List<GameObject> chestsList; // List of chest locations
    private static int chestsAmountToSpawn = 0; // How many chests to spawn
    private int chestsAmountSpawned; // Chests spawned

    public static float scaleValueHP = 3f; // Hp
    public static float scaleValueAS = 1.2f; // Attack speed


    private void Awake()
    {
        // Spawn Enemies
        if (SceneManager.GetActiveScene().name != "Room1_8")
            SpawnEnemies();
        else
            SpawnEnemiesKeyRoom(3);
        //Spawn Chests
        SpawnChests();
    }

    public void Start()
    {

        player = GameManager.Instance.PlayerObject;
        frenzyAttackScript = player.gameObject.GetComponent<FrenzyAttack>();

        // Instantiate the array for the possible rooms
        if (!roomListInstantiated) {
            AddRoomsToList();
        }
    }
    private void Update()
    {
        if (GameManager.Instance.Enemies.Count == 0 || SceneManager.GetActiveScene().name == "SandboxTutorialRoom") // Checks if all enemies are defeated
        {
            roomCleared = true; // Unlocks if yes
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false); // Door lock
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true); // Green Arrow
        }
        else // Locks if no
        {
            roomCleared = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().name != "Room1_8")
                this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other) // Trigger when collides with door
    {
        if (other.CompareTag("Player") && currLevel != 8) // Check that collider is player
        {
            if (roomCleared || SceneManager.GetActiveScene().name == "Room1_8") // Checks if all enemies are dead OR player is in key room
            {
                //GameStats.Instance.frenzyMeterCount = 0;
                frenzyAttackScript.FrenzyModeOff();
                if (currLevel > 0)
                {
                    GameManager.Instance.PlayerScript.SavePlayerData(); // Saves player data if player is past the tutorial scene
                }
                int randLevel = Random.Range(0, roomList.Count); // Looks up random number in the list
                int chosenScene = roomList[randLevel];
                roomList.RemoveAt(randLevel); // Removes the scene from the list - It works!
                currLevel++; // Keeps track of level
                SceneManager.LoadScene(chosenScene); // Loads the scene

                // Increase amount to spawn for next room
                if ((currLevel % 2 == 0) && currLevel > 0) // Currently: amount increases every 2 levels, max is 7 with 8 rooms
                {
                    EnemiesAmountToSpawn++;
                    chestsAmountToSpawn++;
                }
            }
        }
        else if (other.CompareTag("Player") && currLevel == 8) // Wincon: beating 8 rooms
        {
            //GameStats.Instance.frenzyMeterCount = 0;
            frenzyAttackScript.FrenzyModeOff();
            EnemiesAmountToSpawn = 3;
            chestsAmountToSpawn = 0;
            currLevel = 0;
            AddRoomsToList();
            GlobalSettings.Instance.loadPlayer = false;
            GameMusic.ClearInstance();
            SceneManager.LoadScene(12);
        }
    }

    //Scaling Enemies
    public static void ScaleEnemy(GameObject enemy)
    {
        StateController controller = enemy.GetComponent<StateController>();
        controller.stats.hpMax += scaleValueHP; // Scale enemy hp per level
        controller.stats.hp += scaleValueHP;
        controller.attackRateMultiplier += 0.1f * scaleValueAS; // Scale enemy attack speed per level

        // TODO: uncomment and declare scaleValueAD instance variable to enable damage scaling
        //controller.attackDamageMultiplier += 0.1f * scaleValueAD; // Scale enemy attack damage per level
    }
    //Enemy Spawning
    void SpawnEnemies()
    {
        enemiesAmountSpawned = 0;
        if (spawnPoints.Count > 0) // Check if spawnpoints in room
        {
            while (enemiesAmountSpawned < EnemiesAmountToSpawn) // Spawn specific amount of enemies each level
            {
                int randSpot = Random.Range(0, spawnPoints.Count);
                if (spawnPoints[randSpot].monsterData == null)
                {
                    spawnPoints[randSpot].transform.Rotate(new Vector3(0f, 1f, 0f), Random.Range(-90, 91));
                    spawnPoints[randSpot].monsterData = allMonsters[Random.Range(0, 3)];
                    spawnPoints.RemoveAt(randSpot);
                    enemiesAmountSpawned++;
                }
            }
            roomCleared = false; // Sets the room clear condition to false
        }
    }

    public void SpawnEnemiesKeyRoom(int amountOfEnemeiesKeyRoom)
    {
        enemiesAmountSpawned = 0;
        if (spawnPoints.Count > 0) // Check if spawnpoints in room
        {
            while (enemiesAmountSpawned < amountOfEnemeiesKeyRoom) // Spawn specific amount of enemies each level
            {
                int randSpot = Random.Range(0, spawnPoints.Count);
                if (spawnPoints[randSpot].monsterData == null)
                {
                    spawnPoints[randSpot].transform.Rotate(new Vector3(0f, 1f, 0f), Random.Range(-90, 91));
                    spawnPoints[randSpot].monsterData = allMonsters[Random.Range(0, 3)];
                    if(keyRoomEnemiesAlreadySpawned)
                        spawnPoints[randSpot].SpawnMonster();
                    spawnPoints.RemoveAt(randSpot);
                    enemiesAmountSpawned++;
                }
            }
        }
        keyRoomEnemiesAlreadySpawned = true;
    }

    //Chest Spawning
    void SpawnChests()
    {
        chestsAmountSpawned = 0;
        if (chestsList.Count > 0)
        {
            while (chestsAmountSpawned < chestsAmountToSpawn) // Spawn specific amount of chests
            {
                // Depending on the number of room the player is in, spawn different tier chests.
                if (currLevel == 2)
                {
                    int randSpot = Random.Range(0, 4); // Tier 1 
                    if (!chestsList[randSpot % chestsList.Count].activeInHierarchy)
                    {
                        chestsList[randSpot].SetActive(true);
                        chestsAmountSpawned++;
                        chestsList.RemoveAt(randSpot);
                    }
                }
                else if (currLevel < 3)
                {
                    int randSpot = Random.Range(0, 4); // Tier 1 
                    if (!chestsList[randSpot % chestsList.Count].activeInHierarchy)
                    {
                        chestsList[randSpot].SetActive(true);
                        chestsAmountSpawned++;
                        chestsList.RemoveAt(randSpot);
                    }
                }
                else if (currLevel >= 3 && currLevel < 6)
                {
                    int randSpot = Random.Range(4, 8); // Tier 2 
                    if (!chestsList[randSpot % chestsList.Count].activeInHierarchy)
                    {
                        chestsList[randSpot].SetActive(true);
                        chestsAmountSpawned++;
                        chestsList.RemoveAt(randSpot);
                    }
                }
                else if (currLevel >= 6) // Tier 3 
                {
                    for (int i = 8; i < chestsList.Count; i++)
                    {
                        chestsList[i].SetActive(true);
                        chestsAmountSpawned++;
                    }
                }
            }
        }
    }

    void AddRoomsToList()
    {
        for (int i = 4; i < 12; i++)
        {
            roomList.Add(i);
        }
        roomListInstantiated = true;
    }
}