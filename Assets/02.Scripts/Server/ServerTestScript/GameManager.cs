using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.UI;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text lobbyname;
    private string _lobbyId;
    private Lobby hostLobby;
    private Lobby joindLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;
    private string playerName;
    async void Start()
    {
        // Initialize unity services
        await UnityServices.InitializeAsync();
        
        //Setup events listeners
        SetupEvent();

        //Unity Login
        await SignInAnonymouslyAsync();
        
        playerName = "CodeMonkey" + UnityEngine.Random.RandomRange(10, 99);
        Debug.Log(playerName);
    }
    private void Update() {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }
    private async void HandleLobbyHeartbeat(){
        if(hostLobby != null){
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f ){
                float heartbeatTimerMax = 150;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
                Debug.Log("Success HeartBeat!");
            }
        }

    }

    private async void HandleLobbyPollForUpdates()
    {
        if(joindLobby != null){
            lobbyUpdateTimer -= Time.deltaTime;
            if(lobbyUpdateTimer < 0f ){
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joindLobby.Id);
                joindLobby = lobby;
                Debug.Log("Success HeartBeat!");
            }
        }
    }

#region  로그인
    void SetupEvent()
    {
        AuthenticationService.Instance.SignedIn += ()=>{
            Debug.Log($"PlayerID : {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"AccessToken : {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) =>{
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>{
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>{
            Debug.Log("Player session could not be refreshed and expired");
        };
    }
    async Task SignInAnonymouslyAsync()
    {
    try
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Sign in anonymously succeeded!");
        
        // Shows how to get the playerID
        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); 

    }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
    }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
#endregion
#region 로비공개방만들기

    public async void MakeLobby()
    {
        string lobbyName = lobbyname.text;
        int maxPlayers = 6;
        CreateLobbyOptions options = new CreateLobbyOptions{
            IsPrivate = false,
            Player = GetPlayer(),
            Data = new Dictionary <string, DataObject> {
                {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag")},
                {"Map", new DataObject(DataObject.VisibilityOptions.Public, "de_Dust2" ) }
            }
        };
 
        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,maxPlayers, options);
        _lobbyId =lobby.Id;
        hostLobby = lobby;
        joindLobby = hostLobby;

        PrintPlayers(hostLobby);
        Debug.Log("Success Create"+lobbyName+" "+lobby.MaxPlayers+" Id: "+lobby.Id+" Code: "+lobby.LobbyCode);
    }

    private void OnDestroy() 
    {
        Lobbies.Instance.DeleteLobbyAsync(_lobbyId);
    }
#endregion
#region 로비방 확인 및 접속
    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Count = 25, //??????????????????????
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder> {
                    new QueryOrder(false,QueryOrder.FieldOptions.Created)
                }

            };


            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found : "+ queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers+" "+lobby.Data["GameMode"].Value);
            }
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions{
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joindLobby = lobby;

            Debug.Log("JoinLobby with code : "+ lobbyCode);

            PrintPlayers(lobby);
        }
        
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void QuickJoinLobby()
    {
        try
        {
        await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private Player GetPlayer(){
        return new Player{
                Data = new Dictionary<string, PlayerDataObject>{
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                }
            };
    }

    private void PrintPlayers()
    {
        PrintPlayers(joindLobby);
    }
    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby " + lobby.Name+ " "+lobby.Data["Gamemode"].Value+ " "+ lobby.Data["Map"].Value);
        foreach(Player player in lobby.Players)
        {
            Debug.Log(player.Id + " "+ player.Data["PlayerName"].Value);
        }
    }

    private async void UpdateLobbyGameMode(string gameMode){
        try{
        hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions {
            Data = new Dictionary<string, DataObject> {
                {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
            }
        });
        joindLobby = hostLobby;

        PrintPlayers(hostLobby);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
#endregion
    private async void UpdatePlayerName(string newPlayerName){
        try{
        playerName = newPlayerName;
        await LobbyService.Instance.UpdatePlayerAsync(joindLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions{
            Data = new Dictionary<string, PlayerDataObject> {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        });
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void LeaveLobby() {
        try{
        LobbyService.Instance.RemovePlayerAsync(joindLobby.Id, AuthenticationService.Instance.PlayerId);

        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void MigrateLobbyHost(){
        try{
        hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions {
            HostId = joindLobby.Players[1].Id
            
        });
        joindLobby = hostLobby;

        PrintPlayers(hostLobby);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
