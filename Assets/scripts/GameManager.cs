using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public bool isDevelop = true;
	public bool isShow = true;
	#region room map
	private DrawMap _drawMap;
	public Player player;
	public GameObject audioManager;			// 便于各个 audioCtrl 查找控制器，因为 ctrl 都绑定在同一个对象身上了。 ctrl 目前的作用就是通过 editor 绑定 audio。
	public GameObject canvas;
	#endregion
	// 当前房间地图
	private static GameManager _instance = null;
	private GameState _state = GameState.Ready;
	private utils _utils = null;

	private void Awake() {
		_instance = this;
		CanvasCtrl.adjustCanvasScale(canvas);
	}

	public static GameManager getInstance() {
		if (_instance == null) {
			_instance = new GameManager();
			return _instance;
		}
		return _instance;
	}

	// Use this for initialization
	void Start () {
		_drawMap = this.GetComponent<DrawMap>();
		MapData.initMapData();
		initAudioCtrl();
		DrawMap();
		if (GameManagerGlobalData.isFirstTimeEnter) {
			setGameState(GameState.Plot);
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.CHAPTER_01_1);
			PlayerAudioCtrl.getInstance().play(PlayerAudioData.SLIDER_TO_FRONT_TIPS, initGameState);
			GameManagerGlobalData.isFirstTimeEnter = false;
		}
		else {
			initGameState();
		}
	}

	// 绘制 map 地图
	public void DrawMap() {
		_drawMap.Init();
		_utils = new utils(_drawMap);
		_drawMap.setMap(MapData.getFirstWave(), _utils);
		_drawMap.draw();
	}
	// 初始化游戏与角色状态
	private void initGameState() {
		player.Init();
		_state = GameState.Playing;
		// 因为 gesturectrl 会有数据保留，因此这句属于针对性测试代码
		// GestureCtrl.getInstance().preInit();
	}
	// init audio
	private void initAudioCtrl() {
		AudioPlayCtrl.init(audioManager);
		PlayerAudioCtrl.init(audioManager);
		BackgroundAudioCtrl.init(audioManager);
		ObstacleAudioCtrl.init(audioManager);
		// close contain feature when in the runtime.
		if (PlatformUtils.isTouchUser()) {
			isDevelop = false;
		}
	}

	private void Update() {
		// 不需要初始化，因为 StartScene 已经初始化过的， 两种情况下对方无法移动
		if (_state == GameState.Plot || _state == GameState.GameOver && player.getState() == PlayerState.Dead) {
			GestureCtrl.getInstance().timer();
		}
	}

	// 游戏结束
	public void GameOver() {
		_state = GameState.GameOver;
		if (player.getState() != PlayerState.Dead) {
			Success();
		}
		else {
			Failed();
		}
	}
	// 游戏胜利
	private void Success() {
		Debug.Log("Game Over Success!");
		SceneManager.LoadScene("StartScene");
	}
	// 游戏失败
	private void Failed() {
		Debug.Log("Game Over Failed!");
		// 手势监听，判断游戏结束后用户操作，但是由于目前策划功能不完善，所以这个部分默认是重新开始游戏
		BackgroundAudioCtrl.getInstance().play(BackgroundAudioData.GAME_OVER);
		BackgroundAudioCtrl.getInstance().play(BackgroundAudioData.NONE);
		switchGestureToGameOver();
		PlayerAudioCtrl.getInstance().play(PlayerAudioData.RESTART_GAME_TIPS);
	}

	private void switchGestureToGameOver() {
		GestureCtrl.getInstance().toCenterGesture = RestartGame;
		GestureCtrl.getInstance().toLeftGesture = null;
		GestureCtrl.getInstance().toRightGesture = null;
		GestureCtrl.getInstance().toFrontGesture = null;
		GestureCtrl.getInstance().toBackGesture = null;
	}

	// 重新开始游戏
	static void RestartGame () {
		SceneManager.LoadScene("MainScene");
	}
	
	public GameState getGameState() {
		return _state;
	}

	public void setGameState(GameState state) {
		Debug.Log("GameState Changed: " + state);
		_state = state;
		if (_state == GameState.Plot) {
			// 方便跳过剧情，通常用于测试
			GestureCtrl.getInstance().toCenterGesture = AudioPlayCtrl.getInstance().stopEffect;
		}
		else if (_state == GameState.Playing) {
			GestureCtrl.getInstance().toCenterGesture = null;
		}
	}
	// 数据隔离
	public MapObj getCurMap() {
		return _drawMap.getCurMap();
	}
}
