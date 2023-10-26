using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRoomEnemySpawner : MonoBehaviour
{

    private float spawnTimer;
    [SerializeField]
    private float secondsBetweenWaves;
    [SerializeField]
    private int amountOfEnemiesToSpawn;
    
    LoadLevelScript loadLevelScript;

    // Start is called before the first frame update
    void Start()
    {
        loadLevelScript = GameObject.FindGameObjectWithTag("SceneTriggerCube").GetComponent<LoadLevelScript>();
        spawnTimer = secondsBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0f)
        {
            loadLevelScript.SpawnEnemiesKeyRoom(amountOfEnemiesToSpawn);
            spawnTimer = secondsBetweenWaves;
        }
    }
}
