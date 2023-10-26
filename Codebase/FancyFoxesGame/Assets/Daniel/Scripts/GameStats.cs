using System;
using System.Collections;
using System.Collections.Generic;

public class GameStats {
	private static GameStats instance = null;
	public static GameStats Instance {
		get {
			if (null == instance) {
				instance = new GameStats();
				instance.Init();
			}
			return instance;
		}
	}

	/*
	 * TODO:
	 * @Will, remove this static C'tor and check that everything still works on your end.
	 * I believe the issue at the time on your end was caused due to me referencing the wrong static,
	 * I accidently made the variable public and auto referenced it in other scripts instead of using
	 * the static property, so I think now it should work for you without this C'tor.
	 */
	static GameStats() {
		instance = new GameStats();
		instance.Init();
	}


	public float Score { get; private set; }
	public float TimePlayed { get; private set; }
	public int KillCounter { get; private set; }
	public int frenzyMeterCount { get; set; }

	public void Init() {
		Score = TimePlayed = KillCounter = 0;
	}

	// kill method
	public void OnKill(MonsterSO so) {
		OnKill(so.scoreValue);
	}

	public void OnKill(MonsterStats stats) {
		OnKill(stats.scoreValue);
	}

	public void OnKill(float bonusScore) {
		//if (null != stats)
		//	Score += stats.scoreValue;
		//++Score;
		//++KillCounter;
		//++frenzyMeterCount;
		if (0 < bonusScore)
			Score += bonusScore;
		++Score;
		++KillCounter;
		++frenzyMeterCount;
	}

	public void IncrementTime(float seconds) {
		TimePlayed += seconds;
	}
}
