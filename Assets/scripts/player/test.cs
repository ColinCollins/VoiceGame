using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 这是一个语法测试类
public class test : MonoBehaviour {

	private delegate void Test();
	private Test delegateTest;
	private float a = 0;
	public GameObject obj;

	private void add() {
		a += 1;
	}

	private void add2() {
		a += 1;
	}

	private int testCut() {
		Debug.Log("TestCut");
		return 1;
	}


	// Use this for initialization
	void Start () {

		delegateTest = () => {
			Debug.Log(this.testCut());
		};

		delegateTest();

		testForParam(ref obj);

		//delegateTest = add;
		//delegateTest();
		////Debug.LogWarning(a);
		//delegateTest += add2;
		//delegateTest();
		//Debug.LogWarning(a);
	}

	private void testForParam(ref GameObject _obj) {
		testForSecond(ref _obj);
	}

	private void testForSecond(ref GameObject _obj) {
		_obj = null;
		Debug.Log("`````````````````````````testForParam: " + obj);
	}

	public void JumpStartScene() {
		SceneManager.LoadScene("StartScene");
	}
}
