using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum menuState {
	NONE,
	START,
	CONTACT,
	EXIT
}

public class StartSceneCtrl : MonoBehaviour {
	public GameObject canvas;
	public GameObject audioManager;
	public GameObject background;

	private menuState _state = menuState.NONE;
	private float _elapseTime = 270f;
	private float _curTime = 0;
	private float _titleDisplayTime = 3.0f;
	private bool isFirstTimeEnterGame = true;

	private Image _title = null;
	private Image _tips = null;
	private Button _start = null;
	private Button _contact = null;
	private Button _exit = null;

	private static StartSceneCtrl _instance = null;
	public static StartSceneCtrl getInstance() {
		return _instance;
	}

	private void Awake() {
		CanvasCtrl.adjustCanvasScale(canvas);
		_instance = this;
	}
	// Use this for initialization
	void Start() {
		GestureCtrl.getInstance().preInit();
		GestureCtrl.getInstance().init();
		init();
	}

	private void init() {
		_curTime = 0;
		// 动态加载资源
		if (!ResourcesSaveUtils.getInstance().isLoaded) {
			// 开启协程,异步执行数据内容（并不是异步）
			StartCoroutine(ResourcesSaveUtils.getInstance().loadSpriteResources());
		}

		// init audio
		AudioPlayCtrl.init(audioManager);
		StartSceneAudioCtrl.init(audioManager);
		// repeat play audio
		GestureCtrl.getInstance().toLeftGesture = playSystemAudio;
		GestureCtrl.getInstance().toCenterGesture = makeSure;
		GestureCtrl.getInstance().toRightGesture = null;
		GestureCtrl.getInstance().toBackGesture = () => {
			changeChoice(false);
		};
		GestureCtrl.getInstance().toFrontGesture = () => {
			changeChoice(true);
		};
		bindingUIFeature();
		GameManagerGlobalData.resetGameGlobalData();
	}

