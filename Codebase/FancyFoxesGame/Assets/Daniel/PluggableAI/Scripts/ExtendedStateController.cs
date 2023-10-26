using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExtendedStateController : StateController {

	// TODO: suitable temporary fix, not planning on changing in near future
	public bool killable = true;

	#region Larry's code/logic
	Color origionalColor;
	public AnimationClip damageClip;
	#endregion

	[Header("Object Specific Drops:")]
	[SerializeField] private GameObject[] drop;
	[SerializeField][Range(0, 1)] private float dropRate;

	protected override void Update() {
		base.Update();

		#region Larry's code/logic
		float orientation = Vector3.Angle(navMeshAgent.velocity.normalized, navMeshAgent.transform.forward) > 90f ? -1 : 1;
		anim.SetFloat("Speed", navMeshAgent.velocity.normalized.magnitude * orientation);
		anim.SetFloat("SpeedMagnitude", navMeshAgent.velocity.normalized.magnitude);
		#endregion
	}
	protected override void Start() {
		base.Start();
		#region temporary fix
		GetComponent<Rigidbody>().drag = Mathf.Infinity;
		GetComponent<Rigidbody>().angularDrag = Mathf.Infinity;
		#endregion
		#region Larry's code/logic
		origionalColor = renderer.material.color;
		#endregion
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Ability"))
			target = GameManager.Instance.PlayerObject.transform;

		if (collision.gameObject.GetComponent<ElementalAbilityAttack>() is ElementalAbilityAttack attack)
			TakeDamage(attack.ability.abilityDamage);
	}

	public void ScaleDifficulty(UnityAction<GameObject> action) {
		action(gameObject);
	}
	
	// TODO: contains suitable temporary fix, not planning on changing in near future
	public void TakeDamage(float damage) {
		if (0 < stats.hp && 0 < damage) {
			stats.hp -= damage;
			anim.SetTrigger("Damage");
			if (stats.hp <= 0) {
				OnDeath(killable);
				if (!killable) {
					stats.hp = stats.hpMax;
					++GameStats.Instance.frenzyMeterCount;
					anim.SetBool("Death", false);
				}
			}
			FlashRed();
		}
	}

	#region Larry's code/logic
	void OnDestroy() {
		if (GameManager.Instance.Enemies.Contains(gameObject))
			GameManager.Instance.Enemies.Remove(gameObject);
	}

	void OnDeath() {
		OnDeath(true);
	}

	void OnDeath(bool countScore) {
		stats.hp = 0;
		if ((Random.Range(0f, 1f) < dropRate || 1f == dropRate) && 0 < drop.Length && killable)
			Instantiate(drop[Random.Range(0, drop.Length)], transform.position, transform.rotation);
		if (countScore) GameStats.Instance.OnKill(stats);
		anim.SetBool("Death", true);
	}

	void FlashRed() {
		renderer.material.color = Color.red;
		Invoke("ResetColor", damageClip.averageDuration);
	}

	void ResetColor() {
		renderer.material.color = origionalColor;
	}
	#endregion
}
