using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/PlayerAlive")]
public class PlayerAliveDecision : Decision {
	public override bool Decide(StateController controller) {
		return IsPlayerAlive(controller);
	}

	private bool IsPlayerAlive(StateController controller) => 0 < GameManager.Instance.PlayerScript.healthValue;
}
