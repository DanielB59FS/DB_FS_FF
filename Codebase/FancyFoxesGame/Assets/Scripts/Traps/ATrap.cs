using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ATrap : MonoBehaviour
{

    public TriggerPlates trigger;
    public SendDamage damage;
    public TrapInfo info;
    public UnityEvent<GameObject> onEventTriggered;
    public UnityEvent<GameObject> onEventUntriggered;

    public float runTime = 1f;
    private float timeLeft = 0f;
    private bool running = false;

    void OnEnable()
    {
        if (trigger == null)
        {
            return;
        }
        trigger.AddEngageListener((ATrap)this);
    }
    void OnDisable()
    {
        trigger.RemoveEngageListener((ATrap)this);
    }
    public void OnEventEngaged(GameObject callback)
    {
        onEventTriggered.Invoke(callback);
    }
    public void OnEventDisengaged(GameObject callback)
    {
        onEventUntriggered.Invoke(callback);
    }
    public bool canStartTimer()
    {
        return timeLeft == 0f;
    }

    public void StartTimer()
    {
        running = true;
        timeLeft = runTime;
    }
    public void ReStartTimer()
    {
        timeLeft = runTime;
        running = false;
    }
    public void ResetTrap()
    {
        ReStartTimer();
        OnEventDisengaged(this.gameObject);
    }


    private void Update()
    {
        if (timeLeft > 0f) timeLeft -= Time.deltaTime;
        else if (timeLeft < 0f) timeLeft = 0f;
        else if (running && timeLeft == 0f) {
            Disengage(null);
            running = false;
        }
    }

    public abstract void Engage(GameObject callback);
    public abstract void Disengage(GameObject callback);
    public abstract void PassDamage();
    
}
