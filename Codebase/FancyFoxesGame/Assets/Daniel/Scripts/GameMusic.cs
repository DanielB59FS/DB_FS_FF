using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour {

	public static GameMusic Instance { get; private set; } = null;

	public List<AudioClip> playlist = null;

	private AudioSource audioSource = null;
	private int clipIndex = 0;

	// Start is called before the first frame update
	void Awake() {
		if (null == Instance) {
			Instance = this;
			Init();
			DontDestroyOnLoad(gameObject);
		}
		else if (this != Instance)
			Destroy(gameObject);
	}

	// Update is called once per frame
	void Update() {
		if (!audioSource.isPlaying) {
			audioSource.clip = playlist[clipIndex++];
			clipIndex %= playlist.Count;
			audioSource.PlayDelayed(1);
		}
	}

	public static void ClearInstance() {
		if (null != Instance) {
			Destroy(Instance.gameObject);
			Instance = null;
		}
	}

	public void Init() {
		audioSource = GetComponent<AudioSource>();
		audioSource.ignoreListenerPause = true;
	}
}
