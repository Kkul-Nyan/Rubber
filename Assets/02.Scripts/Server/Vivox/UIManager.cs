using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class UIManager : MonoBehaviour
{
    #region Singleton

    static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Search for existing instance.
                instance = (UIManager)FindObjectOfType(typeof(UIManager));

                // Create new instance if one doesn't already exist.
                if (instance == null)
                {
                    // Need to create a new GameObject to attach the singleton to.
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<UIManager>();
                    singletonObject.name = typeof(UIManager).ToString() + " (Singleton)";
                }
            }
            // Make instance persistent even if its already in the scene
            DontDestroyOnLoad(instance.gameObject);
            return instance;
        }
    }

    #endregion

    #region Fields

    [Header("UI - Main")]
    public GameObject uiMain;
    public Button btnSetting;
    public TMP_InputField tifDisplayName;
    public Button btnStart;

    [Header("UI - Setting")]
    public GameObject uiSetting;
    public Button btnMain;

    [Header("UI - Lobby")]
    public GameObject uiLobby;

    [Header("UI - Room")]
    public GameObject uiRoom;

    [Header("Values")]
    VivoxVoiceManager vivoxVoiceManager;
    EventSystem eventSystem;
    public int defaultMaxStringLength;
    int permissionAskedCount;

    #endregion

    #region Unity Event

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Debug.LogWarning("현재 Scene에서 다중 UIManager 발견됐습니다. 하나의 UIManager 존재해야 하므로 중복된 UIManager 삭제합니다.");
            Destroy(this);
            return;
        }

        eventSystem = GetComponent<EventSystem>();
        vivoxVoiceManager = VivoxVoiceManager.Instance;

        // Vivox Event Connect
        vivoxVoiceManager.OnUserLoggedInEvent += OnUserLoggedIn;
        vivoxVoiceManager.OnUserLoggedOutEvent += OnUserLoggedOut;
        // UI Listener Add
        // UI - 메인
        btnSetting.onClick.AddListener(() => { UIChangeAToB(uiMain, uiSetting); });
        //tifDisplayName.onEndEdit.AddListener((string text) => { LoginToVivoxService(); });
        btnStart.onClick.AddListener(() => { });
        // UI - 설정
        btnMain.onClick.AddListener(() => { UIChangeAToB(uiSetting, uiMain); });

        if (vivoxVoiceManager.LoginState == VivoxUnity.LoginState.LoggedIn)
        {
            OnUserLoggedIn();
            tifDisplayName.text = vivoxVoiceManager.LoginSession.Key.DisplayName;
        }
        else
        {
            OnUserLoggedOut();
            var systInfoDeviceName = String.IsNullOrWhiteSpace(SystemInfo.deviceName) == false ? SystemInfo.deviceName : Environment.MachineName;

            tifDisplayName.text = Environment.MachineName.Substring(0, Math.Min(defaultMaxStringLength, Environment.MachineName.Length));
        }

        // 사용자가 마이크로폰 사용을 승인했다면
        if (IsMicPermissionGranted())
        {
            LoginToVivox();
        }
        else
        {
            // 사용자가 마이크 사용 권한을 거부한 경우 사용 권한을 요청하거나 기능을 사용하지 않고 계속 진행합니다.
            if (IsMicPermissionsDenied())
            {
                permissionAskedCount = 0;
                LoginToVivox();
            }
            else
            {
                AskForPermissions();
            }
        }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        // Vivox Event DisConnect
        vivoxVoiceManager.OnUserLoggedInEvent -= OnUserLoggedIn;
        vivoxVoiceManager.OnUserLoggedOutEvent -= OnUserLoggedOut;
        // UI Listener Remove
        btnSetting.onClick.RemoveAllListeners();
        tifDisplayName.onEndEdit.RemoveAllListeners();
        btnStart.onClick.RemoveAllListeners();
    }

    #endregion

    #region Methods

    /// <summary>
    /// GameObject SetActive Change Method (A = false, B = true)
    /// </summary>
    /// <param name="A">false</param>
    /// <param name="B">true</param>
    public void UIChangeAToB(GameObject A, GameObject B)
    {
        A.SetActive(false);
        B.SetActive(true);
    }

    /// <summary>
    /// Vivox 로그인
    /// </summary>
    void LoginToVivox()
    {
        if (string.IsNullOrEmpty(tifDisplayName.text))
        {
            Debug.LogError("Please enter a display name.");
            return;
        }
        vivoxVoiceManager.Login(tifDisplayName.text);
    }

    /// <summary>
    /// 유저 음성 권한 사용 제안
    /// </summary>
    void AskForPermissions()
    {
        string permissionCode = Permission.Microphone;

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (PermissionAskedCount == 1 && IsAndroid12AndUp())
        {
            permissionCode = GetBluetoothConnectPermissionCode();
        }
#endif
        permissionAskedCount++;
        Permission.RequestUserPermission(permissionCode);
    }

    /// <summary>
    /// 마이크 사용 권한 부여 여부
    /// </summary>
    /// <returns></returns>
    bool IsMicPermissionGranted()
    {
        bool isGranted = Permission.HasUserAuthorizedPermission(Permission.Microphone);
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (IsAndroid12AndUp())
        {
            // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission for all features to work
            isGranted &= Permission.HasUserAuthorizedPermission(GetBluetoothConnectPermissionCode());
        }
#endif
        return isGranted;
    }

    /// <summary>
    /// 마이크 사용 권한 거부 여부
    /// </summary>
    /// <returns></returns>
    bool IsMicPermissionsDenied()
    {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission
        if (IsAndroid12AndUp())
        {
            return PermissionAskedCount == 2;
        }
#endif
        return permissionAskedCount == 1;
    }

    /// <summary>
    /// 로그인 했을 때 발생되는 콜백 함수
    /// </summary>
    void OnUserLoggedIn() { }

    /// <summary>
    /// 로그아웃 했을 때 발생되는 콜백 함수
    /// </summary>
    void OnUserLoggedOut() { }

    #endregion
}