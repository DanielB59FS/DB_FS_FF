using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision {
	public override bool Decide(StateController controller) {
		return Look(controller);
	}

	private bool Look(StateController controller) {
		return Vector3.Angle(controller.transform.forward, controller.target.transform.position - controller.transform.position) <= 10f;
	}
}
