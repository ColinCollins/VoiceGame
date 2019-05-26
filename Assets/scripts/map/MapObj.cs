using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 存储 map 数据对象

public class MapObj {
	public delegate void SpecialPlot(Vector2 pos);

	public string name = "";
	public int width = 0;
	public int height = 0;
	public int[] mapData = null;
	public SpecialPlot handle = null;

	public MapObj(int width, int height, int[] data, string name) {
		this.width = width;
		this.height = height;
		this.mapData = data;
		this.name = name;
	}
}
