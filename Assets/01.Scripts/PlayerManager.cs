using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : MonoBehaviour
{
    private readonly string ApiUrl = "https://localhost:5162/api/player";

    private void Start()
    {
        
    }

    // Get
    private IEnumerator GetPlayers()
    {
        var webRequest = UnityWebRequest.Get(ApiUrl);
        yield return webRequest.SendWebRequest();

        if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Debug.Log($"Player List: {webRequest.downloadHandler.text}");
        }
    }

    // Post
    private IEnumerator AddPlayer(string playerName, int score)
    {
        var newPlayerData = new PlayerData { Name = playerName, Score = score };
        var jsonData = JsonUtility.ToJson(newPlayerData);

        var webRequest = new UnityWebRequest(ApiUrl, "POST");
        var bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
        
        if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Debug.Log($"Player Added: {webRequest.downloadHandler.text}");
        }
    }
}
