using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudioCtrl : AudioClipCtrl {

	private static BackgroundAudioCtrl _instance = null;

	private AudioSource _rain = null;
	private AudioSource _atom = null;

	private float _atomDiffPitchValue = 0;
	private float _atomGolPitchValue = 0;
	private float _interDeltaTime = 0;
	private float _atomElapseTime = 15.0f;
	private float _curSpeadTime = 0.0f;

	public static BackgroundAudioCtrl getInstance() {
		return _instance;
	}

	public static void init(GameObject manager) {
		_instance = manager.GetComponent<BackgroundAudioCtrl>();
	}

	private void Start() {
		_atom = selectSource("atmo");
		_rain = selectSource("rain");
		_rainPlayStart();
		_atomPlayStart();
	}

	private void Update() {
		_rainPlayRaining();
		// randomAtomEffect();
		_curSpeadTime -= Time.deltaTime;

		if (_curSpeadTime <= 0) {
			_curSpeadTime = _atomElapseTime;
			_atom.Play();
		}
	}

	private void _rainPlayStart() {
		_rain.clip = selectClip("raining");
		_rain.loop = false;
		_rain.Play();
	}

	private void _rainPlayRaining() {
		if (_rain.isPlaying && _rain.clip.name == "Jamboree") return;
		// Debug.Log("Playing the second clip");
		_rain.clip = selectClip("Jamboree");
		_rain.loop = true;
		_rain.Play();
	}

	private void _atomPlayStart() {
		_atomDiffPitchValue = _atom.pitch;
		_atom.Play();
	}

	// 用于控制恐怖氛围的音效变化
	private void randomAtomEffect() {
		if (_interDeltaTime > 0) {
			_interDeltaTime -= Time.deltaTime;
			_atom.pitch += Time.deltaTime * _atomDiffPitchValue;
		}
		else {
			_atomGolPitchValue = Random.Range(-2, 2);
			_atomDiffPitchValue = _atomGolPitchValue > _atom.pitch ? _atomGolPitchValue - _atom.pitch : _atom.pitch - _atomGolPitchValue;
			_interDeltaTime = Random.Range(15, 30);
			_atomDiffPitchValue /= _interDeltaTime;
		}
	}

	// 添加背景音乐播放
	public override void play(System.String clipName, callback method = null) {
		if (audioPlayers.Count <= 0 || clips.Count <= 0) return;
		float maxTime = -1;
		AudioSource player = _atom;

		switch (clipName) {
			case "":
				player.clip = null;
				return;
		}

		AudioPlayObj obj = new AudioPlayObj(selectClip(clipName), this, player, maxTime);
		AudioPlayCtrl.getInstance().addBackgroundObj(obj);

		if (method != null) AudioPlayCtrl.getInstance().addCallbackToBackgroundClip(clipName, method);
	}
}
