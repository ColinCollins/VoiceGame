using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 声明共有的 delegate。 bool 是为了判断当前位置是否可以移动, 加入 nextPos 为了同步移动数据，因为 player position 数据更新在 check 之后，因此 action 函数获取不到下一个 pos 位置信息。
public delegate void Action(Player player, Vector2 nextPos);

public class PlayerAction {
	// init when the game start
	private static PlayerAction _instance = null;
	// 撞墙次数,以后可能有后续的机关功能要加入，因此用 Count 计数
	private int hitOnWallCount = 0;

	public static PlayerAction getInstance() {
		if (_instance == null) {
			_instance = new PlayerAction();
		}
		return _instance;
	}
	public void init() {
		hitOnWallCount = 0;
	}

	public void onWalking(Player player, Vector2 nextPos, Action handle = null) {
		player.setState(PlayerState.Moving);

		PlayerAudioCtrl.getInstance().play(PlayerAudioData.STEP_CLIP, () => {
			if (handle != null) handle(player, nextPos);
			else player.setState(PlayerState.Idle);
		});
		return;
	}

	public void onObstacle(Player player, Vector2 nextPos) {
		Debug.Log("Walk on Obstacle");
		//// 播放机关开启音效
		if (GameManagerGlobalData.isFirstMeetObstacle)
		{
			// 第一次进行格挡
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.SWITCH_OBSTACLE_CLIP, () =>
			{
				PlayerAudioCtrl.getInstance().play(PlayerAudioData.HAVE_TO_PARRY_CLIP, () =>
				{
					player.setState(PlayerState.Parry);
					ArrowDir _arrowDir = Parry.getInstance().startParry();
					ObstacleAudioCtrl.getInstance().playAudio(_arrowDir);
				});
			});
		}
		else if (GameManagerGlobalData.isSecondMeetObstacle)
		{
			// 第二次进行格挡
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.SWITCH_OBSTACLE_CLIP, () =>
			{
				player.setState(PlayerState.Parry);
				ArrowDir _arrowDir = Parry.getInstance().startParry();
				ObstacleAudioCtrl.getInstance().playAudio(_arrowDir);
			});
		}
		else
		{
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.SWITCH_OBSTACLE_CLIP, () =>
			{
				player.setState(PlayerState.Parry);
				ArrowDir _arrowDir = Parry.getInstance().startParry();
				ObstacleAudioCtrl.getInstance().playAudio(_arrowDir);
			});
		}
		player.setState(PlayerState.Idle);
	}

	public void onMonster(Player player, Vector2 nextPos) {
		Debug.Log("Walk on Monster");
		if (GameManagerGlobalData.isFirstMeetMonster) {

		}
		else {

		}
		player.setState(PlayerState.Idle);
	}

	public void onWall(Player player, Vector2 nextPos) {
		Debug.Log("Walk hit the Wall");
		hitOnWallCount++;
		player.setState(PlayerState.Waiting);
		if (hitOnWallCount == 1) {
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.HIT_WALL_CLIP, () => {
				PlayerAudioCtrl.getInstance().play(PlayerAudioData.IS_WALL_CLIP, () => {
					player.setState(PlayerState.Idle);
				});
			});
		}
		else {
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.HIT_WALL_CLIP, () => {
				player.setState(PlayerState.Idle);
			});
		}
	}

	public void onExit(Player player, Vector2 nextPos) {
		Debug.Log("Walk exit the room");
		player.setState(PlayerState.Waiting);
		// play audio
		PlayerAudioCtrl.getInstance().play(PlayerAudioData.OPEN_DOOR_CLIP, () => {
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.STEP_CLIP);
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.CLOSE_DOOR_CLIP, () => {
				// 返回主菜单吧
				GameManager.getInstance().GameOver();
			});
		});
	}

	public void onSpecialPlot(Player player, Vector2 nextPos) {

	}
}
