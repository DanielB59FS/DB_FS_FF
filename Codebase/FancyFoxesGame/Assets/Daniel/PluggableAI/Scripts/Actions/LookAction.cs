using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Look")]
public class LookAction : Action {
	public override void Act(StateController controller) {
		Look(controller);
	}

	private void Look(StateController controller) {
		Vector3 direction = (controller.target.position - controller.transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		//controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * controller.navMeshAgent.angularSpeed);
		Vector3 v = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * controller.navMeshAgent.angularSpeed).eulerAngles;
		v.x = v.z = 0f;
		controller.transform.rotation = Quaternion.Euler(v);

		controller.navMeshAgent.isStopped = controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending;
	}
}
