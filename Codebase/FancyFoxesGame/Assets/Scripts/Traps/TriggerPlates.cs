using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPlates : MonoBehaviour
{

    public UnityEvent<GameObject> onTriggerEnterEvents;
    public UnityEvent<GameObject> onTriggerStayEvents;
    public UnityEvent<GameObject> onTriggerExitEvents;
    public LayerMask interactableLayer;
    public bool isPressed;
    public bool wasPressed;
    public float cooldownTime = 3f;

    private List<ATrap> onEngage = new List<ATrap>();
    private List<KeyPuzzle> onPuzzleComplete = new List<KeyPuzzle>();
    private bool running = false;

    private void Start()
    {
        running = true;
        StartCoroutine(StartAfter());
    }
    IEnumerator StartAfter()
    {
        yield return new WaitForSeconds(cooldownTime);
        ResetTrigger();
    }

    public void EngageTrigger(GameObject callback)
    {
        if (!running)
        {

            running = true;
            if (onEngage.Count > 0)
            {
                for (int i = onEngage.Count - 1; i >= 0; i--)
                {
                    onEngage[i].OnEventEngaged(callback);
                }
            }
            if (onPuzzleComplete.Count > 0)
            {
                for (int i = onPuzzleComplete.Count - 1; i >= 0; i--)
                {
                    onPuzzleComplete[i].OnEventEngaged(callback);

                }
            }
            StartCoroutine(ResetAfterCooldown(cooldownTime));

        }
    }

    public void ResetTrigger()
    {
        isPressed = false;
        wasPressed = false;
        running = false;
        onTriggerExitEvents.Invoke(null);
        foreach (ATrap trap in onEngage)
        {
            trap.ResetTrap();
        }
    }

    IEnumerator ResetAfterCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        ResetTrigger();
    }

    public void AddEngageListener(ATrap trap)
    {
        onEngage.Add(trap);
    }
    public void RemoveEngageListener(ATrap trap)
    {
        onEngage.Remove(trap);
    }
    public void AddEngageListener(KeyPuzzle puzzle)
    {
        onPuzzleComplete.Add(puzzle);
    }
    public void RemoveEngageListener(KeyPuzzle puzzle)
    {
        onPuzzleComplete.Remove(puzzle);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //interactableLayer.value == (1 << other.gameObject.layer) && !running
        {
            isPressed = true;
            wasPressed = true;
            onTriggerEnterEvents.Invoke(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
     
    }
    private void OnTriggerExit(Collider other)
    {
        if (interactableLayer.value == other.gameObject.layer)
        {
            isPressed = false;
            //onTriggerExit.Invoke(other.gameObject);
        }
    }
}
