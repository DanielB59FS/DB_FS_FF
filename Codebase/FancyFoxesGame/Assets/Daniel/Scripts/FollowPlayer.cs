using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic script for camera to follow around it's target (the player) in world space with various options
/// </summary>
public class FollowPlayer : MonoBehaviour {

	private static FollowPlayer instance = null;
	public static FollowPlayer Instance { get => instance; private set => instance = value; }

	[SerializeField]
	private Vector3 positionOffset = new Vector3(0, 20, -13);

	[SerializeField]
	private Vector3 rotationOffset = new Vector3(60, 0, 0);

	[SerializeField]
	private float fieldOfView = 100f;

	[SerializeField]
	private bool followRotation;
	public bool FollowRotation { get => followRotation; set => followRotation = value; }

	void Awake() {
		if (null == instance) {
			instance = this;
			transform.rotation = Quaternion.Euler(rotationOffset);
			GetComponent<Camera>().fieldOfView = fieldOfView;
			FollowRotation = GlobalSettings.Instance.followRotation;
		}
	}

	/// <summary>
	/// Only follows when game isn't paused.
	/// While rotational follow is enabled, the mouse cursor is locked and not visible.
	/// </summary>
	// Update is called once per frame
	void LateUpdate() {
		if (!GameManager.Instance.isPaused) {
			if (followRotation) {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				transform.position = GameManager.Instance.PlayerObject.transform.position + GameManager.Instance.PlayerObject.transform.TransformDirection(positionOffset);
				transform.rotation = GameManager.Instance.PlayerObject.transform.rotation * Quaternion.Euler(rotationOffset);
			}
			else {
				transform.position = GameManager.Instance.PlayerObject.transform.position + positionOffset;
				transform.rotation = Quaternion.Euler(rotationOffset);
			}
		}
	}
}
