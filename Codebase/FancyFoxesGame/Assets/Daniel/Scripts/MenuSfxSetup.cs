using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSfxSetup : MonoBehaviour {
	// Start is called before the first frame update
	void Awake() {
		GetComponent<AudioSource>().ignoreListenerPause = true;
	}

	void OnEnable() {
		if (GetComponent<Slider>() is Slider slider) {
			if (PlayerPrefs.HasKey(slider.name))
				slider.value = PlayerPrefs.GetFloat(slider.name);
			else
				PlayerPrefs.SetFloat(slider.name, slider.value);
		}
	}

	void OnDisable() {
		if (GetComponent<Slider>() is Slider slider)
			PlayerPrefs.SetFloat(slider.name, slider.value);
	}
}
