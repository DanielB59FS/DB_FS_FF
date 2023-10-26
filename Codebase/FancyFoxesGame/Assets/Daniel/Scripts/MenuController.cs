#define WIP

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class MenuController : MonoBehaviour {

	/// <summary>
	/// Logic for swapping currently active menu panels.
	/// the property either retrieves current active panel
	/// or disables current panel and sets a new currently active panel
	/// </summary>
	private GameObject currentContext = null;
	public GameObject CurrentContext {
		get => currentContext;
		set {
			if (currentContext) currentContext.SetActive(false);
			if (value) value.SetActive(true);
			currentContext = value;
		}
	}

	public GameObject panelStats;
	public TextMeshProUGUI panelStatsTitle;

	public GameObject panelOptions;

	[SerializeField]
	private Button resumeButton = null;
	[SerializeField]
	private Button restartButton = null;
	[SerializeField]
	private Button noButton = null;
	[SerializeField]
	private Toggle followModeToggle = null;
	[SerializeField]
	private Toggle navModeToggle = null;
	[SerializeField]
	private Image loadingBar = null;

	public Button defaultButton = null;

	[SerializeField]
	private AudioMixer sfxMixer = null;
	[SerializeField]
	private AudioMixer ambientMixer = null;
}

public partial class MenuController : MonoBehaviour {
	public enum QuitMode { QuitScene, QuitGame }
}

public partial class MenuController : MonoBehaviour {

	void OnEnable() {
		panelStatsTitle.text = "Statistics";
	}

	void Start() {
		followModeToggle.isOn = GlobalSettings.Instance.followRotation;
		navModeToggle.isOn = GlobalSettings.Instance.cartesian;
		panelOptions.SetActive(false);
		SelectDefaultButton(false);
	}

	void Update() {
		elapsedTimeSeconds += Time.unscaledDeltaTime;
	}

	public void OnGameOver(string title = "Game Over", bool isWin = false) {
		resumeButton.interactable = false;
		if (isWin) restartButton.interactable = false;
		CurrentContext = panelStats;
		panelStatsTitle.text = title;
	}

	public void OnValueChangedMaster(Slider slider) {
		AudioListener.volume = slider.value;
	}

	public void OnValueChangedSfx(Slider slider) {
		sfxMixer.SetFloat("SfxMaster", 0 == slider.value ? -80f : Mathf.Log10(slider.value) * 20f);
	}

	public void OnValueChangedAmbient(Slider slider) {
		ambientMixer.SetFloat("AmbientMaster", 0 == slider.value ? -80f : Mathf.Log10(slider.value) * 20f);
	}

	public void OnButtonResume() {
		GameManager.Instance.OnCancel();
	}

	public void OnButtonStats() {

	}

	public void OnButtonRestart() {
		GameManager.Instance.ignoreInput = true;
		GlobalSettings.Instance.loadPlayer = true;
		GameStats.Instance.frenzyMeterCount = 0;
		StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().name, 1));
	}

	private float elapsedTimeSeconds = 0f;
	public IEnumerator LoadSceneAsync(string sceneName, uint waitTimeSeconds = 0) {
		elapsedTimeSeconds = 0;

		AsyncOperation aop = SceneManager.LoadSceneAsync(sceneName);
		aop.allowSceneActivation = false;

		while (!aop.isDone) {
			if (Input.GetButtonDown("Jump")) {
				elapsedTimeSeconds = waitTimeSeconds;
			}

			yield return loadingBar.fillAmount = (aop.progress + elapsedTimeSeconds) / (1 + waitTimeSeconds);

			if (0.9f <= aop.progress && elapsedTimeSeconds >= waitTimeSeconds) {
				aop.allowSceneActivation = true;
				Time.timeScale = 1f;
				AudioListener.pause = false;
			}
		}

		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

		yield return aop;
	}

	public void OnButtonPlay() {
		LoadLevelScript.currLevel = 0;
		LoadLevelScript.scaleValueHP = 3f;
		LoadLevelScript.scaleValueAS = 1.2f;
		StartCoroutine(LoadSceneAsync("SandboxTutorialRoom", 1));
	}

	public void OnButtonOptions() {

	}

	public void OnButtonCredits() {

	}

	public void OnButtonCreditsFull() {
		LoadLevelScript.currLevel = 0;
		LoadLevelScript.scaleValueHP = 3f;
		LoadLevelScript.scaleValueAS = 1.2f;
		StartCoroutine(LoadSceneAsync("EndCreditScenes", 1));
	}

	public void OnButtonQuit() {

	}

	public void OnButtonQuitScene() {
		GameMusic.ClearInstance();
		GameManager.Instance.ignoreInput = true;
		GlobalSettings.Instance.loadPlayer = false;
		LoadLevelScript.currLevel = 0;
		StartCoroutine(LoadSceneAsync("Scene_MainMenu", 1));
	}

	public void OnButtonQuitGame() {
		PlayerPrefs.Save();
		Application.Quit();
	}

	public void OnButtonCancelQuit(bool clickSource = true) {
		if (!clickSource)
			noButton.onClick.Invoke();

		if (GameManager.Instance)
			if (!GameManager.Instance.gameOver)
				resumeButton.interactable = true;
	}

	public void OnToggleFollowMode() {
		GlobalSettings.Instance.followRotation = followModeToggle.isOn;
		if (FollowPlayer.Instance)
			FollowPlayer.Instance.FollowRotation = followModeToggle.isOn;
		PlayerPrefs.SetInt("FollowMode", followModeToggle.isOn ? 1 : 0);
	}

	public void OnToggleNavMode() {
		GlobalSettings.Instance.cartesian = navModeToggle.isOn;
		if (GameManager.Instance)
			GameManager.Instance.PlayerScript.carterian = navModeToggle.isOn;
		PlayerPrefs.SetInt("NavMode", navModeToggle.isOn ? 1 : 0);
	}

	public void SelectDefaultButton(bool invokeButton) {
		if (null != defaultButton && default != defaultButton) {
			defaultButton.Select();
			if (invokeButton) defaultButton.onClick.Invoke();
		}
	}
}
