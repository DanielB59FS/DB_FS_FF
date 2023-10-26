using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action {
	public override void Act(StateController controller) {
		Chase(controller);
	}

	private void Chase(StateController controller) {
		NavMeshHit hit;
		bool collided = !controller.navMeshAgent.Raycast(controller.target.position, out hit);

		controller.navMeshAgent.isStopped = collided && hit.distance <= controller.stats.attackRange && controller.navMeshAgent.pathPending;

		if (controller.navMeshAgent.isStopped)
			controller.navMeshAgent.ResetPath();
		else
			controller.navMeshAgent.SetDestination(controller.target.position);
	}
}
