using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFrameRate : MonoBehaviour {
	public int frameRate = 30;

	private void Awake() {
		GlobalSettings.SetFrameRate(frameRate);
	}
}
