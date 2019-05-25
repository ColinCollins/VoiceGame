using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUtils {

	public static bool isKeyBoardUser() {
		return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}

	public static bool isTouchUser() {
		return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
	}
}
