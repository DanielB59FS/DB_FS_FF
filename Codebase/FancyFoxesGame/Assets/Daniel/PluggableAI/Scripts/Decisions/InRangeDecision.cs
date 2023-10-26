using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/InRange")]
public class InRangeDecision : Decision {
	public override bool Decide(StateController controller) {
		return InRange(controller);
	}

	private bool InRange(StateController controller) {
		return Vector3.Distance(controller.transform.position, controller.target.transform.position) <= controller.stats.attackRange;
	}
}
