using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/MonsterStats")]
public class MonsterStats : ScriptableObject {

	public float lookRange = 15;
	public float lookSphereCastRadius = 0.5f;

	public float attackRange = 20;
	public float attackRate = 1;
	public float aggroRange = 15;

	// is handled by the Ability
	//public float attackForce = 15;
	//public int attackDamage = 50;

	public float hpMax = 35;
	public float hp = 35;
	public int scoreValue = 1;

	#region Larry's old AI parameters
	//public GameObject powerupPickup;
	//public GameObject model;
	//public enum Temperament { Hostile, Tactical, Skittish }
	//public int hpMin, mana, stamina, range, teamwork, bravery, intelligence;
	#endregion
}
