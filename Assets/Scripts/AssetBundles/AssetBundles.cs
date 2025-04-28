using System.IO;
using UnityEngine;

public class AssetBundles : MonoBehaviour
{
    string folderPath = "AssetBundles";
    string bundleFileName = "backgroundEffect";
    string gameObjectName = "backgroundEffect";

    private AssetBundle assetBundle;

    private void Start()
    {
        LoadBundle();
        LoadCustomUnit();
    }

    private void LoadCustomUnit()
    {
        if (assetBundle == null)
            return;

        var go = assetBundle.LoadAsset<GameObject>(gameObjectName);
        if (go)
            Instantiate(go, Vector3.zero, Quaternion.identity);
    }


    private void LoadBundle()
    {
        var combinedPath = Path.Combine(Application.streamingAssetsPath, folderPath, bundleFileName);
        if (!File.Exists(combinedPath))
        {
            Debug.LogError("AssetBundle not found: " + combinedPath);
            return;
        }

        assetBundle = AssetBundle.LoadFromFile(combinedPath);
    }
}
