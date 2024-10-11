using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public event Action OnSetEvent = null;
    
    public bool IsSet { get; private set; }

    private void OnEnable()
    {
        Application.wantsToQuit += OnApplicationWantToQuit;
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
    }

    private void OnDisable()
    {
        Application.wantsToQuit -= OnApplicationWantToQuit;
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
    }

    private async void Start()
    {
        IsSet = false;
        await HubConnectionManager.Instance.Init();
        ServerManager.Instance.Init();
        PlayerManager.Instance.Init();
        IsSet = true;
        OnSetEvent?.Invoke();
    }

    private bool OnApplicationWantToQuit()
    {
        StartCoroutine(ApplicationQuitRoutine());
        return false;
    }
    
    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode && ServerManager.Instance.IsLogIn && !ServerManager.Instance.IsLogOut)
        {
            EditorApplication.isPlaying = true;
            StartCoroutine(ApplicationQuitRoutine());
        }
    }

    private IEnumerator ApplicationQuitRoutine()
    {
        HubConnectionManager.Instance.GetHubConnection(HubType.GameHub).SendAsync("LogOut",
            ServerManager.Instance.MyUserId);

        yield return new WaitUntil(() => ServerManager.Instance.IsLogOut);
        
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}