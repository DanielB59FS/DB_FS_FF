using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IsVisible")]
public class TargetVisibleDecision : Decision {
	public override bool Decide(StateController controller) {
		return IsTargetVisible(controller);
	}

	private bool IsTargetVisible(StateController controller) {
		NavMeshHit hit;
		return !controller.navMeshAgent.Raycast(controller.target.position, out hit);
	}
}
