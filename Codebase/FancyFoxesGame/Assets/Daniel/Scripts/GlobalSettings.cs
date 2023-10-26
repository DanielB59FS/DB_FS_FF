using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings {
	private static GlobalSettings instance = null;
	public static GlobalSettings Instance {
		get {
			if (null == instance) {
				instance = new GlobalSettings();
				instance.Init();
			}
			return instance;
		}
	}

	public void Init() {
		if (PlayerPrefs.HasKey("FollowMode")) followRotation = 1 == PlayerPrefs.GetInt("FollowMode");
		if (PlayerPrefs.HasKey("NavMode")) cartesian = 1 == PlayerPrefs.GetInt("NavMode");
	}

	public static void SetFrameRate(int value) {
		// I don't know if the condition is necessary, is just in case this process includes overhead.
		if (value != Application.targetFrameRate) Application.targetFrameRate = value;
	}

	/*
	 * TODO:
	 * @Will, remove this static C'tor and check that everything still works on your end.
	 * I believe the issue at the time on your end was caused due to me referencing the wrong static,
	 * I accidently made the variable public and auto referenced it in other scripts instead of using
	 * the static property, so I think now it should work for you without this C'tor.
	 */
	static GlobalSettings() {
		instance = new GlobalSettings();
		instance.Init();
	}

	public bool followRotation = false;
	public bool cartesian = true;

	public bool loadPlayer = false;

	public float playerHealth;
	public bool playerHasItem;
	public ElementalAbility playerAbility;
	public CreatureAbility creatureAbility;
	public int shotsRemaining;
}
