using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime;
using UnityEngine;
using VivoxUnity;
using Unity.Services.Core;
using Unity.Services.Vivox;
#if AUTH_PACKAGE_PRESENT
using Unity.Services.Authentication;
#endif


public class VivoxManager : MonoBehaviour
{
    #region Enums

    /// <summary>
    /// Defines properties that can change. Used by the functions that subscribe to the OnAfterTYPEValueUpdated functions.
    /// </summary>
    public enum ChangedProperty { None, Speaking, Typing, Muted }
    public enum ChatCapability { TextOnly, AudioOnly, TextAndAudio };

    #endregion

    #region Delegates/Events

    public delegate void ParticipantValueChangedHandler(string username, ChannelId channel, bool value);
    // 음성 감지에 대한 이벤트
    public event ParticipantValueChangedHandler OnSpeechDetectedEvent;

    public delegate void ParticipantValueUpdatedHandler(string username, ChannelId channel, double value);
    // 음량 변화에 대한 이벤트
    public event ParticipantValueUpdatedHandler OnAudioEnergyChangedEvent;

    public delegate void ParticipantStatusChangedHandler(string username, ChannelId channel, IParticipant participant);
    // 추가된 참가자에 대한 이벤트
    public event ParticipantStatusChangedHandler OnParticipantAddedEvent;
    // 제거된 참가자에 대한 이벤트
    public event ParticipantStatusChangedHandler OnParticipantRemovedEvent;

    public delegate void ChannelTextMessageChangedHandler(string sender, IChannelTextMessage channelTextMessage);
    public event ChannelTextMessageChangedHandler OnTextMessageLogReceivedEvent;

    public delegate void LoginStatusChangedHandler();
    // 유저 로그인 이벤트
    public event LoginStatusChangedHandler OnUserLoggedInEvent;
    // 유저 로그아웃 이벤트
    public event LoginStatusChangedHandler OnUserLoggedOutEvent;

    public delegate void RecoveryStateChangedHandler(ConnectionRecoveryState recoveryState);
    // 재활성화 상태 변경 이벤트
    public event RecoveryStateChangedHandler OnRecoveryStateChangedEvent;

    #endregion

    #region Member Variables

    static VivoxManager instance;

    //These variables should be set to the projects Vivox credentials if the authentication package is not being used
    //Credentials are available on the Vivox Developer Portal (developer.vivox.com) or the Unity Dashboard (dashboard.unity3d.com), depending on where the organization and project were made
    [SerializeField] string key;
    [SerializeField] string issuer;
    [SerializeField] string domain;
    [SerializeField] string server;

    Unity.Services.Vivox.Account account;

    VivoxUnity.Client client => VivoxService.Instance.Client;

    public LoginState LoginState { get; private set; }
    public ILoginSession LoginSession;

    public VivoxUnity.IReadOnlyDictionary<ChannelId, IChannelSession> ActiveChannels => LoginSession?.ChannelSessions;

    public IAudioDevices AudioInputDevices => client.AudioInputDevices;
    public IAudioDevices AudioOutputDevices => client.AudioOutputDevices;

