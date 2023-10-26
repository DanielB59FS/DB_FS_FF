using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Initialize")]
public class InitializeAction : Action {
	public override void Act(StateController controller) {
		Initialize(controller);
	}

	private void Initialize(StateController controller) {
		GameManager.Instance.Enemies.Add(controller.gameObject);
	}
}
