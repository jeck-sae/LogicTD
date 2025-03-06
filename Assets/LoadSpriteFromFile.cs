using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class LoadSpriteFromFile : MonoBehaviour
{
    [SerializeField] string textureName;
    [SerializeField] bool loadedTexture;

    private void Awake()
    {
        if (loadedTexture)
            return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        StartCoroutine(GetSpriteFromFile(sr, Path.Combine(Application.streamingAssetsPath, textureName)));
    }

    protected IEnumerator GetSpriteFromFile(SpriteRenderer r, string file)
    {
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(file);
        yield return textureRequest.SendWebRequest();

        if (textureRequest.result == UnityWebRequest.Result.ConnectionError || textureRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Could not load the texture: {file} ({textureRequest.result}) {name}", gameObject);
            yield break;
        }

        loadedTexture = true;
        var tex = DownloadHandlerTexture.GetContent(textureRequest);
        r.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2, 32);
    }
}