    // Check to see if we're about to be destroyed.
    static object m_Lock = new object();

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static VivoxManager Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (instance == null)
                {
                    // Search for existing instance.
                    instance = (VivoxManager)FindObjectOfType(typeof(VivoxManager));

                    // Create new instance if one doesn't already exist.
                    if (instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<VivoxManager>();
                        singletonObject.name = typeof(VivoxManager).ToString() + " (Singleton)";
                    }
                }
                // Make instance persistent even if its already in the scene
                DontDestroyOnLoad(instance.gameObject);
                return instance;
            }
        }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Retrieves the first instance of a session that is transmitting. 
    /// </summary>
    public IChannelSession TransmittingSession
    {
        get
        {
            if (client == null)
                throw new NullReferenceException("client");
            return client.GetLoginSession(account).ChannelSessions.FirstOrDefault(x => x.IsTransmitting);
        }
        set
        {
            if (value != null)
            {
                client.GetLoginSession(account).SetTransmissionMode(TransmissionMode.Single, value.Channel);
            }
        }
    }

    #endregion

    #region Unity Event

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Debug.LogWarning("현재 Scene에서 다중 VivoxVoiceManager가 발견됐습니다. 하나의 VivoxVoiceManager만 존재해야 하므로 중복된 VivoxVoiceManager를 삭제합니다.");
            Destroy(this);
            return;
        }
    }

    async void Start()
    {
        var options = new InitializationOptions();
        // server, domain, issuer, key 중 하나라도 비어있다면 false 리턴
        if (CheckManualCredentials())
        {
            options.SetVivoxCredentials(server, domain, issuer, key);
        }

        await UnityServices.InitializeAsync(options);
#if AUTH_PACKAGE_PRESENT
        if(!CheckManualCredentials())
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
#endif
        // [Vivox 초기화] 자격증명, 토큰 처리
        VivoxService.Instance.Initialize();
    }

    private void OnApplicationQuit()
    {
        // Needed to add this to prevent some unsuccessful uninit, we can revisit to do better -carlo
        Client.Cleanup();
        if (client != null)
        {
            VivoxLog("Uninitializing client.");
            // Vivox 수동 토큰 해제
            client.Uninitialize();
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// 로그인 : ILoginSession.BeginLogin() ---> 사용자 로그인
    /// </summary>
    /// <param name="displayName">닉네임</param>
    public void Login(string displayName = null)
    {
        account = new Account(displayName);
        LoginSession = client.GetLoginSession(account);
        // [콜백 함수 연결] - 로그인 세션 프로퍼티
        LoginSession.PropertyChanged += OnLoginSessionPropertyChanged;
        // [로그인]
        LoginSession.BeginLogin(LoginSession.GetLoginToken(), SubscriptionMode.Accept, null, null, null, ar =>
        {
            try
            {
                LoginSession.EndLogin(ar);
            }
            catch (Exception e)
            {
                // Handle error 
                VivoxLogError(nameof(e));
                // Unbind if we failed to login.
                LoginSession.PropertyChanged -= OnLoginSessionPropertyChanged;
                return;
            }
        });
    }

    /// <summary>
    /// 로그아웃 : ILoginSession.Logout() ---> 사용자 로그아웃(Vivox SDK에서 수신하거나 수신하는 네트워크 트래픽이 없어짐)
    /// </summary>
    public void Logout()
    {
        if (LoginSession != null && LoginState != LoginState.LoggedOut && LoginState != LoginState.LoggingOut)
        {
            LoginSession.Logout();
        }
    }

    /// <summary>
    /// 채널 참여
    /// </summary>
    /// <param name="channelName">채널 이름(Vivox.Channel Class의 매개변수)</param>
    /// <param name="channelType">채널 타입</param>
    /// <param name="chatCapability">채팅 기능(IChannelSession.BeginConnect()의 매개변수)</param>
    /// <param name="transmissionSwitch">(IChannelSession.BeginConnect()의 매개변수)</param>
    /// <param name="properties">채널 프로퍼티(Vivox.Channel3DProperties Class의 매개변수)</param>
    public void JoinChannel(string channelName, ChannelType channelType, ChatCapability chatCapability, bool transmissionSwitch = true, Channel3DProperties properties = null)
    {
        if (LoginState == LoginState.LoggedIn)
        {
            Channel channel = new Channel(channelName, channelType, properties);
            // 로그인 세션 인터페이스에서 가진 채널ID를 채널 세션 인터페이스로 저장
            IChannelSession channelSession = LoginSession.GetChannelSession(channel);
            // [콜백 함수 연결] - 채널 세션 프로퍼티
            channelSession.PropertyChanged += OnChannelPropertyChanged;
            // [콜백 함수 연결] - 참가자 추가
            channelSession.Participants.AfterKeyAdded += OnParticipantAdded;
            // [콜백 함수 연결] - 참가자 제거
            channelSession.Participants.BeforeKeyRemoved += OnParticipantRemoved;
            // [콜백 함수 연결] - 참가자 상태 업데이트
            channelSession.Participants.AfterValueUpdated += OnParticipantValueUpdated;
            // [콜백 함수 연결] - 메세지 로그 전달
            channelSession.MessageLog.AfterItemAdded += OnMessageLogRecieved;
            // [채널 가입]
            channelSession.BeginConnect(chatCapability != ChatCapability.TextOnly, chatCapability != ChatCapability.AudioOnly, transmissionSwitch, channelSession.GetConnectToken(), ar =>
            {
                try
                {
                    channelSession.EndConnect(ar);
                }
                catch (Exception e)
                {
                    // Handle error 
                    VivoxLogError($"Could not connect to voice channel: {e.Message}");
                    return;
                }
            });
        }
        else
        {
            VivoxLogError("Cannot join a channel when not logged in.");
        }
    }

    /// <summary>
    /// 메세지 전송
    /// </summary>
    /// <param name="messageToSend"></param>
    /// <param name="channel"></param>
    /// <param name="applicationStanzaNamespace"></param>
    /// <param name="applicationStanzaBody"></param>
    /// <exception cref="ArgumentException"></exception>
    public void SendTextMessage(string messageToSend, ChannelId channel, string applicationStanzaNamespace = null, string applicationStanzaBody = null)
    {
        if (ChannelId.IsNullOrEmpty(channel))
        {
            throw new ArgumentException("Must provide a valid ChannelId");
        }
        if (string.IsNullOrEmpty(messageToSend))
        {
            throw new ArgumentException("Must provide a message to send");
        }

        var channelSession = LoginSession.GetChannelSession(channel);
        // [메세지 전송]
        channelSession.BeginSendText(null, messageToSend, applicationStanzaNamespace, applicationStanzaBody, ar =>
        {
            try
            {
                channelSession.EndSendText(ar);
            }
            catch (Exception e)
            {
                VivoxLog($"SendTextMessage failed with exception {e.Message}");
            }
        });
    }

    /// <summary>
    /// 활성화 중인 채널을 전부 삭제함
    /// </summary>
    public void DisconnectAllChannels()
    {
        if (ActiveChannels?.Count > 0)
        {
            foreach (var channelSession in ActiveChannels)
            {
                channelSession?.Disconnect();
            }
        }
    }

    /// <summary>
    /// [중요] server, domain, issuer, key필드 확인 함수
    /// 최초 연결 때 사용
    /// </summary>
    /// <returns></returns>
    bool CheckManualCredentials()
    {
        return !(string.IsNullOrEmpty(key) && string.IsNullOrEmpty(issuer) && string.IsNullOrEmpty(domain) && string.IsNullOrEmpty(server));
    }

    #region Vivox Callbacks

    /// <summary>
    /// [메세지 로그 전달 콜백 함수]
    /// JoinChannel() 함수에서 이벤트 연결
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="textMessage"></param>
    void OnMessageLogRecieved(object sender, QueueItemAddedEventArgs<IChannelTextMessage> textMessage)
    {
        ValidateArgs(new object[] { sender, textMessage });

        IChannelTextMessage channelTextMessage = textMessage.Value;
        VivoxLog(channelTextMessage.Message);
        OnTextMessageLogReceivedEvent?.Invoke(channelTextMessage.Sender.DisplayName, channelTextMessage);
    }

    /// <summary>
    /// [로그인 세션 프로퍼티 변경 콜백 함수]
    /// Login() 함수에서 이벤트 연결
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="propertyChangedEventArgs"></param>
    void OnLoginSessionPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        if (propertyChangedEventArgs.PropertyName == "RecoveryState")
        {
            OnRecoveryStateChangedEvent?.Invoke(LoginSession.RecoveryState);
            return;
        }
        if (propertyChangedEventArgs.PropertyName != "State")
        {
            return;
        }

        var loginSession = (ILoginSession)sender;
        LoginState = loginSession.State;

        VivoxLog("Detecting login session change");

        switch (LoginState)
        {
            case LoginState.LoggingIn:
                {
                    VivoxLog("Logging in");
                    break;
                }
            case LoginState.LoggedIn:
                {
                    VivoxLog("Connected to voice server and logged in.");
                    OnUserLoggedInEvent?.Invoke();
                    break;
                }
            case LoginState.LoggingOut:
                {
                    VivoxLog("Logging out");
                    break;
                }
            case LoginState.LoggedOut:
                {
                    VivoxLog("Logged out");
                    OnUserLoggedOutEvent?.Invoke();
                    LoginSession.PropertyChanged -= OnLoginSessionPropertyChanged;
                    break;
                }
            default:
                break;
        }
    }

    /// <summary>
    /// [채널 세션 프로퍼티 콜백 함수]
    /// JoinChannel() 함수에서 이벤트 연결
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="propertyChangedEventArgs"></param>
    void OnChannelPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        ValidateArgs(new object[] { sender, propertyChangedEventArgs });

        //if (_client == null)
        //    throw new InvalidClient("Invalid client.");
        var channelSession = (IChannelSession)sender;

        // IF the channel has removed audio, make sure all the VAD indicators aren't showing speaking.
        if (propertyChangedEventArgs.PropertyName == "AudioState" && channelSession.AudioState == ConnectionState.Disconnected)
        {
            VivoxLog($"Audio disconnected from: {channelSession.Key.Name}");

            foreach (var participant in channelSession.Participants)
            {
                OnSpeechDetectedEvent?.Invoke(participant.Account.Name, channelSession.Channel, false);
            }
        }

        // IF the channel has fully disconnected, unsubscribe and remove.
        if ((propertyChangedEventArgs.PropertyName == "AudioState" || propertyChangedEventArgs.PropertyName == "TextState") &&
            channelSession.AudioState == ConnectionState.Disconnected &&
            channelSession.TextState == ConnectionState.Disconnected)
        {
            VivoxLog($"Unsubscribing from: {channelSession.Key.Name}");
            // Now that we are disconnected, unsubscribe.
            channelSession.PropertyChanged -= OnChannelPropertyChanged;
            // [이벤트 제거] - 사용자 참가
            channelSession.Participants.AfterKeyAdded -= OnParticipantAdded;
            // [이벤트 제거] - 사용자 제거
            channelSession.Participants.BeforeKeyRemoved -= OnParticipantRemoved;
            // [이벤트 제거] - 사용자 상태 업데이트
            channelSession.Participants.AfterValueUpdated -= OnParticipantValueUpdated;
            // [이벤트 제거] - 메세지 로그 전달
            channelSession.MessageLog.AfterItemAdded -= OnMessageLogRecieved;

            // Remove session.
            var user = client.GetLoginSession(account);
            user.DeleteChannelSession(channelSession.Channel);

        }
    }

    static void ValidateArgs(object[] objs)
    {
        foreach (var obj in objs)
        {
            if (obj == null)
                throw new ArgumentNullException(obj.GetType().ToString(), "Specify a non-null/non-empty argument.");
        }
    }

    /// <summary>
    /// [참가자 추가 콜백 함수]
    /// JoinChannel() 함수에서 연결
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="keyEventArg"></param>
    void OnParticipantAdded(object sender, KeyEventArg<string> keyEventArg)
    {
        ValidateArgs(new object[] { sender, keyEventArg });

        // INFO: sender is the dictionary that changed and trigger the event.  Need to cast it back to access it.
        var source = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;
        // Look up the participant via the key.
        var participant = source[keyEventArg.Key];
        var username = participant.Account.Name;
        var channel = participant.ParentChannelSession.Key;
        var channelSession = participant.ParentChannelSession;

        // Trigger callback
        OnParticipantAddedEvent?.Invoke(username, channel, participant);
    }

    /// <summary>
    /// [참가자 제거 콜백 함수]
    /// JoinChannel() 함수에서 연결
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="keyEventArg"></param>
    void OnParticipantRemoved(object sender, KeyEventArg<string> keyEventArg)
    {
        ValidateArgs(new object[] { sender, keyEventArg });

        // INFO: sender is the dictionary that changed and trigger the event.  Need to cast it back to access it.
        var source = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;
        // Look up the participant via the key.
        var participant = source[keyEventArg.Key];
        var username = participant.Account.Name;
        var channel = participant.ParentChannelSession.Key;
        var channelSession = participant.ParentChannelSession;

        if (participant.IsSelf)
        {
            VivoxLog($"Unsubscribing from: {channelSession.Key.Name}");
            // Now that we are disconnected, unsubscribe.
            channelSession.PropertyChanged -= OnChannelPropertyChanged;
            channelSession.Participants.AfterKeyAdded -= OnParticipantAdded;
            channelSession.Participants.BeforeKeyRemoved -= OnParticipantRemoved;
            channelSession.Participants.AfterValueUpdated -= OnParticipantValueUpdated;
            channelSession.MessageLog.AfterItemAdded -= OnMessageLogRecieved;

            // Remove session.
            var user = client.GetLoginSession(account);
            user.DeleteChannelSession(channelSession.Channel);
        }

        // Trigger callback
        OnParticipantRemovedEvent?.Invoke(username, channel, participant);
    }

    /// <summary>
    /// [참가자 상태 업데이트 콜백 함수]
    /// JoinChannel() 함수에서 연결
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="valueEventArg"></param>
    void OnParticipantValueUpdated(object sender, ValueEventArg<string, IParticipant> valueEventArg)
    {
        ValidateArgs(new object[] { sender, valueEventArg });

        var source = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;
        // Look up the participant via the key.
        var participant = source[valueEventArg.Key];

        string username = valueEventArg.Value.Account.Name;
        ChannelId channel = valueEventArg.Value.ParentChannelSession.Key;
        string property = valueEventArg.PropertyName;

        switch (property)
        {
            case "SpeechDetected":
                {
                    VivoxLog($"OnSpeechDetectedEvent: {username} in {channel}.");
                    OnSpeechDetectedEvent?.Invoke(username, channel, valueEventArg.Value.SpeechDetected);
                    break;
                }
            case "AudioEnergy":
                {
                    OnAudioEnergyChangedEvent?.Invoke(username, channel, valueEventArg.Value.AudioEnergy);
                    break;
                }
            default:
                break;
        }
    }

    #endregion Vivox Callbacks
    #endregion Methods

    #region VivoxLog

    void VivoxLog(string msg)
    {
        Debug.Log("<color=green>VivoxVoice: </color>: " + msg);
    }

    void VivoxLogError(string msg)
    {
        Debug.LogError("<color=green>VivoxVoice: </color>: " + msg);
    }

    #endregion
}