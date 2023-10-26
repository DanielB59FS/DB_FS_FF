using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Flock")]
public class FlockAction : Action {
	public float neighborDistance = 15f;
	public float avoidDistance = 6f;

	public override void Act(StateController controller) {
		Flock(controller);
	}

	private void Flock(StateController controller) {
		Vector3 vCenter = Vector3.zero, vAvoid = Vector3.zero;
		uint neighborCount = 0;
		float currentDistance;

		foreach (GameObject enemy in GameManager.Instance.Enemies)
			if (enemy != controller.gameObject) {
				currentDistance = Vector3.Distance(enemy.transform.position, controller.transform.position);

				if (currentDistance <= neighborDistance) {
					++neighborCount;
					vCenter += enemy.transform.position;

					if (currentDistance <= avoidDistance)
						vAvoid += (controller.transform.position - enemy.transform.position);
				}
			}

		if (0 < neighborCount) {
			vCenter = (vCenter + controller.target.position) / (neighborCount + 1);
			Vector3 vFinal = vCenter + (vAvoid - vCenter);
			if (!controller.navMeshAgent.pathPending && 0 < controller.stats.hp && Vector3.zero != vFinal) {
				controller.transform.position += vFinal.normalized * Time.deltaTime * controller.navMeshAgent.speed;
			}
		}
	}
}
