using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagingPanel : MonoBehaviour
{
    private PlayerManager _playerManager;

    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _playerContentPrefab;

    [Header("POST")] 
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TMP_InputField _scoreInputField;
    [SerializeField] private Button _postApplyBtn;

    [Header("GET")] 
    [SerializeField] private Button _getApplyBtn;

    private List<GameObject> _contentList = new List<GameObject>();

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        _postApplyBtn.onClick.AddListener(SendPostRequest);
        _getApplyBtn.onClick.AddListener(SendGetRequest);
    }

    private void SendPostRequest()
    {
        if (string.IsNullOrEmpty(_nameInputField.text) || string.IsNullOrEmpty(_scoreInputField.text))
        {
            Debug.LogWarning("값을 채우십쇼.");
            return;
        }
       
        _playerManager.AddPlayer(_nameInputField.text, int.Parse(_scoreInputField.text));
        _nameInputField.text = "";
        _scoreInputField.text = "";
    }

    private void SendGetRequest()
    {
        _playerManager.GetPlayers(UpdatePlayerData);
    }

    private void UpdatePlayerData(PlayerDataWrapper playerDataWrapper)
    {
        foreach (var content in _contentList)
        {
            Destroy(content);
        }
        
        foreach (var data in playerDataWrapper.playerData)
        {
            var dataContent = Instantiate(_playerContentPrefab, _contentParent);

            var nameText = dataContent.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var scoreText = dataContent.transform.Find("Score").GetComponent<TextMeshProUGUI>();

            nameText.text = data.name;
            scoreText.text = data.score.ToString();
            
            _contentList.Add(dataContent);
        }
    }
}
