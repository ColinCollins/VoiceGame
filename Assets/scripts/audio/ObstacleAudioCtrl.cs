using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObstacleAudioCtrl : AudioClipCtrl {

	private static ObstacleAudioCtrl _instance = null;

	public static ObstacleAudioCtrl getInstance() {
		return _instance;
	}

	public static void init(GameObject manager) {
		_instance = manager.GetComponent<ObstacleAudioCtrl>();
	}

	public void playAudio(ArrowDir dir) {
		String name = selectedDir(dir);
		System.String clipName = "";
		AudioPlayObj obj = null;

		if (dir == ArrowDir.FRONT) {
			clipName = "frontSword";
			obj = new AudioPlayObj(selectClip(clipName), this, selectSource(name));
		}
		else {
			clipName = "bow";
			obj = new AudioPlayObj(selectClip(clipName), this, selectSource(name));
		}

		AudioPlayCtrl.getInstance().addEffectObj(obj);

		// 教学音频设计需要
		if (GameManagerGlobalData.isFirstMeetObstacle) {
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.PARRY_TO_FRONT_TIPS, () => {
				Parry.getInstance().setParry(true);
				GameManagerGlobalData.isFirstMeetObstacle = false;
			});
		}
		else if (GameManagerGlobalData.isSecondMeetObstacle) {
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.PARRY_TO_LEFT_TIPS, () => {
				Parry.getInstance().setParry(true);
				GameManagerGlobalData.isSecondMeetObstacle = false;
			});
		}
	}

	private String selectedDir(ArrowDir dir) {
		String name = "";

		switch (dir) {
			// case ArrowDir.BACK: name = "back"; break;
			case ArrowDir.FRONT: name = "front"; break;
			case ArrowDir.LEFT: name = "left"; break;
			case ArrowDir.RIGHT: name = "right"; break;
			// case ArrowDir.CENTER: name = "center"; break;
		}

		return name;
	}
}
