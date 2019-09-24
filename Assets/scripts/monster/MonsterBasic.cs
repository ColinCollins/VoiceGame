using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasic {
	private Player _owner;
	// 怪物状态切换时间
	private float _stateSwitchInterval = 5f;
	private float _curTime = 0f;

	private delegate void monsterAction();
	private monsterAction action;

	private static MonsterBasic _instance = null;
	public static MonsterBasic getInstance() {
		if (_instance == null)
		{
			_instance = new MonsterBasic();
			Debug.Log("MonsterBasic component forget to declaration itself.");
		}
		return _instance;
	}

	public void init(Player player) {
		_owner = player;
	}

	// end of Monster State
	public void killedMonset() {

	}

	// start of Monster State
	public void encounterMonster() {
		// 播放怪物声效
	}

	public void timer() {

	}

	private void switchAction() {
		// Monster Attack 状态下，用户只能防御
		// Monster Defence 状态下，用户可以进行攻击
	}

	// 切换手势功能
	private void switchGestureToCurrentState() {

	}
}
