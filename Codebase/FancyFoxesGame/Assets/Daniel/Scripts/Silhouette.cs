using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Silhouette : MonoBehaviour {
	[SerializeField] private GameObject targetObject;
	private Renderer silhouetteRenderer;

	void Awake() {
		silhouetteRenderer = GetComponent<Renderer>();
	}

	void Update() {
		int relevantLayers = LayerMask.GetMask("Characters", "Projectiles", "Monsters");
		int ignoreLayers = ~LayerMask.GetMask("UI", "Floor", "Pickups", "Ignore Raycast");

		RaycastHit hit;
		Vector3 targetPosition = Camera.main.WorldToScreenPoint(targetObject.transform.position);
		Ray ray = Camera.main.ScreenPointToRay(targetPosition);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreLayers))
			if (0 == ((1 << hit.transform.gameObject.layer) & relevantLayers) && !hit.transform.CompareTag("ShellDomeShield") && !hit.transform.CompareTag("DragonFire") && !hit.transform.CompareTag("RockRain"))
				silhouetteRenderer.enabled = true;
			else
				silhouetteRenderer.enabled = false;
		else
			silhouetteRenderer.enabled = false;
	}
}
