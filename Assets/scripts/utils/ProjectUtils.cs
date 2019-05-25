using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectUtils {
	#region Debug
	public static void Log(String msg) {
		if (GameManager.getInstance().getGameState() == GameState.GameOver) return;
		Debug.Log(msg);
	}

	public static void Warn(String msg) {
		if (GameManager.getInstance().getGameState() == GameState.GameOver) return;
		Debug.LogWarning(msg);
	}

	public static void Error(String msg) {
		if (GameManager.getInstance().getGameState() == GameState.GameOver) return;
		Debug.LogError(msg);
	}
	#endregion

	#region ChildIteration

	#endregion
}
