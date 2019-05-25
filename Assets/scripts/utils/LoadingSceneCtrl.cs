using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneCtrl : MonoBehaviour {
	public GameObject canvas;
	public GameObject audioManager;
	public float minLoadSpeed = 0.01f;
	public float elapseTime = 10.0f;

	public Image fillBar;
	public Image fillEyes;

	private const string LOADING_TIPS = "loading";
	private const string LOADING_COMPLETE = "loadingCompleteTips";

	private float progressValue = 0.0f;
	private bool isWaitingForTranslate = false;
	// Use this for initialization
	void Start () {
		CanvasCtrl.adjustCanvasScale(canvas);
		LoadingSceneAudioCtrl.init(audioManager);
		AudioPlayCtrl.init(audioManager);
		LoadingSceneAudioCtrl.getInstance().play(LOADING_TIPS);
		GestureCtrl.removeInstance();

		fillBar.fillAmount = 0;
		fillEyes.fillAmount = 0;
		StartCoroutine(LoadMainScene());
	}

	private void Update() {
		if (fillBar.fillAmount < progressValue) {
			fillBar.fillAmount += Time.deltaTime * minLoadSpeed;
		}
		if (fillBar.fillAmount > 0.9) {
			fillEyes.fillAmount += Time.deltaTime;
		}
	}

	IEnumerator LoadMainScene() {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
		asyncLoad.allowSceneActivation = false;
		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone) {
			if (asyncLoad.progress < 0.9f)
				progressValue = asyncLoad.progress;
			else
				progressValue = 1.0f;

			if (progressValue >= 0.9) {
				if (fillBar.fillAmount >= 0.9 && fillEyes.fillAmount >= 0.9) {
					// 避免多次执行
					if (!isWaitingForTranslate) {
						isWaitingForTranslate = true;
						LoadingSceneAudioCtrl.getInstance().play(LOADING_COMPLETE, () => {
							asyncLoad.allowSceneActivation = true;
						});
					}
				}
			}
			// 这种写法是为了避免一帧内的死循环
			yield return null;
		}
	}
}
