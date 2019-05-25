using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundle : Editor {
	[MenuItem("AssetBundle/Test")]
	public static void BuildAssetBundle() {
		System.String spriteSrcPath = "Assets/Resources";
		if (Directory.Exists(spriteSrcPath) == false) {
			Directory.CreateDirectory(spriteSrcPath);
		}
		AssetDatabase.Refresh();
		BuildPipeline.BuildAssetBundles(spriteSrcPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android); ;
	}
	
}
