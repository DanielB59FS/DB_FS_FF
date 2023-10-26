using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition {
	public Decision[] decisions;
	[Tooltip("True->And, False->Or")]
	public bool andOrProcess;
	public bool muteTransition;
	public PluggableState trueState;
	public PluggableState falseState;

	public bool ProcessDecisions(StateController controller) {
		bool result = andOrProcess;
		
		foreach (Decision decision in decisions) {
			if (andOrProcess)
				result = result && decision.Result(controller);
			else
				result = result || decision.Result(controller);

			if ((andOrProcess && !result) || (!andOrProcess && result)) break;
		}

		return result;
	}
}
