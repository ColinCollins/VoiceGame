using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourcesSaveUtils {
	// streamingAsset file path in different platform
	public static readonly string PathURL =
#if UNITY_ANDROID
		"jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
		 Application.dataPath + "/StreamingAssets/";
#else
		string.Empty;
#endif


	private static ResourcesSaveUtils _instance = null;
	// 加载图片资源管理
	private List<Sprite> sprites = new List<Sprite>();
	public bool isLoaded = false;

	public static ResourcesSaveUtils getInstance() {
		if (_instance == null) {
			_instance = new ResourcesSaveUtils();
		}
		return _instance;
	}

	// Use this for initialization. Resources 在 Android 上会资源加载失败.
	public IEnumerator loadSpriteResources () {
	AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(PathURL + "test.ab");
		// wait foring request 
		yield return request;
		AssetBundle ab = request.assetBundle;
		Sprite[] sprs = ab.LoadAllAssets<Sprite>();
		for (int i = 0; i < sprs.Length; i++) {
			sprites.Add(sprs[i]);
		}
		isLoaded = true;
	}

	public Sprite getSpriteByName(System.String name) {
		return sprites.Find((sprite) => {
			return sprite.name == name;
		}); ;
	}

}
