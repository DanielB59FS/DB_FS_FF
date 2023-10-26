using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script logic for extracting and presenting Player health into world space UI
/// </summary>
public class HealthUpdate : MonoBehaviour {

	private Image healthBar;

	// Start is called before the first frame update
	void Start() {
		healthBar = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update() {
		healthBar.fillAmount = GameManager.Instance.PlayerScript.healthValue / GameManager.Instance.PlayerScript.healthMax;
	}
}
