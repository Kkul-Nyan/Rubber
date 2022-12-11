using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Vivox;
using VivoxUnity;

public class LobbyUIManager : NetworkBehaviour
{
    [SerializeField] private MainLobbyScreen _mainLobbyScreen;
    [SerializeField] private CreateLobbyScreen _createScreen;
    [SerializeField] private RoomScreen _roomScreen;
    public GameObject PL;

    string playerID = Authentication.PlayerId;

    void Start()
    {
        _mainLobbyScreen.gameObject.SetActive(true);
        _createScreen.gameObject.SetActive(false);
        _roomScreen.gameObject.SetActive(false);

        CreateLobbyScreen.LobbyCreated += CreateLobby;
        LobbyRoomPanel.LobbySelected += OnLobbySelected;
        RoomScreen.LobbyLeft += OnLobbyLeft;
        RoomScreen.StartPressed += OnGameStart;
        
        NetworkObject.DestroyWithScene = true;
    }

    #region MainLobby
    private async void OnLobbySelected(Lobby lobby) {
;
        try
        {
            await LobbyAndRelayManager.JoinLobbyWithAllocation(lobby.Id);
            Destroy(PL);
            LobbyAndRelayManager.isConnect = true;
            _mainLobbyScreen.gameObject.SetActive(false);
            _roomScreen.gameObject.SetActive(true);

            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    
}
    #endregion
    #region CreateLobby

    private async void CreateLobby(LobbyData data)
    {
        //Lobby Data = data;
        await LobbyAndRelayManager.CreateLobbyWithAllocation(data);
        ChatManager.JoinChannel(data);

        Destroy(PL);
        
        LobbyAndRelayManager.isConnect = true;
        _createScreen.gameObject.SetActive(false);
        _roomScreen.gameObject.SetActive(true);

        // Starting the host immediately will keep the relay server alive
        NetworkManager.Singleton.StartHost();

    }
    #endregion
    #region Room

    private readonly Dictionary<ulong, bool> _playersInLobby = new();
    public static event Action<Dictionary<ulong, bool>>LobbyPlayersUpdated;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            _playersInLobby.Add(NetworkManager.Singleton.LocalClientId, false);
            UpdateInterface();
        }
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    private void UpdateInterface()
    {
        LobbyPlayersUpdated?.Invoke(_playersInLobby);
    }
    
    private void OnClientConnectedCallback(ulong playerId)
    {
        if(!IsServer) return;

        if(!_playersInLobby.ContainsKey(playerId)) _playersInLobby.Add(playerId,false);

        PropagateToClients();
    }

    private void OnClientDisconnectCallback(ulong playerId)
    {
        if(IsServer)
        {
            if(_playersInLobby.ContainsKey(playerId)) _playersInLobby.Remove(playerId);

            RemovePlayerClientRpc(playerId);

            UpdateInterface();
        }
        else
        {
            _roomScreen.gameObject.SetActive(false);
            _mainLobbyScreen.gameObject.SetActive(true);
            OnLobbyLeft();

        }
    }

    private void PropagateToClients()
    {
        foreach(var player in _playersInLobby)
        {
            UpdatePlayerClientRpc(player.Key, player.Value);
        }
    }

    [ClientRpc]
    private void UpdatePlayerClientRpc(ulong clientId, bool isReady)
    {
        if(IsServer) return;
        if(!_playersInLobby.ContainsKey(clientId)) _playersInLobby.Add(clientId, isReady);
        else _playersInLobby[clientId] = isReady;
        UpdateInterface();
    }
    [ClientRpc]
    private void RemovePlayerClientRpc(ulong clientId)
    {
        if(IsServer) return;

        if(_playersInLobby.ContainsKey(clientId)) _playersInLobby.Remove(clientId);
        UpdateInterface();
    }

    private async void OnLobbyLeft()
    {
        _playersInLobby.Clear();
        NetworkManager.Singleton.Shutdown();
        LobbyAndRelayManager.isConnect = false;
        await LobbyAndRelayManager.LeaveLobby();
    }

    public void OnReadyClicked()
    {
        SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetReadyServerRpc(ulong playerId)
    {
        _playersInLobby[playerId] =true;
        PropagateToClients();
        UpdateInterface();
    }

    public override void OnDestroy() 
    {
     
        base.OnDestroy();
        CreateLobbyScreen.LobbyCreated -= CreateLobby;
        LobbyRoomPanel.LobbySelected -= OnLobbySelected;
        RoomScreen.LobbyLeft -= OnLobbyLeft;
        RoomScreen.StartPressed -= OnGameStart;
        
        // We only care about this during lobby
        if (NetworkManager.Singleton != null) {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            }
    }

    private async void OnGameStart() 
    {
        await LobbyAndRelayManager.LockLobby();
        NetworkManager.Singleton.SceneManager.LoadScene("Hk.Map", LoadSceneMode.Single);
    }
    
      
    
}

    #endregion


