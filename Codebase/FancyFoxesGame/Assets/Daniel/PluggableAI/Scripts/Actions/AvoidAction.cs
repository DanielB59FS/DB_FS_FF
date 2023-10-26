using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Avoid")]
public class AvoidAction : Action {
	public float avoidDistanceRatio = 0.95f;

	public override void Act(StateController controller) {
		Avoid(controller);
	}

	private void Avoid(StateController controller) {
		NavMeshHit hit;
		bool collided = controller.navMeshAgent.Raycast(controller.target.position, out hit);

		controller.navMeshAgent.isStopped = (collided || (hit.distance >= controller.stats.lookRange * avoidDistanceRatio)) && !controller.navMeshAgent.pathPending;

		if (controller.navMeshAgent.isStopped)
			controller.navMeshAgent.ResetPath();
		else
			controller.navMeshAgent.SetDestination(controller.transform.position + (controller.transform.position - controller.target.position));
	}
}
