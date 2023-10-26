using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TrapInfo", menuName = "Traps/TrapInfo")]
public class TrapInfo : ScriptableObject
{
    public float speed_start;
    public float speed_stop;
    public bool isContinueous;
    public float time_empty;
    public float time_reload;
    public int ammo;
    public int damage;
}
