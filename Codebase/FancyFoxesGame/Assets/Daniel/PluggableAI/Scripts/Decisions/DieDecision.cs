using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Die")]
public class DieDecision : Decision {
	public override bool Decide(StateController controller) {
		return IsDead(controller);
	}

	private bool IsDead(StateController controller) {
		if (controller.stats.hp <= 0)
			return controller.navMeshAgent.isStopped = true;
		return false;
	}
}
