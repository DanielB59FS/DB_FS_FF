using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private static List<GameObject> spawners = new List<GameObject>(); //spawners will add themselves to this list.
    void Start()
    {
        SpawnEnemies();
    }

    public static void AddSpawner(GameObject spawner) //Spawners will use this to add themselves to the list.
    {
        spawners.Add(spawner);
    }
    public static void RemoveSpawner(GameObject spawner) //This will be called by spawners ondestroy for clean-up.
    {
        if (spawners.Contains(spawner))
        {
            spawners.Remove(spawner);
        }
    }
    void SpawnEnemies()
    {
       
    }
}

