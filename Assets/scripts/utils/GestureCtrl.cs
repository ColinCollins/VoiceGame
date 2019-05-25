using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void gesture();


public class GestureCtrl {
	private float doubleClickElapseTime = 0.7f;
	private static GestureCtrl _instance = null;
	// 当前含有手势种类
	public gesture toLeftGesture = null;            // 向左滑动手势
	public gesture toRightGesture = null;           // 向右滑动手势
	public gesture toFrontGesture = null;           // 向前滑动手势
	public gesture toBackGesture = null;			// 向后滑动手势
	public gesture toCenterGesture = null;          // 中心点击事件

	// 考虑到后期的手势识别要素，要规定滑动的准确范围区间才可以。
	public float minHorizontalMoveWidthLength = 0.0f;				// 最小判定水平滑动长度
	public float minVerticalMoveHeightLength = 0.0f;				// 最小判定垂直滑动长度
	public float maxHorizontalMoveBoardY = 80f;						// 水平滑动标准最大高度区间
	public float maxVerticalMoveBoardX = 100f;						// 垂直滑动标准最大宽度区间

	protected float _clickTime = 0.0f;
	protected bool isClick = false;
	private bool isDown = false;

	private Vector3 _startPos = Vector3.zero;
	private Vector3 _endPos = Vector3.zero;

	public float _screenWidth = 0.0f;
	private float _screenHeight = 0.0f;

	// test mouse input
	private bool _mouseDown = false;

	public static GestureCtrl getInstance() {
		if (_instance == null) {
			_instance = new GestureCtrl();
		}
		return _instance;
	}

	public static void removeInstance() {
		_instance = null;
	}

	// 这个在场景一 执行就可以了，跳转之后数据做了保留
	public void preInit() {
		// 获取当前设备宽高
		_screenWidth = CanvasCtrl.deviceWidth;
		_screenHeight = CanvasCtrl.deviceHeight;
		// 设计滑动长度的最小极限值
		minHorizontalMoveWidthLength = _screenWidth * 0.4f;
		minVerticalMoveHeightLength = _screenHeight * 0.3f;
		// 限制手指滑动范围,适当的扩大范围
		maxHorizontalMoveBoardY *= CanvasCtrl.heightScale;
		maxVerticalMoveBoardX *= CanvasCtrl.widthScale;
	}

	public void init() {
		_clickTime = 0.0f;
		isClick = false;
	}

	public void timer() {
		if (isClick) _clickTime += Time.deltaTime;
		quitApplication();
		if (Input.touchCount > 0) {
			Touch t = Input.GetTouch(0);

			if (t.phase == TouchPhase.Began && !isDown) {
				_startPos = t.position;
				// Debug.Log("_startTouchPos: " + _startPos);
				isDown = true;
			}

			if (t.phase == TouchPhase.Ended && isDown) {
				_endPos = t.position;
				// Debug.Log("_endTouchPos: " + _endPos);
				// 这里的执行顺序有讲究，优先判定非滑动事件之后裁判定为点击事件
				if (!adjustGesture(_startPos, _endPos) && isDoubleCheck()) {
					Debug.Log("Double Click make sure");
					if (toCenterGesture != null) toCenterGesture();
				}
				isDown = false;
			}
		}
		// 测试，这个测试主要是用于将区间限定的效果查看的更加清楚。
		if (PlatformUtils.isKeyBoardUser()) {
			if (Input.GetMouseButtonDown(0)) {
				_startPos = Input.mousePosition;
				_mouseDown = true;
				//  Debug.Log("_startTouchPos: " + _startPos);
			}

			if (Input.GetMouseButton(0) && _mouseDown) {
				_endPos = Input.mousePosition;
			}

			if (Input.GetMouseButtonUp(0)) {
				_mouseDown = false;
				_endPos = Input.mousePosition;
				//  Debug.Log("_endTouchPos: " + _endPos);
				if (!adjustGesture(_startPos, _endPos) && isDoubleCheck()) {
					Debug.Log("Double Click make sure");
					if (toCenterGesture != null) toCenterGesture();
					return;
				}
				_startPos = Vector3.zero;
				_endPos = Vector3.zero;
			}
		}
	}

	public void quitApplication() {
		if (PlatformUtils.isTouchUser() && Input.GetKeyDown(KeyCode.Escape)) {
			if (isDoubleCheck()) {
				Debug.Log("Is Double Click");
				// 返回主界面
				Application.Quit();
			}
		}
	}

	// 双击确认操作
	public bool isDoubleCheck() {
		if (!isClick) {
			isClick = true;
			return false;
		}

		if (_clickTime <= doubleClickElapseTime) {
			_clickTime = 0f;
			isClick = false;
			return true;
		}

		_clickTime = 0f;
		isClick = false;
		return false;
	}
	// 判断是否是滑动操作，若不是滑动操作则认为是点击一次。原点在左下角
	private bool adjustGesture(Vector3 startPos, Vector3 endPos) {
		float dx = startPos.x - endPos.x;
		float dy = startPos.y - endPos.y;
		bool isGesture = false; 
		// horizontal
		if (Mathf.Abs(dx) > minHorizontalMoveWidthLength && Mathf.Abs(dy) / 2 < maxHorizontalMoveBoardY) {
			runActionX(dx);
			isGesture = true;
		}
		// vertical
		else if (Mathf.Abs(dy) > minVerticalMoveHeightLength && Mathf.Abs(dx) / 2 < maxVerticalMoveBoardX) {
			runActionY(dy);
			isGesture = true;
		}
		else if (Mathf.Abs(dx) > minVerticalMoveHeightLength && Mathf.Abs(dy) > minHorizontalMoveWidthLength) {
			// Custome gesture to added here，这里用斜率判断并且判断方向，但是目前由于功能设计不够完善，强行设计一下
			if (Mathf.Abs(dx) > Mathf.Abs(dy)) {
				runActionX(dx);
			}
			else {
				runActionY(dy);
			}
			isGesture = true;
		}

		_startPos = Vector3.zero;
		_endPos = Vector3.zero;
		return isGesture;
	}

	private void runActionX(float dx) {
		if (dx > 0) {
			if (toLeftGesture != null) toLeftGesture();
		}
		else {
			if (toRightGesture != null) toRightGesture();
		}
	}

	private void runActionY(float dy) {
		if (dy > 0)
		{
			if (toBackGesture != null) toBackGesture();
		}
		else
		{
			if (toFrontGesture != null) toFrontGesture();
		}
	}

	public Vector3 getStartPos() {
		return new Vector3(_startPos.x, _startPos.y, _startPos.z);
	}

	public Vector3 getEndPos() {
		return new Vector3(_endPos.x, _endPos.y, _endPos.z);
	}
}
