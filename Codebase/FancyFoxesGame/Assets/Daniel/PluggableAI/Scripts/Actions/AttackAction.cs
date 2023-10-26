using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action {
	public override void Act(StateController controller) {
		Attack(controller);
	}

	private void Attack(StateController controller) {
		int relevantLayers = LayerMask.GetMask("Miscellaneous", "Environment", "Characters", "Monsters");

		RaycastHit hit;
		//if (Physics.SphereCast(controller.eyes.position, controller.stats.lookSphereCastRadius, controller.transform.forward, out hit, controller.stats.attackRange, relevantLayers)) {
		if (Physics.Raycast(controller.eyes.position, controller.transform.forward, out hit, controller.stats.attackRange, relevantLayers)) {
			if ((hit.transform.CompareTag("Player") || hit.transform.CompareTag("ShellDomeShield")) && controller.StateElapsedDelta(controller.stats.attackRate / controller.attackRateMultiplier)) {
				if (0 == Random.Range(0, 2))
					controller.anim.SetTrigger("Fire1");
				else
					controller.anim.SetTrigger("Fire2");
				controller.useAbility.AlternateUseAbility(controller.eyes.gameObject, null);
			}
		}
	}
}
