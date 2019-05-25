using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Vectrosity;

public class DrawMap : MonoBehaviour {
	private MapObj _curMap = null;

	public GameObject mapPanel;
	public Sprite wall;
	public Sprite floor;
	public Sprite player;
	public Sprite stair;
	public Sprite obstacle;
	public GameObject gridPrefab;

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
		if (GameManagerGlobalData.isFirstTimeGenerateMap) {
			generateBasicMapByImage("floor", floor);
			generateSpecialMapImage(_walls, wall);
			generateSpecialMapImage(_obstacle, obstacle);
			generateSpecialMapImage(_player, player);
			generateSpecialMapImage(_exit, stair);
			// generateSpecialMapImage(_monster, monster);
			GameManagerGlobalData.isFirstTimeGenerateMap = false;
		}
		else {
			updatePlayerPosition();
		}
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
					_player.Add(new gridObj(i, ""));
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

	private void generateBasicMapByImage (String name, Sprite sprite) {
		for (int i = 0; i < _curMap.width * _curMap.height; i++) {
			int x = i % _curMap.width;
			int y = i / _curMap.width;
			Rect tempRect = new Rect(_offsetX + _gridWidth * x, _offsetY + _gridHeight* y, _gridWidth, _gridHeight);
			generateSprite(tempRect, name, Color.white, sprite);
		}
	}

	private void generateSpecialMapImage(List<gridObj> list, Sprite sprite = null) {
		int width = _curMap.width;
		int height = _curMap.height;
		for (int i = 0; i < list.Count; i++) {
			gridObj prop = list[i];
			int index = prop.index;
			String name = prop.gridName;
			int x = index % _curMap.width;
			int y = height - index / _curMap.width - 1;
			Rect tempRect = new Rect(_offsetX + _gridWidth * x, _offsetY + _gridHeight * y, _gridWidth, _gridHeight);
			prop.sprite = generateSprite(tempRect, name, prop.color, sprite);
		}
	}

	public void updatePlayerPosition() {
		int index = _player[0].index;
		int x = index % _curMap.width;
		int y = _curMap.height - index / _curMap.width - 1;
		_player[0].sprite.GetComponent<RectTransform>().position = new Vector3(_offsetX + _gridWidth * x, _offsetY + _gridHeight * y, 1);
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

	private GameObject generateSprite(Rect rect, String name = "", Color color = default(Color), Sprite sprite = null) {
		GameObject obj = Instantiate<GameObject>(gridPrefab);
		obj.name = name;
		RectTransform transform = obj.GetComponent<RectTransform>();
		transform.sizeDelta = new Vector2(rect.width, rect.height);
		transform.position = new Vector3(rect.x, rect.y, 1);

		Image image = obj.GetComponent<Image>();
		image.color = color;
		image.sprite = sprite;
		obj.transform.parent = mapPanel.transform;

		return obj;
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
