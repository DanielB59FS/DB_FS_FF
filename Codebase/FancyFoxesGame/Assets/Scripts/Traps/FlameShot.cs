using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameShot : ATrap
{

    float time;
    float timeMax;
    bool isDamageOverTime;

    bool isRunning;

    public override void PassDamage()
    {
        damage.SetDamage(info.damage);
        damage.SetSource(this.gameObject);
        damage.Settimed(true);
        time = info.time_empty;
        timeMax = info.time_reload;
        isDamageOverTime = info.isContinueous;
        isRunning = false;

    }

    public override void Disengage(GameObject callback)
    {
        OnEventDisengaged(callback);
    }

    public override void Engage(GameObject callback)
    {
        if (canStartTimer())
        {
            StartTimer();
            OnEventEngaged(callback);
            StartCoroutine(DamageOverTime());
        }
    }
    IEnumerator DamageOverTime()
    {
        float timepassed = 0f;
        isRunning = true;
        while (isRunning)
        {
            foreach (GameObject source in damage.sourcesDamaged)
            {
                Player possiblePlayer = source.GetComponent<Player>();
                if (possiblePlayer != null)
                {
                    damage.SendDamageTo(possiblePlayer);
                }

                AIController possibleAI = source.GetComponent<AIController>();
                if (possibleAI != null)
                {
                    damage.SendDamageTo(possibleAI);
                }
            }
            yield return new WaitForSeconds(time);
            timepassed += time;
            if (timepassed > timeMax)
            {
                isRunning = false;
            }
        }
    }
}
