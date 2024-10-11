using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private GameObject _initPanel;
    [SerializeField] private GameObject _waitPanel;

    [SerializeField] private TMP_InputField _userIdInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Button _button;

    private void Awake()
    {
        GameManager.Instance.OnSetEvent += OnSet;
    }

    private void OnSet()
    {
        _waitPanel.SetActive(false);
        _button.onClick.AddListener(OnClickButton);
    }

    private async void OnClickButton()
    {
        var gameHub = HubConnectionManager.Instance.GetHubConnection(HubType.GameHub);
        if (await gameHub.InvokeAsync<bool>("Login", _userIdInput.text, _passwordInput.text))
        {
            _initPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Login 실패");
        }
    }
}
