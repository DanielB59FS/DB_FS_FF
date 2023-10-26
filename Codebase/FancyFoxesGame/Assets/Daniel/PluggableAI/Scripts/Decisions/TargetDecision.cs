using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Target")]
public class TargetDecision : Decision {
	public override bool Decide(StateController controller) {
		return HasTarget(controller);
	}

	private bool HasTarget(StateController controller) => !(null == controller.target || default == controller.target);
}
