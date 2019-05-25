using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	private Vector2 _pos;
	private MoveCtrl _moveCtrl = null;
	private utils _utils = null;
	private PlayerState _state = PlayerState.None;

	private Action runAction;

	public int life = 1;
	public Slider sliderBar;

	public void Init(DrawMap mapManager) {
		Parry.getInstance().init(this);
		PlayerAction.getInstance().init();
		_utils = new utils(mapManager);

		_moveCtrl = this.GetComponent<MoveCtrl>();
		_pos = _utils.getPlayerPosition();
		_moveCtrl.setPosition(_pos);
		_moveCtrl.setOwner(this);
		_moveCtrl.setGestureToMove();
		setState(PlayerState.Idle);
	}

	// 控制位置,返回值是为了方便 moveCtrl 做同步
	public bool checkPositionState(int x, int y) {
		bool ableTomove = actionType(_utils.getGridData(x, y));
		if (ableTomove) PlayerAction.getInstance().onWalking(this, runAction);
		return ableTomove;
	}

	private bool actionType(int data) {
		switch (data) {
			// 因为初始的数据我们没有做变动,因此 1 也在数据读取的范围之内
			case 1:
			case 0:
				runAction = null;
				return true;
			case -1:
				PlayerAction.getInstance().onWall(this);
				return false;
			case -2:
				runAction = PlayerAction.getInstance().onObstacle;
				return true;
			case 3:
				runAction = PlayerAction.getInstance().onExit;
				return true;
			case 4:
				runAction = PlayerAction.getInstance().onMonster;
				return true;
			default:
				Debug.Log("Can't find the special index" + data);
				return false;
		}
	}

	private void Update() {
		Parry.getInstance().timer();
		if (_moveCtrl)
			_moveCtrl.timer();
	}

	// 这里可以用于控制 player state 的修改，避免相同的 keybord 监听引起的效果冲突
	//private void LateUpdate() {}

	// 因为不想 moveCtrl 与 parry 有关联，保持不同组件间的独立关系
	public void switchGestureToMove() {
		if (_moveCtrl != null) 
			_moveCtrl.setGestureToMove();
	}

	public void setPosition(Vector2 pos) {
		_pos = pos;
		_utils.updatePlayerData((int)_pos.x, (int)_pos.y);
	}

	public Vector2 getPosition() {
		return new Vector2(_pos.x, _pos.y);
	}

	public PlayerState getState() {
		return _state;
	}

	public void setState(PlayerState value) {
		Debug.Log("Player State changed: " + value);
		if (GameManager.getInstance().getGameState() != GameState.GameOver)
			_state = value;
	}

	public void setLife(int value) {
		life += value;

		// 判断游戏是否结束
		if (life <= 0) {
			life = 0;
			setState(PlayerState.Dead);
			GameManager.getInstance().GameOver();
		}
	}

	public int getLife() {
		return life;
	}

}
