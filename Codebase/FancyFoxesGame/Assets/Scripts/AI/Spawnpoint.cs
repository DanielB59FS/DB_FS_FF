using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject monsterData;
    //public MonsterSO monsterData;
    [SerializeField]
    int spawnCount;
    [SerializeField]
    int spawnRate;

    int currentCount;
    void Start()
    {
        SpawnMonster();

    }

    public void SpawnMonster() //Object pooling spawn is not currently active, will work on next week.
    {
        if (currentCount >= spawnCount)
        {
            CancelInvoke();
        }

        if (monsterData != null)
        {
            GameObject spawn = Instantiate(monsterData, transform.position, transform.rotation) as GameObject;
            spawn.GetComponent<ExtendedStateController>().ScaleDifficulty(LoadLevelScript.ScaleEnemy);
            currentCount++;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(0.5f, 0.5f, 0.5f));
    }
    private void OnDestroy()
    {
        SpawnManager.RemoveSpawner(this.gameObject);
    }
}
