using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Services.Lobbies.Models;
using Unity.Netcode;
using System.Linq;
using UnityEngine.SceneManagement;

public class RoomScreen : MonoBehaviour
{
    [SerializeField] private LobbyPlayerPanel _playerPanelPrefab;
    [SerializeField] private Transform _playerPanelParent;
    [SerializeField] private TMP_Text _waitingText;
    [SerializeField] private GameObject _startBTN, _readyBTN;

    private readonly List<LobbyPlayerPanel> _playerPanels = new();
    private bool _allReady;
    private bool _ready;

    public static event Action StartPressed;

    private void OnEnable() 
    {
        foreach(Transform child in _playerPanelParent) Destroy(child.gameObject);
        _playerPanels.Clear();

        LobbyUIManager.LobbyPlayersUpdated += NetworkLobbyPlayersUpdated;
        LobbyAndRelayManager.CurrentLobbyRefreshed += OnCurrentLobbyRefreshed;
        _startBTN.SetActive(false);
        _readyBTN.SetActive(false);

        _ready = false;
    }

    private void OnDisable() 
    {
        LobbyUIManager.LobbyPlayersUpdated -= NetworkLobbyPlayersUpdated;
        LobbyAndRelayManager.CurrentLobbyRefreshed -= OnCurrentLobbyRefreshed;
    }
 
    public void OnLeaveLobby() 
    {
        LobbyLeft?.Invoke();
    }

    public static event Action LobbyLeft;

    private void NetworkLobbyPlayersUpdated(Dictionary<ulong, bool> players)
    {
          var allActivePlayerIds = players.Keys;

        // Remove all inactive panels
        var toDestroy = _playerPanels.Where(p => !allActivePlayerIds.Contains(p.PlayerId)).ToList();
        foreach (var panel in toDestroy) {
            _playerPanels.Remove(panel);
            Destroy(panel.gameObject);
        }

        foreach (var player in players) {
            var currentPanel = _playerPanels.FirstOrDefault(p => p.PlayerId == player.Key);
            if (currentPanel != null) {
                if (player.Value) currentPanel.SetReady();
            }
            else {
                var panel = Instantiate(_playerPanelPrefab, _playerPanelParent);
                panel.Init(player.Key);
                _playerPanels.Add(panel);
            }
        }

        _startBTN.SetActive(NetworkManager.Singleton.IsHost && players.All(p => p.Value));
        _readyBTN.SetActive(!_ready);
    }
    private void OnCurrentLobbyRefreshed(Lobby lobby)
    {
        _waitingText.text = $"Waiting players {lobby.Players.Count}/{lobby.MaxPlayers}";
    }

    public void OnReadyClicked()
    {
        _readyBTN.SetActive(false);
        _ready = true;
    }

    public void OnStartClicked() 
    {
        StartPressed?.Invoke();
    }
}
