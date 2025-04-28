using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{

    [MenuItem("Assets/Build AssetBundles")]
    static void CreateAllAssetBundles()
    {
        string assetBundleDirectory = Path.Combine(Application.streamingAssetsPath, "AssetBundles");

        if (!Directory.Exists(assetBundleDirectory))
            Directory.CreateDirectory(assetBundleDirectory);
        var allFiles = Directory.GetFiles(assetBundleDirectory);
        foreach (var file in allFiles)
            File.Delete(file);

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
