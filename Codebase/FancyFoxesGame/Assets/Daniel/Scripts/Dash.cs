using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour {

	private Image staminaBar;
	private float timeElapsed;

	public float dashSpeedMultiplier = 1.45f;

	// Start is called before the first frame update
	void Start() {
		staminaBar = GetComponent<Image>();
		timeElapsed = GameManager.Instance.PlayerScript.staminaRestoreDelay;
	}

	// Update is called once per frame
	void Update() {
		if (!GameManager.Instance.isPaused && 0 < GameManager.Instance.PlayerScript.healthValue) {
			if (Input.GetButton("Dash") && 0 != GameManager.Instance.PlayerObject.GetComponent<CharacterController>().velocity.magnitude) {
				timeElapsed = 0;
				if (0 < GameManager.Instance.PlayerScript.stamina) {
					GameManager.Instance.PlayerScript.speedMultiplier = dashSpeedMultiplier;
					GameManager.Instance.PlayerScript.anim.speed = dashSpeedMultiplier;
					if (0 < GameManager.Instance.PlayerScript.stamina) GameManager.Instance.PlayerScript.stamina -= GameManager.Instance.PlayerScript.staminaExhuastRate * Time.deltaTime;
					else
						GameManager.Instance.PlayerScript.stamina = 0;
				}
				else {
					GameManager.Instance.PlayerScript.speedMultiplier = 1f;
					GameManager.Instance.PlayerScript.anim.speed = 1f;
					GameManager.Instance.PlayerScript.stamina = 0;
				}
			}
			else {
				GameManager.Instance.PlayerScript.speedMultiplier = 1f;
				GameManager.Instance.PlayerScript.anim.speed = 1f;
			}


			if (timeElapsed < GameManager.Instance.PlayerScript.staminaRestoreDelay)
				timeElapsed += Time.deltaTime;

			if (timeElapsed >= GameManager.Instance.PlayerScript.staminaRestoreDelay) {
				if (GameManager.Instance.PlayerScript.stamina < GameManager.Instance.PlayerScript.staminaMax)
					GameManager.Instance.PlayerScript.stamina += GameManager.Instance.PlayerScript.staminaRestoreRate * Time.deltaTime;
				if (GameManager.Instance.PlayerScript.staminaMax <= GameManager.Instance.PlayerScript.stamina)
					GameManager.Instance.PlayerScript.stamina = GameManager.Instance.PlayerScript.staminaMax;
			}

			staminaBar.fillAmount = GameManager.Instance.PlayerScript.stamina / GameManager.Instance.PlayerScript.staminaMax;
		}
	}
}
