using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyPuzzle : MonoBehaviour
{

    public List<TriggerPlates> triggers;
    public List<KeySpawnLocation> keySpawnLocations; // Store positions for randomizing

    public UnityEvent<GameObject> onEventTriggered;
    public UnityEvent<GameObject> onEventUntriggered;


    void OnEnable()
    {
        if (triggers == null)
        {
            return;
        }

        foreach (TriggerPlates plate in triggers)
        {   
            plate.AddEngageListener((KeyPuzzle)this);
            int randSpot = Random.Range(0, keySpawnLocations.Count);
            plate.gameObject.transform.position = new Vector3(keySpawnLocations[randSpot].transform.position.x,
                keySpawnLocations[randSpot].transform.position.y, keySpawnLocations[randSpot].transform.position.z);
            keySpawnLocations.RemoveAt(randSpot);
        }
    }
    void OnDisable()
    {
        foreach (TriggerPlates plate in triggers)
        {
            plate.RemoveEngageListener((KeyPuzzle)this);

        }
    }
    public void OnEventEngaged(GameObject callback)
    {
        foreach (TriggerPlates plate in triggers)
        {
            if (!plate.wasPressed)
            {
                return;
            }
        }
        onEventTriggered.Invoke(callback);
    }
    public void OnEventDisengaged(GameObject callback)
    {
        onEventUntriggered.Invoke(callback);
    }
    private void Update()
    {
        foreach (TriggerPlates plate in triggers)
        {
            if (!plate.wasPressed)
            {
                OnEventDisengaged(this.gameObject);
                return;
            }
        }
        onEventTriggered.Invoke(this.gameObject);

    }
    public void ResetPuzzle()
    {
        foreach (TriggerPlates plate in triggers)
        {
            plate.ResetTrigger();
        }
    }
}