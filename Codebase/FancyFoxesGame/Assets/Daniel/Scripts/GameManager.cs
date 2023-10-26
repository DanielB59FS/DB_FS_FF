using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpawnManager))]
public partial class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; } = null;

	[SerializeField]
	private int frameRate = 30;

	[SerializeField]
	private GameObject playerObject = null;
	public GameObject PlayerObject { get => playerObject; private set => playerObject = value; }

	[SerializeField]
	private Player playerScript = null;
	public Player PlayerScript { get => playerScript; private set => playerScript = value; }

	public GameObject hud = null;

	public GameObject menuObject = null;
	public MenuController menuScript = null;

	public bool isPaused = false;
	public bool gameOver = false;
	[HideInInspector]
	public bool ignoreInput = false;

	public List<GameObject> Enemies;
}

public partial class GameManager : MonoBehaviour {
	public void Init() {

		// get the players reference
		if (null == PlayerObject) {
			PlayerObject = GameObject.FindGameObjectWithTag("Player");
			PlayerScript = PlayerObject.GetComponent<Player>();
		}

		// get HUD reference
		if (null == hud)
			hud = GameObject.FindGameObjectWithTag("HudObject");

		// get menu references
		if (null == menuObject) {
			menuObject = GameObject.FindGameObjectWithTag("MenuObject");
			menuScript = GameObject.FindGameObjectWithTag("MenuScript").GetComponent<MenuController>();
			menuObject.SetActive(false);
		}

		GlobalSettings.SetFrameRate(frameRate);
	}
}

public partial class GameManager : MonoBehaviour {

	void Awake() {
		if (null == Instance) {
			Instance = this;
			Instance.Init();
		}
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Cancel") && !gameOver && !ignoreInput) OnCancel();
		if (!isPaused) GameStats.Instance.IncrementTime(Time.deltaTime);
	}

	// Enforcing menu on game over (with delay so animations can complete)
	public IEnumerator OnGameOver(float delay, string title = "Game Over", bool isWin = false) {
		gameOver = true;
		yield return new WaitForSeconds(delay);
		OnCancel();
		menuScript.OnGameOver(title, isWin);
	}

	// Logic & toggle on/off of game pause & menu
	public void OnCancel() {
		AudioListener.pause = isPaused = !isPaused;

		if (isPaused) {
			Time.timeScale = 0f;
			menuObject.SetActive(true);

			menuScript.SelectDefaultButton(true);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else {
			if (FollowPlayer.Instance.FollowRotation) {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			Invoke("ResetMenu", 0f);
			Time.timeScale = 1f;
		}
	}

	private void ResetMenu() {
		menuScript.OnButtonCancelQuit(false);
		Invoke("DisableMenu", 0f);
	}
	private void DisableMenu() {
		menuObject.SetActive(false);
	}
}