	private void bindingUIFeature() {
		Image[] uiComps = canvas.GetComponentsInChildren<Image>();
		for (int i = 0; i < uiComps.Length; i++) {
			Transform trans = uiComps[i].transform;
			switch (trans.name) {
				case "title":
					_title = uiComps[i];
					break;
				case "tips":
					_tips = uiComps[i];
					break;
				case "start":
					_start = trans.GetComponent<Button>();
					break;
				case "contact":
					_contact = trans.GetComponent<Button>();
					break;
				case "exit":
					_exit = trans.GetComponent<Button>();
					break;
			}
		}
		_title.gameObject.SetActive(true);
		// Making title suit to screen.
		_title.rectTransform.position.Set(_title.rectTransform.position.x, _title.rectTransform.position.y * CanvasCtrl.heightScale, _title.rectTransform.position.z);

		_tips.gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update() {
		GestureCtrl.getInstance().timer();
		_curTime += Time.deltaTime;
		if (isFirstTimeEnterGame) {
			showStartAnim();
		}
		
		if (_curTime > _elapseTime) {
			StartSceneAudioCtrl.getInstance().play(StartSceneAudioData.DOUBLE_CLICK);
			playSystemAudio();
		}
	}

	private void showStartAnim() {
		Color color = Color.white;
		if (_curTime < _titleDisplayTime) {
			color.a = _curTime / _titleDisplayTime;
		}
	
		_title.color = color;
		if (_curTime > _titleDisplayTime && ResourcesSaveUtils.getInstance().isLoaded) {
			StartSceneAudioCtrl.getInstance().play(StartSceneAudioData.TIPS, () => {
				setTipsSprite(null);
			});
			setTipsSprite("tips1");
			setState(menuState.START);
			// button
			initButton();
			isFirstTimeEnterGame = false;
		}		
	}

	public void JumpToLoading() {
		SceneManager.LoadScene("LoadingScene");
	}

	public void JumpToMainScene() {
		SceneManager.LoadScene("MainScene");
	}

	private void makeSure() {
		switch (_state) {
			case menuState.START:
				JumpToLoading();
				break;
			case menuState.CONTACT:
				contactUS();
				break;
			case menuState.EXIT:
				exitApplication();
				break;
		}
	}

	private void changeChoice (bool isUp) {
		switch (_state) {
			case menuState.START:
				setState(isUp ? menuState.EXIT : menuState.CONTACT);
				break;
			case menuState.CONTACT:
				setState(isUp ? menuState.START : menuState.EXIT);
				break;
			case menuState.EXIT:
				setState(isUp ? menuState.CONTACT : menuState.START);
				break;
		}
	}

	private void playSystemAudio() {
		switch (_state) {
			case menuState.START:
				StartSceneAudioCtrl.getInstance().play(StartSceneAudioData.START);
				break;
			case menuState.CONTACT:
				StartSceneAudioCtrl.getInstance().play(StartSceneAudioData.CONTACT_OFFICE);
				break;
			case menuState.EXIT:
				StartSceneAudioCtrl.getInstance().play(StartSceneAudioData.EXIT);
				break;
		}
	}

	private void exitApplication() {
		Application.Quit();
	}

	private void contactUS() {
		StartSceneAudioCtrl.getInstance().play(StartSceneAudioData.ADDQQ, () => {
			setTipsSprite(null);
		});
		setTipsSprite("tips2");
	}

	private void setTipsSprite(string name = null) {
		if (name == null) {
			_tips.color = default(Color);
		}
		else {
			_tips.sprite = ResourcesSaveUtils.getInstance().getSpriteByName(name);
			_tips.color = Color.white;
		}
		return;
	}

	private void initButton() {
		showButton();
		_start.onClick.AddListener(JumpToLoading);
		_contact.onClick.AddListener(contactUS);
		_exit.onClick.AddListener(exitApplication);
	}

	// 显示按钮
	private void showButton() {
		Color color = Color.white;
		color.a = 1;
		_start.transform.GetComponent<Image>().color = color;
		_contact.transform.GetComponent<Image>().color = color;
		_exit.transform.GetComponent<Image>().color = color;

		_start.transform.GetComponentsInChildren<Image>()[1].color = color;
		_contact.transform.GetComponentsInChildren<Image>()[1].color = color;
		_exit.transform.GetComponentsInChildren<Image>()[1].color = color;
	}

	public void setState(menuState state) {
		Debug.Log("Start menu state change to: " + state);
		_state = state;
		playSystemAudio();
	}
	#region Test for Gesture
	//private void OnDrawGizmos() {
	//	Vector3 startPos = GestureCtrl.getInstance().getStartPos();
	//	Vector3 endPos = GestureCtrl.getInstance().getEndPos();
	//	if (startPos != endPos && startPos != Vector3.zero) {
	//		float dx = -(startPos.x - endPos.x) / 2 + Mathf.Abs(startPos.x);
	//		float dy = -(startPos.y - endPos.y) / 2 + Mathf.Abs(startPos.y);
	//		float width = Mathf.Abs(startPos.x - endPos.x);
	//		float height = Mathf.Abs(startPos.y - endPos.y);
	//		float boardY = GestureCtrl.getInstance().maxHorizontalMoveBoardY;
	//		float boardX = GestureCtrl.getInstance().maxVerticalMoveBoardX;

	//		Vector3 center = new Vector3(dx, dy, 0);
	//		// horizontal 
	//		Gizmos.color = new Color(236 / 255f, 112 / 255f, 99 / 255f, 0.8f);
	//		Gizmos.DrawCube(center, new Vector3(width, 2 * boardY));
	//		// vertical
	//		Gizmos.color = new Color(130 / 255f, 224 / 255f, 170 / 255f, 0.5f);
	//		Gizmos.DrawCube(center, new Vector3(2 * boardX, height));
	//		// normal
	//		Gizmos.color = new Color(133 / 255f, 193 / 255f, 233 / 255f, 0.3f);
	//		Gizmos.DrawCube(center, new Vector3(width, height));
	//	}
	//}
	#endregion
}
