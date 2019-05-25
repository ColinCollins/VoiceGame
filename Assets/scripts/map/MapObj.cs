using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 存储 map 数据对象

public class MapObj {
	public int width = 0;
	public int height = 0;
	public int[] mapData = null;

	public MapObj(int width, int height, int[] data) {
		this.width = width;
		this.height = height;
		this.mapData = data;
	}
}
