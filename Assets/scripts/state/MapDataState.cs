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
 *  5: specialChapter 特殊章节剧情或者音效触发
 */

public enum MapDataState {
	MOVEABLE = 0,
	PLAYER = 1,
	NPC = 2,
	EXIT = 3,
	MONSTER = 4,
	SPECIAL_CHAPTER = 5,
	WALL = -1,
	OBSTACLE = -2
}