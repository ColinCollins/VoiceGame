﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 辅助工具类，用于帮助处理 player 的数据需求
public class utils {

	private MapObj _curMap;
	private DrawMap _mapManager;
	private List<gridObj> _playerPos = new List<gridObj>();

	public utils() {}

	public utils(DrawMap manager) {
		_mapManager = manager;
		_playerPos = _mapManager.getMapData("player");
		_curMap = _mapManager.getCurMap();
	}

	// 更新玩家位置信息 1: nextPos 0: curPos
	public void updatePlayerData(int x, int y) {
		int index = x + _curMap.width * y;
		_playerPos[1].index = index;
		_playerPos[0].color = Color.white;
		_mapManager.draw();
	}

	// 反馈传入位置是否可以移动
	public int getGridData(int x, int y) {
		int index = x + _curMap.width * y;
		return _curMap.mapData[index];
	}

	// 获取 player 当前位置
	public Vector2 getPlayerPosition() {
		// 这里需要注意，绘制和定位是不同的原点，绘制左上角，移动左下角
		int x = _playerPos[0].index % _curMap.width;
		int y = _playerPos[0].index / _curMap.width;
		return new Vector2(x, y);
	}
}