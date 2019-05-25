using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl {
	// 标准尺寸
	public const float _stdScreenWidth = 1080f;
	public const float _stdScreenHeight = 1920f;

	public static float widthScale = 0.0f;
	public static float heightScale = 0.0f;
	// 获取宽高
	public static float deviceWidth = Screen.width;
	public static float deviceHeight = Screen.height;

	public static void adjustCanvasScale(GameObject canvas) {
		if (canvas == null) {
			Debug.LogError("Canvas has been lost.");
			return;
		}
		float adjustor = 0f;
		float _standard_aspect = 0f;
		float _device_aspect = 0f;
	
		// 计算宽高比例
		_standard_aspect = _stdScreenWidth / _stdScreenHeight;
		_device_aspect = deviceWidth / deviceWidth;

		// 计算当前设备与标准预设值的比例值，目前用于 Gesture 手势识别范围限定
		widthScale = deviceWidth / _stdScreenWidth;
		heightScale = deviceHeight / _stdScreenHeight;

		// 计算矫正比例
		if (_device_aspect < _standard_aspect) {
			adjustor = _standard_aspect / _device_aspect;
		}

		CanvasScaler canvasScalerTemp = canvas.transform.GetComponent<CanvasScaler>();

		if (adjustor == 0) {
			canvasScalerTemp.matchWidthOrHeight = 1;
		}
		else {
			canvasScalerTemp.matchWidthOrHeight = 0;
		}
	}
}
