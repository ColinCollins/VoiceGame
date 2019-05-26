using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 用户存储游戏内部共有的数据信息
public class GameManagerGlobalData {
	public static bool isFirstTimeEnter = true;
	// 用于临时修改地图绘制功能
	public static bool isFirstTimeGenerateMap = true;

	// PlayerAction State
	public static bool isFirstMeetObstacle = true;
	public static bool isSecondMeetObstacle = true;
	public static bool isFirstMeetMonster = true;
}
