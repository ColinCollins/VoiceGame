using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : MonoBehaviour {

	private Player _owner;
	private Vector2 _pos = Vector2.zero;
	private bool _isMoving = false;
	public bool isMoving {
		get {
			return _isMoving;
		}
	}
	// 切换手势功能到移动
	public void switchGestureToMove() {
		GestureCtrl.getInstance().toLeftGesture = moveLeft;
		GestureCtrl.getInstance().toRightGesture = moveRight;
		GestureCtrl.getInstance().toBackGesture = _owner.refuseMoveBack;
		GestureCtrl.getInstance().toFrontGesture = moveFront;
		GestureCtrl.getInstance().toCenterGesture = null;
	}

	// 外传的计时器
	public void timer() {
		// Debug.Log("MoveCtrl player state: " + _owner.getState());
		if (_owner.getState() != PlayerState.Idle) return;
		if (PlatformUtils.isKeyBoardUser())
			keyBoardEvent();
		// 为了能够有管控，因为 mobile 平台上 gesture 替代了 keyboard 所以要有与之对应的管控。parry 设置同理。
		GestureCtrl.getInstance().timer();
	}

	public void setPosition(Vector2 pos) {
		_pos = pos;
	}

	public void setOwner(Player owner) {
		_owner = owner;
	}

	private void gestureEvent() { }

	private void keyBoardEvent() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			moveFront();
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			moveLeft();
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			moveBack();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			moveRight();
		}
 	}

	public void moveLeft() {
		int x = (int)(_pos.x - 1);
		int y = (int)_pos.y;
		if (_owner.checkPositionState(x, y)) {
			this.setPosition(new Vector2(x, y));
			_owner.setPosition(_pos);
		}
		_owner.action();
		Debug.Log("Left");
	}

	public void moveFront() {
		int x = (int)_pos.x;
		int y = (int)(_pos.y - 1);
		if (_owner.checkPositionState(x, y)) {
			this.setPosition(new Vector2(x, y));
			_owner.setPosition(_pos);
		}
		_owner.action();
		Debug.Log("Front");
	}

	public void moveBack() {
		int x = (int)_pos.x;
		int y = (int)(_pos.y + 1);
		if (_owner.checkPositionState(x, y)) {
			this.setPosition(new Vector2(x, y));
			_owner.setPosition(_pos);
		}
		_owner.action();
		Debug.Log("Back");
	}

	public void moveRight() {
		int x = (int)(_pos.x + 1);
		int y = (int)_pos.y;
		if (_owner.checkPositionState(x, y)) {
			this.setPosition(new Vector2(x, y));
			_owner.setPosition(_pos);
		}
		_owner.action();
		Debug.Log("Right");
	}

	public void startMoving() {
		_isMoving = true;
	}

	public void endMoving() {
		_isMoving = false;
	}
}
