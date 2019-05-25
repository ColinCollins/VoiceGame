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
 */

public static class MapData {

	static MapObj bedRoom = null;
	// static int[] bedRoom;

	public static void initMapData() {
		// 15 x 17 room
		int[] bedMapData = {
			0, 0, 0, 0, 0, 0, -1, 3, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, -1, 4, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, -1, -1, -1, 0,
			0, 0, 0, 0, 0, 0, -1, 0, 0, 0, -2, 0, 0, -1, 0,
			0, -1, -1, -1, -1, -1, -1, 0, -1, -1, -1, -1, 0, -1, 0,
			-1, -1, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1, -2, -1, 0,
			-1, 0, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, -1, -1,
			-1, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1, 0, 0, -1,
			-1, -1, -1, -1, 0, -1, 0, 0, 0, 0, 0, 0, 0, 4, -1,
			0, 0, 0, -1, -2, -1, -1, -2, -1, -1, -1, -1, -1, -1, -1,
			0, 0, 0, -1, 0, -1, -1, 0, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, -1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, -1, -1, -1, -1, -2, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, -1, 1, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, -1, -1, -1, 0, 0, 0, 0, 0, 0
		};
		bedRoom = new MapObj(15, 17, bedMapData);

	}

	public static MapObj getBedRoom() {
		if (bedRoom == null) {
			Debug.LogWarning("MapData lost: bedRoom");
			return null;
		}
		return bedRoom;
	}
}
