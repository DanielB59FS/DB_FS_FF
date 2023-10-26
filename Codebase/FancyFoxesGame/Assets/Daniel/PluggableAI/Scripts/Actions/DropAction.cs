using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Drop")]
public class DropAction : Action {

	[SerializeField] private GameObject[] drop;
	[SerializeField][Range(0, 1)] private float rate;

	public override void Act(StateController controller) {
		Drop(controller);
	}

	private void Drop(StateController controller)
	{
		if ((Random.Range(0f, 1f) < rate || 1f == rate) && 0 < drop.Length)
		{
			if (LoadLevelScript.currLevel < 3)
				Instantiate(drop[Random.Range(0, 2)], controller.transform.position, controller.transform.rotation);

			else if (LoadLevelScript.currLevel >= 3 && LoadLevelScript.currLevel < 6)
				Instantiate(drop[Random.Range(2, 5)], controller.transform.position, controller.transform.rotation);

			else if (LoadLevelScript.currLevel >= 6)
				Instantiate(drop[Random.Range(5, 8)], controller.transform.position, controller.transform.rotation);
		}
	}
}
