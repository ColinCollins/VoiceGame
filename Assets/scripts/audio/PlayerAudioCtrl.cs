using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAudioCtrl : AudioClipCtrl {
	// 因为和 player 的状态相关，所以需要添加相应的状态控制。
	public Player owner = null;
	private static PlayerAudioCtrl _instance = null;
	private AudioSource _center = null;
	private AudioSource _switch = null;
	
	public static PlayerAudioCtrl getInstance() {
		return _instance;
	}

	public static void init(GameObject manager) {
		_instance = manager.GetComponent<PlayerAudioCtrl>();
	}

	private void Start() {
		_center = selectSource("center");
		_switch = selectSource("switch");

		if (_center == null || _switch == null) {
			ProjectUtils.Error("PlayerAudioCtrl lost target.");
			return;
		}
	}

	public override void play(String clipName, callback method = null) {
		if (audioPlayers.Count <= 0 || clips.Count <= 0) return;
		float maxTime = -1;
		AudioSource player = _center;
		// specific clip data. switch 的传入判断值必须在运行时就已经是既定值，即 const 不能作为判断对象。
		switch (clipName) {
			case PlayerAudioData.SWITCH_OBSTACLE_CLIP:
				player = _switch;
				break;
			default: break;
		}

		AudioPlayObj obj = new AudioPlayObj(selectClip(clipName), this, player, maxTime);
		AudioPlayCtrl.getInstance().addEffectObj(obj);
		if (method != null) AudioPlayCtrl.getInstance().addCallbackToEffectClip(clipName, method);
	}
}
