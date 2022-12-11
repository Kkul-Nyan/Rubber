using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Object = UnityEngine.Object;

public static class LobbyAndRelayManager
{

    private const int HeartbeatInterval = 15;
    private const int LobbyRefreshRate =2;

    private static UnityTransport _transport;
    private static UnityTransport Transport{
        get => _transport != null ? _transport : _transport = Object.FindObjectOfType<UnityTransport>();
        set => _transport = value;
    }

    private static Lobby _currentLobby;
    private static CancellationTokenSource _heartbeatSource, _updateLobbySource;

    public static event Action<Lobby> CurrentLobbyRefreshed;
    public static bool isConnect =  false;

    #region 
    public static void ResetStatics()
    {
        if (Transport != null)
        {
            Transport.Shutdown();
            Transport = null;
        }

        _currentLobby = null;
    }

    public static async Task<List<Lobby>> GatherLobbies() 
    {
        var options = new QueryLobbiesOptions {
            Count = 15,

            Filters = new List<QueryFilter> {
                new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                new(QueryFilter.FieldOptions.IsLocked, "0", QueryFilter.OpOptions.EQ)
            }
        };

        var allLobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
        return allLobbies.Results;
    }
    #endregion

    #region 방만들면서 동시에 릴레이서버 만들기 + 방 살아있는동안 서버에서 방못없애게 핑찍기
    public static async Task CreateLobbyWithAllocation(LobbyData data)
    {
        
        var a = await RelayService.Instance.CreateAllocationAsync(data.MaxPlayers);
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        CreateLobbyOptions option = new CreateLobbyOptions
        {
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>
            {
                {Constant.JoinKey, new DataObject(DataObject.VisibilityOptions.Member, joinCode)},
                {"Map", new DataObject(DataObject.VisibilityOptions.Public, data.Map.ToString(), DataObject.IndexOptions.S1 )},
                {"Difficuly", new DataObject(DataObject.VisibilityOptions.Public, data.Difficulty.ToString(),DataObject.IndexOptions.N2)}
            }

        };
        _currentLobby = await LobbyService.Instance.CreateLobbyAsync(data.Name,data.MaxPlayers,option);
        Transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
        
        Heartbeat();
        PeriodicallyRefreshLobby();
    }

   
    private static async void Heartbeat()
    {
        _heartbeatSource = new CancellationTokenSource();
        while(!_heartbeatSource.IsCancellationRequested && _currentLobby != null)
        {
            await Lobbies.Instance.SendHeartbeatPingAsync(_currentLobby.Id);
            await Task.Delay(HeartbeatInterval * 1000);
        }
    }

    private static async void PeriodicallyRefreshLobby()
    {
        _updateLobbySource = new CancellationTokenSource();
        await Task.Delay(LobbyRefreshRate * 1000);
        while(!_updateLobbySource.IsCancellationRequested && _currentLobby != null) 
        {
            _currentLobby = await Lobbies.Instance.GetLobbyAsync(_currentLobby.Id);
            CurrentLobbyRefreshed?.Invoke(_currentLobby);
            await Task.Delay(LobbyRefreshRate * 1000);
        }
    }
    #endregion

    #region 방참여
    public static async Task JoinLobbyWithAllocation(string lobbyId) 
    {
        _currentLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId);
        var a = await RelayService.Instance.JoinAllocationAsync(_currentLobby.Data[Constant.JoinKey].Value);

        Transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        PeriodicallyRefreshLobby();
 
    }

    #endregion

    #region 방삭제
    public static async Task LeaveLobby()
    {
        _heartbeatSource?.Cancel();
        _updateLobbySource?.Cancel();

        if(_currentLobby != null)
        {
            try
            {
                if(_currentLobby.HostId == AuthenticationService.Instance.PlayerId)
                await Lobbies.Instance.DeleteLobbyAsync(_currentLobby.Id);

                else await Lobbies.Instance.RemovePlayerAsync(_currentLobby.Id, AuthenticationService.Instance.PlayerId);
                _currentLobby = null;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
    }
    #endregion
    #region 방고정
    public static async Task LockLobby() 
    {
        try {
          await Lobbies.Instance.UpdateLobbyAsync(_currentLobby.Id, new UpdateLobbyOptions { IsLocked = true });
        }
        catch (Exception e) {
            Debug.Log($"Failed closing lobby: {e}");
        }
    }
    #endregion


}   
