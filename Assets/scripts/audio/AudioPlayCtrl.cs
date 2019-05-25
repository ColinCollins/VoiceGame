using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayCtrl : MonoBehaviour
{

	private List<AudioPlayObj> _effectClipPlayList = new List<AudioPlayObj>();
	private AudioPlayObj _effectClipObj = null;

	private List<AudioPlayObj> _backgroundClipPlayList = new List<AudioPlayObj>();
	private AudioPlayObj _backgroundClipObj = null;

	private static AudioPlayCtrl _instance = null;

	public static AudioPlayCtrl getInstance()
	{
		return _instance;
	}

	public static void init(GameObject manager) {
		_instance = manager.GetComponent<AudioPlayCtrl>();
	}

	// 添加需要播放的音效当队列中
	public void addEffectObj(AudioPlayObj clipObj) {
		_effectClipPlayList.Add(clipObj);
	}

	public void addBackgroundObj(AudioPlayObj clipObj) {
		_backgroundClipPlayList.Add(clipObj);
	}

	private void Update() {
		Debug.Log("_EffectClipObj: " + _effectClipObj);
		autoPlay(ref _effectClipPlayList,ref _effectClipObj);
		autoPlay(ref _backgroundClipPlayList, ref _backgroundClipObj);
	}

	// 值传递对象
	private void autoPlay(ref List<AudioPlayObj> clipList, ref AudioPlayObj clipObj) {
		if (clipList.Count <= 0) return;
		if (removeCurObj(ref clipList, ref clipObj)) {
			clipObj = clipList[0];
			clipObj.player.clip = clipObj.clip;
			clipObj.player.time = 0;
			clipObj.player.Play();
			Debug.Log("clipObj MaxTime: " + clipObj.maxTime);
		}
	}

	private bool removeCurObj(ref List<AudioPlayObj> clipList,ref AudioPlayObj clipObj) {
		if (clipObj == null) return true;
		// 判断当前播放时间是否超过规定时间
		clipObj.maxTime -= Time.deltaTime;
		if (clipObj.maxTime <= 0) {
			clipObj.player.Stop();
			clipList.RemoveAt(0);
			clipObj.handle();
			clipObj = null;
		}
		return false;
	}
	// 因为音频播放是异步的，方便及时增加播放后的回调内容
	public void addCallbackToEffectClip(System.String clipName = "", callback method = null) {
		if (method == null) {
			ProjectUtils.Warn("addCallbackToEffectClip Lost method");
			return;
		};
		// 适配，当 audio 刚刚加入队列时, _effectClipObj 并未被赋值
		if (!clipName.Trim().Equals("")) {
			// 避免同名对象进入而使得 callback 没有成功绑定
			for (int i = _effectClipPlayList.Count - 1; i >= 0; i--) {
				AudioPlayObj obj = _effectClipPlayList[i];
				if (obj.clip.name == clipName) {
					obj.handle += method;
					return;
				}
			}
		}
	}
	// background 控制对象添加 callback
	public void addCallbackToBackgroundClip(System.String clipName = "", callback method = null) {
		if (method == null)
		{
			ProjectUtils.Warn("addCallbackToBackgroundClip Lost method");
			return;
		};

		if (!clipName.Trim().Equals(""))
		{
			for (int i = _backgroundClipPlayList.Count - 1; i >= 0; i--)
			{
				AudioPlayObj obj = _backgroundClipPlayList[i];
				if (obj.clip.name == clipName) {
					obj.handle += method;
					return;
				}
			}
		}
	}

		// 停止播放
		public void stopEffect() {
		if (_effectClipObj == null) return;
		// To Stop current Playing
		if (_effectClipObj.player.isPlaying)
		{
			_effectClipObj.player.Stop();
			if (_effectClipObj.handle != null) _effectClipObj.handle();
			_effectClipPlayList.RemoveAt(0);
			_effectClipObj = null;
		}
	}
}
