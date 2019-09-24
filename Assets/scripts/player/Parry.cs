using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ArrowDir {
	LEFT,			// 1
	RIGHT,			// 2
	FRONT,			// 3
	// BACK,		// 4
	 CENTER			// 5
}

// 格挡类
public class Parry {

	public float elapseTime = 6.0f;					// 格挡等待时间
	public float offsetTime = 2.7f;					// 准确位移格挡时间段
	public float rangeTime = 2.0f;					// 格挡区间范围
	public float parryElapseTime = 0.5f;            // 单次格挡间隔时间
	public int maxParryCount = 1;					// 记录单把剑可以坚持格挡成功次数

	private ArrowDir _arrowDir = ArrowDir.CENTER;
	private Player _owner;
	private float _curTime = 0f;					// 记录当前格挡事件时间
	private float _restTime = 0f;                   // 记录当前格挡间隔时间
	private bool _isParry = false;					// 是否处在格挡阶段

	private Slider _sliderBar = null;				// 用于视觉判断格挡是否成功
	private Image _sliderFillImage = null;

	private bool _firstSuccess = true;				// 初次格挡成功
	private int _parrySuccessCount = 0;				// 当前刀刃成功格挡次数

	private static Parry _instance = null;
	public static Parry getInstance() {
		if (_instance == null) {
			_instance = new Parry();
			Debug.Log("Parry component init to declaration itself.");
		}
		return _instance;
	}

	public void init(Player player) {
		_owner = player;
		_isParry = false;
		_firstSuccess = true;
		_sliderBar = player.sliderBar;
		_sliderFillImage = _sliderBar.targetGraphic.transform.GetComponent<Image>();
	}

	public ArrowDir startParry() {
		_arrowDir = getArrowDir();

		// slider state
		_sliderBar.value = 1;
		_curTime = 0;
		_sliderFillImage.color = Color.white;

		if (!GameManagerGlobalData.isFirstMeetObstacle && !GameManagerGlobalData.isSecondMeetObstacle)
			_isParry = true;

		// 设置格挡手势功能调用
		setGestureToParry();

		return _arrowDir;
	}

	// flag 用于判断，当前格挡状态是成功还是失败
	public void endParry(bool flag) {
		if (flag) {
			// 格挡成功
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.PARRY_CLIP, () => {
				if (!_firstSuccess) _owner.setState(PlayerState.Idle);
			});

			// 成功格挡次数 + 1
			_parrySuccessCount++;
			if (getSwordRemainCount() == 1) {
				PlayerAudioCtrl.getInstance().play(PlayerAudioData.SWORD_WILL_BREAK_CLIP);
			}

			// 播放初次格挡成功音效
			if (_firstSuccess) {
				PlayerAudioCtrl.getInstance().play(PlayerAudioData.ESCAPE_CLIP, () => {
					_firstSuccess = false;
					_owner.setState(PlayerState.Idle);
				});
			}

			_isParry = false;
			ProjectUtils.Log("Parry Success");
		}
		else {
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.HURT_CLIP, () => {
				if (_owner.getLife() != 2) _owner.setLife(-1);
			});

			if (_owner.getLife() == 2) {
				PlayerAudioCtrl.getInstance().play(PlayerAudioData.EXCITATION_CLIP, () => {
					_owner.setState(PlayerState.Idle);
					_owner.setLife(-1);
				});
			}

			_isParry = false;
			ProjectUtils.Log("Parry Failed");
		}

		_owner.switchGestureToMove();
		_sliderBar.value = 0;
		_sliderFillImage.color = Color.white;
	}

	public ArrowDir getArrowDir() {
		int num = (int)UnityEngine.Random.Range(0, 3);

		// Debug.Log("ArrowDir: " + num);

		if (GameManagerGlobalData.isFirstMeetObstacle) {
			return ArrowDir.FRONT;
		}
		else if (GameManagerGlobalData.isSecondMeetObstacle) {
			return ArrowDir.LEFT;
		}
		else {
			switch (num) {
				case 0: return ArrowDir.LEFT;
				case 1: return ArrowDir.FRONT;
				// case 2: return ArrowDir.BACK;
				case 2: return ArrowDir.RIGHT;
				// 因为效果难以区分，暂时不做 center 以及 back 的陷阱触发
				default: return ArrowDir.LEFT;
			}
		}
	}

	public void timer() {
		if (!_isParry) return;

		float dt = Time.deltaTime;
		_curTime += dt;
		_restTime -= dt;
		sliderEffect();

		if (_curTime <= elapseTime && _restTime <= 0) {
			if (PlatformUtils.isKeyBoardUser())
				keyBoardEvent();
			GestureCtrl.getInstance().timer();
		}
		else if (_curTime > elapseTime) {
			this.endParry(false);
		}
	}

	// 键盘操控，挥刀方向
	private void keyBoardEvent() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (checkParry(ArrowDir.FRONT)) endParry(true);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			if (checkParry(ArrowDir.LEFT)) endParry(true);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			if (checkParry(ArrowDir.RIGHT)) endParry(true);
		}
	}

	public void setGestureToParry() {
		GestureCtrl.getInstance().toLeftGesture = () => {
			if (checkParry(ArrowDir.LEFT)) endParry(true);
		};
		GestureCtrl.getInstance().toRightGesture = () => {
			if (checkParry(ArrowDir.RIGHT)) endParry(true);
		};
		GestureCtrl.getInstance().toFrontGesture = () => {
			if (checkParry(ArrowDir.FRONT)) endParry(true);
		};
		GestureCtrl.getInstance().toCenterGesture = null;
		GestureCtrl.getInstance().toBackGesture = null;
	}

	private bool checkParry(ArrowDir _dir) {
		PlayerAudioCtrl.getInstance().play(PlayerAudioData.SWING_CLIP);
		_restTime = parryElapseTime;
		return inTimeRange() && _dir == _arrowDir;
	}

	private bool inTimeRange() {
		return (_curTime >= (offsetTime - rangeTime)) && (_curTime <= (offsetTime + rangeTime)) && _curTime < elapseTime;
	}

	// 交由外部控制 _Parry 属性
	public void setParry(bool isParry) {
		_isParry = isParry;
	}

	// 滑动条效果
	public void sliderEffect() {
		if (inTimeRange()) {
			if (_sliderFillImage.color != Color.red) {
				_sliderFillImage.color = Color.red;
			}
		}
		else
			_sliderFillImage.color = Color.white;
		_sliderBar.value = 1 - _curTime / elapseTime;
	}

	// 当前剑的剩余寿命
	public int getSwordRemainCount() {
		// 剑的功能不够完善，暂时不做完它，这里留一个 bug
		if (_parrySuccessCount > maxParryCount) return 0;
		return maxParryCount - _parrySuccessCount;
	}

	// 重置剑的寿命
	public void setNewSword() {
		_parrySuccessCount = 0;
	}
}
