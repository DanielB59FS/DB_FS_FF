using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueUpdate : MonoBehaviour {

	private enum ValueType { Score, Kill, Time };

	[SerializeField]
	private ValueType type;

	void OnEnable() {
		switch (type) {
			case ValueType.Score:
				UpdateScore();
				break;
			case ValueType.Kill:
				UpdateKillCount();
				break;
			case ValueType.Time:
				UpdateTimePlayed();
				break;
		}
	}

	private void UpdateScore() => GetComponent<TextMeshProUGUI>().text = "Score: " + GameStats.Instance.Score;

	private void UpdateKillCount() => GetComponent<TextMeshProUGUI>().text = "KillCount: " + GameStats.Instance.KillCounter;

	private void UpdateTimePlayed() {
		float totalSeconds = GameStats.Instance.TimePlayed;
		float hours = Mathf.Floor(totalSeconds / 3600);
		float minutes = Mathf.Floor((totalSeconds -= hours * 3600) / 60);
		float seconds = Mathf.Floor(totalSeconds -= minutes * 60);
		GetComponent<TextMeshProUGUI>().text = $"Time Played:\n{hours} Hours\n{minutes} Minutes\n{seconds} Seconds";
	}
}
