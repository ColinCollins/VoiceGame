using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// 设立回调内容，方便传递回调数据
public delegate void callback();

public class AudioPlayObj {

	public AudioClip clip = null;

	// 最长播放事件一定大于 0
	public float maxTime = -1f;
	public AudioClipCtrl clipCtrl = null;
	public AudioSource player = null;
	public callback handle = () => { };

	public AudioPlayObj(
		AudioClip clip = null,
		AudioClipCtrl clipCtrl = null,
		AudioSource player = null,
		float maxTime = -1.0f) {
		if (clip != null && player != null) {
			this.clip = clip;
			this.clipCtrl = clipCtrl;
			this.player = player;
			this.maxTime = maxTime > 0 ? maxTime : clip.length;
		}
	}

	public AudioPlayObj() { }
}
