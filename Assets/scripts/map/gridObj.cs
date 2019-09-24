using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 单一网格对象，用于存储必要的数据信息
public class gridObj {

	public int index = 0;

	// 特殊对象比如 player 需要自定义格子颜色用户刷新移动，因此需要保留对应颜色数据信息
	public Color color = Color.white;
	public String gridName = "";
	public GameObject sprite = null;
	public gridObj() {}
	public gridObj(int index, String gridName) {
		this.index = index;
		this.gridName = gridName;
	}

	public gridObj(int index, Color color, String gridName) {
		this.index = index;
		this.color = color;
		this.gridName = gridName;
	}
}
