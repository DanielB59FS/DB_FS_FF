using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : ScriptableObject {
	public bool flipDecision = false;

	public abstract bool Decide(StateController controller);

	public bool Result(StateController controller) {
		bool result = Decide(controller);
		return flipDecision ? !result : result;
	}
}
