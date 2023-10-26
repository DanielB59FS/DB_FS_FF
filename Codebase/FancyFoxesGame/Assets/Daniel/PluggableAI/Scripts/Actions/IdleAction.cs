using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Idle")]
public class IdleAction : Action {
	public override void Act(StateController controller) {
		Idle(controller);
	}

	private void Idle(StateController controller) {
		controller.target = null;
		controller.navMeshAgent.ResetPath();
		controller.navMeshAgent.isStopped = true;
	}
}
