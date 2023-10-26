using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script logic for extracting and presenting AI health into world space UI
/// </summary>
public class HealthUpdateAI : MonoBehaviour {

	[SerializeField]
	private GameObject parent;
	private AIController controller;
	private MonsterStats stats;
	private float maxHealth;
	private float currentHealth;

	private Image healthBar;

	// Start is called before the first frame update
	void Start() {
		stats = parent.GetComponent<StateController>().stats;
		healthBar = GetComponent<Image>();
		currentHealth = maxHealth = stats.hpMax;
	}

	// Update is called once per frame
	void Update() {
		healthBar.fillAmount = (currentHealth = stats.hp) / maxHealth;
	}
}
