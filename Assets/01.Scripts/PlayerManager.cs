using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : MonoBehaviour
{
    private readonly string ApiUrl = "http://localhost:5162/api/Player/";

    public void GetPlayers(Action<PlayerDataWrapper> callBack = null)
    {
        StartCoroutine(GetPlayersRoutine(callBack));
    }

    public void AddPlayer(string playerName, int id)
    {
        StartCoroutine(AddPlayerRoutine(playerName, id));
    }
    
    // Get
    private IEnumerator GetPlayersRoutine(Action<PlayerDataWrapper> callBack)
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

            if (!string.IsNullOrEmpty(webRequest.downloadHandler.text))
            {
                var data = JsonUtility.FromJson<PlayerDataWrapper>($"{{ \"playerData\" : {webRequest.downloadHandler.text} }}");
                callBack?.Invoke(data);
            }
        }
    }

    // Post
    private IEnumerator AddPlayerRoutine(string playerName, int score)
    {
        var newPlayerData = new PlayerData { name = playerName, score = score };
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
