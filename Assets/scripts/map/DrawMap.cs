using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Vectrosity;

public class DrawMap : MonoBehaviour {
	private MapObj _curMap = null;

	public GameObject mapPanel;
	public Texture2D wall;
	public Texture2D floor;
	public Texture2D player;
	public Texture2D stair;

	private int _gridWidth = 0;
	private int _gridHeight = 0;

	private int _offsetX = 0;
	private int _offsetY = 0;

	// map data index list. create dynamic: player, exit, basic, wallls, obstacles
	private List<VectorObject2D> _grids = new List<VectorObject2D>();

	private List<gridObj> _obstacle = new List<gridObj>();
	private List<gridObj> _walls = new List<gridObj>();
	private List<gridObj> _exit = new List<gridObj>();
	private List<gridObj> _player = new List<gridObj>();
	private List<gridObj> _monster = new List<gridObj>();

	// 创建多个 Line 对象
	public void Init() {
		// 0 -> walls, 1 -> obstacle, 2 -> exit, 3 -> player 4 -> basic, 5 -> monster
		for (int i = 0; i < 6; i++) {
			VectorObject2D comp = createRectangle(0, 0, 0, 0, 0, Color.white);
			comp.transform.SetParent(mapPanel.transform);
			_grids.Add(comp);
		}
	}

	public void Destroy() {
		int count = mapPanel.transform.childCount;
		for (int i = 0; i < count; i++) {
			Destroy(mapPanel.transform.GetChild(0).gameObject);
		}
	}

	public void setMap(MapObj map) {
		_curMap = map;
		float aspect = CanvasCtrl.deviceWidth / CanvasCtrl.deviceHeight;
		this._gridWidth = Mathf.FloorToInt(CanvasCtrl.deviceWidth / (_curMap.width + 2));
		this._gridHeight = Mathf.CeilToInt(CanvasCtrl.deviceHeight / (_curMap.height + 2) * aspect) ;

		this._offsetX = Mathf.CeilToInt(this._gridWidth);
		this._offsetY = Mathf.CeilToInt(_curMap.height / 2 * this._gridHeight);

		parseMapData();
	}

	// 提供当前地图数据给外部。
	public MapObj getCurMap() {
		return new MapObj(_curMap.width, _curMap.height, _curMap.mapData);
	}

	// 获取解析后地图数据
	public List<gridObj> getMapData(String name) {
		List<gridObj> tempList;
		switch (name) {
			case "player":
				tempList = _player;
				break;
			case "exit":
				tempList = _exit;
				break;
			case "wall":
				tempList = _walls;
				break;
			case "obstacle":
				tempList = _obstacle;
				break;
			case "monster":
				tempList = _monster;
				break;
			default:
				Debug.LogWarning("Can't find this data");
				return null;
		}
		List<gridObj> newList = new List<gridObj>();

		tempList.ForEach((obj) =>
		{
			newList.Add(obj);
		});
		return newList;
	}

	// 绘制地图
	public void draw() {
		if (_curMap == null || !GameManager.getInstance().isShow) return;
		// draw basic map
		drawBasicMap(_grids[4]);
		// draw walls
		decorativeMap(_grids[0], _walls, Color.blue);
		// draw obstacle
		decorativeMap(_grids[1], _obstacle, Color.red);
		// draw monster
		decorativeMap(_grids[5], _monster, new Color(187 / 255f, 143 / 255f, 206 / 255f, 1.0f));
		// draw exit
		decorativeMap(_grids[2], _exit, Color.green);
		// draw player
		decorativeMap(_grids[3], _player);
	}

	// 分析地图数据
	public void parseMapData() {
		int[] mapData = this._curMap.mapData;
		for (int i = 0; i < mapData.Length; i++) {
			switch (mapData[i]) {
				// wall
				case -1:
					_walls.Add(new gridObj(i, "wall"));
					break;
				// obstacle
				case -2:
					_obstacle.Add(new gridObj(i, "obstacle"));
					break;
				// player
				case 1:
					_player.Add(new gridObj(i, Color.yellow, ""));
					// keep the origin player data
					_player.Add(new gridObj(i, Color.yellow, ""));
					break;
				// exit
				case 3:
					_exit.Add(new gridObj(i, "wall"));
					break;
				case 4:
					_monster.Add(new gridObj(i, "monster"));
					break;
			}
		}
	}

	// 创建绘制对象
	private VectorObject2D createRectangle(int x, int y, int width, int height, float lineWidth, Color color) {
		GameObject obj = new GameObject();
		obj.AddComponent<VectorObject2D>();
		VectorObject2D comp = obj.GetComponent<VectorObject2D>();
		comp.vectorLine = null;
		return comp;
	}

	// origin at the left-bottom
	private void drawBasicMap(VectorObject2D draw) {
		// Get Transform
		Transform trans = draw.transform;
		int width = _curMap.width;
		int height = _curMap.height;
		List<Vector2> points = new List<Vector2>(8 * width * height);
		// draw main block
		VectorLine lines = new VectorLine("Line", points, null, 5.0f);
		draw.vectorLine = lines;

		for (int i = 0; i < width; i++) {
			// index == offset
			draw.vectorLine.MakeRect(new Rect(_offsetX + _gridWidth * i, _offsetY, _gridWidth, _gridHeight * height), i * 8);
		}
		for (int i = 0; i < height; i++) {
			draw.vectorLine.MakeRect(new Rect(_offsetX, _offsetY + _gridHeight * i, _gridWidth * width, _gridHeight), (width + i) * 8);
		}
		draw.vectorLine.Draw();
	}

	// 绘制对应数据 grid
	private void decorativeMap(VectorObject2D draw, List<gridObj> list, Color color = default(Color)) {
		int count = list.Count;
		int width = _curMap.width;
		int height = _curMap.height;
		VectorLine lines = draw.vectorLine;

		List<Vector2> points = new List<Vector2>(8 * count);
		lines = new VectorLine("Line", points, null, 5.0f);
		// 这么些从理论上是可以节省数据内存的，lines 存储 Color 一定是有开销的
		if (color != default(Color)) lines.SetColor(color);
		draw.vectorLine = lines;
		// 这是一个可优化点，可以将数据存储之后直接套用而不是每次都重新计算。
		for (int i = 0; i < count; i++) {
			int index = list[i].index;
			int x = index % width;
			int y = (height - 1) - index / width;
			int offsetX = _offsetX + _gridWidth * x;
			int offsetY = _offsetY + _gridHeight * y;
			if (color == default(Color))
				lines.SetColor(list[i].color, i * 4, (i + 1) * 4);
			draw.vectorLine.MakeRect(new Rect(offsetX, offsetY, _gridWidth, _gridHeight), i * 8);
		}

		draw.vectorLine.Draw();
	}
	// 清空所有的 line 绘制，用于 display 显示关闭
	public void showMap() {
		if (GameManager.getInstance().isShow) {
			for (int i = 0; i < _grids.Count; i++) {
				clearDrawer(_grids[i]);
			}
		}
		else {
			draw();
		}
		// 转换
		GameManager.getInstance().isShow = !GameManager.getInstance().isShow;
	}
	// 清空单个 line 绘制
	private void clearDrawer(VectorObject2D draw) {
		VectorLine.Destroy(ref draw.vectorLine);
	}

}
