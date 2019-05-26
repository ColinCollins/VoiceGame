using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioData {
	public const string STEP_CLIP = "step";							// 走路
	public const string HURT_CLIP = "hurt";                         // 受伤
	public const string PARRY_CLIP = "parry";						// 格挡
	public const string SWING_CLIP = "swing";                       // 挥剑
	public const string SWORD_WILL_BREAK_CLIP = "willBreak";        // 播放人声，这把剑要撑不住了
	public const string ESCAPE_CLIP = "escape";                     // 逃过一劫
	public const string HAVE_TO_PARRY_CLIP = "haveToParry";         // 不得不挡住
	public const string IS_WALL_CLIP = "isWall";                    // 感叹撞到墙 // 添加后续
	public const string REST_IN_PEACE_CLIP = "restInPeace";         // 安息吧
	public const string HIT_WALL_CLIP = "wall";                     // 撞墙
	public const string SWITCH_OBSTACLE_CLIP = "chair";             // 障碍物开启
	public const string EXCITATION_CLIP = "excitation";             // 初次失败后的激励
	public const string OPEN_DOOR_CLIP = "openDoor";                // 通关门
	public const string CLOSE_DOOR_CLIP = "closeDoor";              // 关闭门
	public const string RESTART_GAME_TIPS = "restartGameTips";      // 当死亡时重生音效。暂时的，目前缺少死亡提示音
	public const string REFUSE_BACK_TIPS = "refuseBackTips";		// 阻止后退

	// Chapter
	public const string CHAPTER_01_1 = "chapter01_1";
	public const string CHAPTER_01_2 = "chapter01_2";

	// tutorial
	public const string PARRY_TO_FRONT_TIPS = "parryFrontTips";
	public const string PARRY_TO_LEFT_TIPS = "parryLeftTips";
	public const string SLIDER_TO_FRONT_TIPS = "sliderToFrontTips";
	public const string SLIDER_TO_LEFT_TIPS = "sliderToLeftTips";
}
