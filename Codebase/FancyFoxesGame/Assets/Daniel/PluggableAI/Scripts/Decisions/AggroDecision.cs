using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Aggro")]
public class AggroDecision : Decision {
	public override bool Decide(StateController controller) {
		return InAggroRange(controller);
	}

	private bool InAggroRange(StateController controller) {
		bool isInRange = Vector3.Distance(controller.transform.position, GameManager.Instance.PlayerObject.transform.position) <= controller.stats.aggroRange;

		controller.target = isInRange ? GameManager.Instance.PlayerObject.transform : controller.target;

		return isInRange;
	}
}
