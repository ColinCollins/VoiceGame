using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneAudioCtrl : AudioClipCtrl {

	private static StartSceneAudioCtrl _instance = null;
	public AudioSource manager = null;

	public static StartSceneAudioCtrl getInstance() {
		return _instance;
	}

	public static void init(GameObject manager) {
		_instance = manager.GetComponent<StartSceneAudioCtrl>();
	}

	public override void play(System.String clipName, callback method = null) {
		if (clips.Count <= 0) return;
		float maxTime = -1;
		AudioSource player = manager;

		AudioPlayObj obj = new AudioPlayObj(selectClip(clipName), this, player, maxTime);
		AudioPlayCtrl.getInstance().addEffectObj(obj);

		if (method != null)
			AudioPlayCtrl.getInstance().addCallbackToEffectClip(clipName, method);
	}
}
