using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Work in progress for alternative and possibly easier to maintain, and more accurate cursor follow script
/// </summary>
public class CursorPosition : MonoBehaviour {

	public static readonly float unitCubeMagnitude = 1.732051f;

	private Vector3 mousePoint;
	private Ray mouseRay;
	private RaycastHit rayHit;
	public Vector3 MousePoint { get => mousePoint; }

	// Update is called once per frame
	void Update() {
		if (Camera.main.orthographic) {
			// Orthographic
			mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
		else {
			// Perspective
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(mouseRay, out rayHit, Mathf.Infinity, LayerMask.GetMask("Floor"))) {
				mousePoint = rayHit.point;
				// TODO: Attempt to address when Raycast hits player. (research)
			}
		}

		transform.localScale = GameManager.Instance.PlayerObject.transform.localScale;
		transform.position = mousePoint - mouseRay.direction.normalized * (transform.localScale.magnitude / (2 * unitCubeMagnitude));
	}
}
