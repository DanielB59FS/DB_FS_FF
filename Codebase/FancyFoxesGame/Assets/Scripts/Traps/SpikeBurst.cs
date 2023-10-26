using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBurst : ATrap
{
    public override void PassDamage()
    {
        damage.SetDamage(info.damage);
        damage.SetSource(this.gameObject);
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
        }
    }
}
