using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster")]
public class MonsterSO : ScriptableObject
{
    public GameObject model, powerupPickup;
    public enum Temperament {Hostile, Tactical, Skittish}
    // List<Ability> abilities = new List<Ability>();
    public int hpMax, hpMin, mana, stamina, range, teamwork, bravery, intelligence, scoreValue;

}
