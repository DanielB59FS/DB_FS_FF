using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Die")]
public class DieAction : Action {

	[SerializeField] private GameObject smokePuff;

	public override void Act(StateController controller) {
		Die(controller);
	}

	private void Die(StateController controller) {
		foreach (Collider collider in controller.GetComponentsInChildren<Collider>())
			collider.enabled = false;

		if (controller.anim.GetCurrentAnimatorStateInfo(0).IsName("Mage_Death")) {

			if (controller.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > controller.anim.GetCurrentAnimatorStateInfo(0).length) //If the animation has been playing longer than it's duration than exit this state, its done
			{
				var puff = Instantiate(smokePuff, controller.transform.position, controller.transform.rotation);
				Destroy(puff.gameObject, 0.5f);
				Destroy(controller.gameObject);
			}
		}
	}
}
