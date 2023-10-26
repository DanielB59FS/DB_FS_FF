using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class PluggableState : ScriptableObject {

	public enum Phase { Enter, Update, Exit };

	public Action[] actionsEnter;
	public Action[] actionsUpdate;
	public Action[] actionsExit;
	public Transition[] transitions;
	public Color sceneGizmoColor = Color.grey;

	public void ProcessState(StateController controller) {
		switch (controller.CurrentPhase) {
			case Phase.Enter:
				EnterState(controller);
				controller.CurrentPhase = Phase.Update;
				break;
			case Phase.Update:
				UpdateState(controller);
				break;
			case Phase.Exit:
				ExitState(controller);
				controller.CurrentPhase = Phase.Enter;
				break;
		}
	}

	private void EnterState(StateController controller) {
		DoActions(controller, actionsEnter);
	}

	private void UpdateState(StateController controller) {
		DoActions(controller, actionsUpdate);
		CheckTransitions(controller);
	}

	private void ExitState(StateController controller) {
		DoActions(controller, actionsExit);
	}

	private void DoActions(StateController controller, Action[] actions) {
		foreach (Action action in actions) action.Act(controller);
	}

	private void CheckTransitions(StateController controller) {
		bool toExit = false;

		foreach (Transition transition in transitions) {
			if (transition.muteTransition) continue;
			if (transition.ProcessDecisions(controller))
				toExit = controller.TransitionToState(transition.trueState);
			else
				toExit = controller.TransitionToState(transition.falseState);
			if (toExit) {
				controller.CurrentPhase = Phase.Exit;
				break;
			}
		}
	}
}
