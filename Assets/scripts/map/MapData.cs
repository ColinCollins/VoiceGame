using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  floor grid state:
 *  0: moveable
 *  1: player
 *  2: Npc
 *  3: exit
 *  -1: wall
 *  -2: obstacle (obs),
 *  x: special stuff (特殊道具)
 *  4: Monster
 *  
 *  5: specialChapter 特殊章节剧情或者音效触发
 */

public static class MapData {

	static MapObj firstWave = null;
	// static int[] bedRoom;

	public static void initMapData() {
		// 15 x 17 room
		int[] firstWaveMapData = {
			0, 0, 0, 0, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, -1, 0, 4, 3, 4, -1, -1, -1, 0, 0, 0,
			0, 0, 0, 0, -1, -2, -1, -1, 0, 0, 0, -1, 0, 0, 0,
			0, 0, 0, 0, -1, 0, -1, -1, -1, -1, -2, -1, -1, -1, 0,
			0, -1, -1, -1, -1, 0, -1, -1, -1, -1, 0, 0, 0, -1, 0,
			-1, -1, 0, 0, 0, 0, -2, -2, 0, -1, -1, -1, 0, -1, 0,
			-1, 0, -2, -1, -1, -1, -1, -1, -1, -1, -2, 0, 0, -1, -1,
			-1, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1, 0, 0, -1, 
			-1, -1, -1, -1, 0, -1, 0, 0, 0, -2, 0, 0, 0, 0, -1, 
			0, 0, 0, -1, -2, -1, -1, 0, -1, -1, -1, -1, -1, -1, -1, 
			0, 0, 0, -1, 0, -1, -1, 0, -1, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, -1, 0, 0, 0, -2, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, -1, -1, -1, -1, 0, -1, -1, -1, -1, 0, 0, 0,
			0, 0, 0, 0, 0, 0, -1, -2, 5, 0, 5, -1, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, -1, -1, -1, -1, 0, -1, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 1, -1, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, 0, 0, 0
		};
		firstWave = new MapObj(15, 17, firstWaveMapData, MapNameData.FIRST_MAP);
	}

	public static MapObj getFirstWave() {
		if (firstWave == null) {
			Debug.LogWarning("MapData lost: bedRoom");
			return null;
		}
		return firstWave;
	}
}
