using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VivoxUnity;

public class LobbyScreenUI : MonoBehaviour
{
    VivoxVoiceManager vivoxVoiceManager;

    public string LobbyChannelName = "lobbyChannel";

    EventSystem eventSystem;

    public Button logoutButton;
    public GameObject lobbyScreen;
    public GameObject connectionIndicatorDot;
    public GameObject connectionIndicatorText;

    Image image_connectionIndicatorDot;
    Text text_connectionIndicatorDot;

    #region Unity Callbacks

    private void Awake()
    {
        vivoxVoiceManager = VivoxVoiceManager.Instance;

        vivoxVoiceManager.OnUserLoggedInEvent += OnUserLoggedIn;
        vivoxVoiceManager.OnUserLoggedOutEvent += OnUserLoggedOut;
        vivoxVoiceManager.OnRecoveryStateChangedEvent += OnRecoveryStateChanged;

        eventSystem = EventSystem.current;
        if (!eventSystem)
            Debug.LogError("Unable to find EventSystem object.");

        image_connectionIndicatorDot = connectionIndicatorDot.GetComponent<Image>();
        if (!image_connectionIndicatorDot)
            Debug.LogError("Unable to find ConnectionIndicatorDot Image object.");

        text_connectionIndicatorDot = connectionIndicatorText.GetComponent<Text>();
        if (!text_connectionIndicatorDot)
            Debug.LogError("Unable to find ConnectionIndicatorText Text object.");

        logoutButton.onClick.AddListener(() => { LogoutOfVivoxService(); });

        if (vivoxVoiceManager.LoginState == LoginState.LoggedIn)
        {
            OnUserLoggedIn();
        }
        else
        {
            OnUserLoggedOut();
        }
    }

    private void OnDestroy()
    {
        vivoxVoiceManager.OnUserLoggedInEvent -= OnUserLoggedIn;
        vivoxVoiceManager.OnUserLoggedOutEvent -= OnUserLoggedOut;
        vivoxVoiceManager.OnParticipantAddedEvent -= VivoxVoiceManager_OnParticipantAddedEvent;
        vivoxVoiceManager.OnRecoveryStateChangedEvent -= OnRecoveryStateChanged;

        logoutButton.onClick.RemoveAllListeners();
    }

    #endregion

    // [중요] 로비 채널 참가 함수
    void JoinLobbyChannel()
    {
        // [수정 금지] 추가된 참가자가 처리할 부분
        vivoxVoiceManager.OnParticipantAddedEvent += VivoxVoiceManager_OnParticipantAddedEvent;
        vivoxVoiceManager.JoinChannel(LobbyChannelName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.TextAndAudio);
    }

    void LogoutOfVivoxService()
    {
        logoutButton.interactable = false;

        vivoxVoiceManager.DisconnectAllChannels();

        vivoxVoiceManager.Logout();
    }

    #region Vivox Callbacks

    void VivoxVoiceManager_OnParticipantAddedEvent(string username, ChannelId channel, IParticipant participant)
    {
        if (channel.Name == LobbyChannelName && participant.IsSelf)
        {
            // if joined the lobby channel and we're not hosting a match
            // we should request invites from hosts
        }
    }

    void OnUserLoggedIn()
    {
        lobbyScreen.SetActive(true);
        logoutButton.interactable = true;
        eventSystem.SetSelectedGameObject(logoutButton.gameObject, null);

        var lobbychannel = vivoxVoiceManager.ActiveChannels.FirstOrDefault(ac => ac.Channel.Name == LobbyChannelName);
        if ((vivoxVoiceManager && vivoxVoiceManager.ActiveChannels.Count == 0) || lobbychannel == null)
        {
            JoinLobbyChannel();
        }
        else
        {
            if (lobbychannel.AudioState == ConnectionState.Disconnected)
            {
                // Ask for hosts since we're already in the channel and part added won't be triggered.

                lobbychannel.BeginSetAudioConnected(true, true, ar =>
                {
                    Debug.Log("Now transmitting into lobby channel");
                });
            }

        }
    }

    void OnUserLoggedOut()
    {
        vivoxVoiceManager.DisconnectAllChannels();

        lobbyScreen.SetActive(false);
    }

    // [중요] 커넨션 상태에 따른 인디케이터 이미지 컬러 변경 + 텍스트 변경
    void OnRecoveryStateChanged(ConnectionRecoveryState recoveryState)
    {
        Color indicatorColor;
        switch (recoveryState)
        {
            case ConnectionRecoveryState.Connected:
                indicatorColor = Color.green;
                break;
            case ConnectionRecoveryState.Disconnected:
                indicatorColor = Color.red;
                break;
            case ConnectionRecoveryState.FailedToRecover:
                indicatorColor = Color.black;
                break;
            case ConnectionRecoveryState.Recovered:
                indicatorColor = Color.green;
                break;
            case ConnectionRecoveryState.Recovering:
                indicatorColor = Color.yellow;
                break;
            default:
                indicatorColor = Color.white;
                break;
        }
        image_connectionIndicatorDot.color = indicatorColor;
        text_connectionIndicatorDot.text = recoveryState.ToString();
    }

    #endregion
}