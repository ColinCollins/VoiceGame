using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 辅助工具类，作为 DrawMap 部分数据对外的接口管理类
public class utils {

	private MapObj _curMap;
	private DrawMap _mapManager;
	private List<gridObj> _playerData = new List<gridObj>();

	private static utils _instance = null;

	public static utils getInstance() {
		if (_instance != null)
			return _instance;
		else
			return null;
	}

	public utils(DrawMap manager) {
		_mapManager = manager;
		_playerData = _mapManager.getMapData("player");
		_instance = this;
	}
	// 更新当前绑定的地图，并且将更新地图的 delegate 函数
	public void updateCurMap() {
		_curMap = _mapManager.getCurMap();
		updateMapSpecialPlotMethod();
	}

	// 更新玩家位置信息 1: nextPos 0: curPos
	public void updatePlayerPosition(int x, int y) {
		int index = x + _curMap.width * y;
		_playerData[0].index = index;
		_mapManager.updatePlayerPosition();
	}

	// 反馈传入位置是否可以移动
	public int getGridData(int x, int y) {
		int index = x + _curMap.width * y;
		return _curMap.mapData[index];
	}

	// 获取 player 当前位置, 这里需要注意，绘制和定位是不同的原点，绘制左上角，移动左下角
	public Vector2 getPlayerPosition() {
		int x = _playerData[0].index % _curMap.width;
		int y = _playerData[0].index / _curMap.width;
		return new Vector2(x, y);
	}
	// 考虑到地图更换
	public void updateMapSpecialPlotMethod() {
		if (_curMap == null) return;
		switch (_curMap.name) {
			// 新建 Wave Data class 记录关卡
			case MapNameData.FIRST_MAP:
				_curMap.handle = firstMapSpecialPlot;
				break;
			default:
				break;
		}
	}

	// 关于各个关卡对应剧情点格子处理方案暂时放在这里
	private void firstMapSpecialPlot(Vector2 pos) {
		Debug.Log("firstMapSpecialPlot: " + pos.x + ": " + pos.y);
		if (pos.x == 11 && pos.y == 14) {

		}
		else if (pos.x == 9 && pos.y == 14) {

		}
	}
}
