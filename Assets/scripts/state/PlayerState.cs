public enum PlayerState {
	None,
	Idle,						// 游戏进行时，人物等待状态
	Moving,						// 游戏进行时，人物移动状态
	Waiting,					// 游戏进行时，人物通过当前关卡后状态
	MonsterAttack,				// 游戏进行时，人物遇到怪物攻击状态
	MonsterDefence,             // 游戏进行时，人物遇到怪物防御状态
	Parry,						// 游戏进行时，人物格挡过程状态
	Dead						// 游戏进行时，人物死亡状态
}