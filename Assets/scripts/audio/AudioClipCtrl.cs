using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioClipCtrl : MonoBehaviour {

	public List<AudioClip> clips = new List<AudioClip>();					// 播放片段
	public List<AudioSource> audioPlayers = new List<AudioSource>();		// 播放对象
	[HideInInspector]
	public AudioSource sources;

	protected Dictionary<String, float> _limitClipName = new Dictionary<String, float>();

	// 选择播放片段
	public AudioClip selectClip(String name) {
		if (clips.Count <= 0) return null;
		AudioClip clip = null;

		for (int i = 0; i < clips.Count; i++) {
			AudioClip c = clips[i];
			if (c.name == name) {
					clip = c;
			}
		}

		if (clip == null) ProjectUtils.Warn("Can't find clip: " + name);

		return clip;
	}

	// 选择播放对象
	public AudioSource selectSource(String name) {
		if (audioPlayers.Count <= 0) return null;
		AudioSource player = null;

		for (int i = 0; i < audioPlayers.Count; i++) {
			AudioSource s = audioPlayers[i];
			if (s.transform.name == name) {
				player = s;
			}
		}

		if (player == null) ProjectUtils.Warn("Can't find source: " + name);

		return player;
	}

	// 这个功能是有可复用性的，只是针对 playeAudioCtrl 并不有效
	public virtual void play(String clipName, callback method = null) {
		if (audioPlayers.Count > 0 && clips.Count > 0) {
			AudioPlayObj obj = new AudioPlayObj(selectClip(clipName), this, audioPlayers[0], 3.5f);
			AudioPlayCtrl.getInstance().addEffectObj(obj);

			if (method != null) AudioPlayCtrl.getInstance().addCallbackToEffectClip(clipName, method);
		}
	}
}
