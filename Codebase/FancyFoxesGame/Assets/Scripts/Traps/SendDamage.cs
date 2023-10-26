using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendDamage : MonoBehaviour
{
    float damage;
    GameObject damageSource;
    public List<GameObject> sourcesDamaged;
    bool isTimed = false;

    private void OnEnable()
    {
        sourcesDamaged = new List<GameObject>();
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
    public void SetSource(GameObject source)
    {
        damageSource = source;
    }
    public void Settimed(bool timmed)
    {
        isTimed = timmed;
    }
    public void ResetDamage()
    {
        sourcesDamaged.Clear();
    }

    public void SendDamageTo(Player player)
    {
        if (isTimed || !sourcesDamaged.Contains(damageSource))
        {
            player.TakeDamage(damage, damageSource);
            sourcesDamaged.Add(damageSource);
        }
    }

    public void SendDamageTo(AIController ai)
    {
        if (isTimed || !sourcesDamaged.Contains(damageSource))
        {
            ai.TakeDamage(Mathf.RoundToInt(damage), damageSource);
            sourcesDamaged.Add(damageSource);
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        Player possiblePlayer = other.GetComponent<Player>();
        if (possiblePlayer != null)
        {
            SendDamageTo(possiblePlayer);
            
            return;
        }

        AIController possibleAI = other.GetComponent<AIController>();
        if (possibleAI != null)
        {
            SendDamageTo(possibleAI);
            return;
        }
    }

}
