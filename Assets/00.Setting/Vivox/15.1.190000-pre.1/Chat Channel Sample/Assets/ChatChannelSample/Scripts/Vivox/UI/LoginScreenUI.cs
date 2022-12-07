using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoginScreenUI : MonoBehaviour
{
    VivoxVoiceManager vivoxVoiceManager;

    public Button LoginButton;
    public InputField if_DisplayName;
    public GameObject LoginScreen;

    int defaultMaxStringLength = 9;
    int PermissionAskedCount = 0;

    #region Unity Callbacks

    EventSystem eventSystem;

    private void Awake()
    {
        // 하이어라키 이벤트 시스템 찾기
        eventSystem = FindObjectOfType<EventSystem>();
        // Vivox Voice Manager 찾기
        vivoxVoiceManager = VivoxVoiceManager.Instance;
        // 델리게이트와 이벤트 연결
        vivoxVoiceManager.OnUserLoggedInEvent += OnUserLoggedIn;
        vivoxVoiceManager.OnUserLoggedOutEvent += OnUserLoggedOut;

        // Mac OS X, Windows 또는 Linux, IOS, Android, STADIA 환경이 아니라면
#if !(UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_STADIA)
        DisplayNameInput.interactable = false;
#else
        // 인풋필드의 내용이 변경되면 LoginToVivoxService() 메소드 실행
        if_DisplayName.onEndEdit.AddListener((string text) => { LoginToVivoxService(); });
#endif  
        // 버튼 클릭시에도 LoginToVivoxService() 메소드 실행
        LoginButton.onClick.AddListener(() => { LoginToVivoxService(); });

        if (vivoxVoiceManager.LoginState == VivoxUnity.LoginState.LoggedIn)
        {
            OnUserLoggedIn();
            if_DisplayName.text = vivoxVoiceManager.LoginSession.Key.DisplayName;
        }
        else
        {
            OnUserLoggedOut();
            var systInfoDeviceName = String.IsNullOrWhiteSpace(SystemInfo.deviceName) == false ? SystemInfo.deviceName : Environment.MachineName;

            if_DisplayName.text = Environment.MachineName.Substring(0, Math.Min(defaultMaxStringLength, Environment.MachineName.Length));
        }
    }

    private void OnDestroy()
    {
        // 이벤트 종료
        vivoxVoiceManager.OnUserLoggedInEvent -= OnUserLoggedIn;
        vivoxVoiceManager.OnUserLoggedOutEvent -= OnUserLoggedOut;

        // 버튼 할당된 리스너 전부 삭제
        LoginButton.onClick.RemoveAllListeners();
#if UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_STADIA
        if_DisplayName.onEndEdit.RemoveAllListeners();
#endif
    }

    #endregion

    // 단순 UI 함수
    void ShowLoginUI()
    {
        LoginScreen.SetActive(true);
        LoginButton.interactable = true;
        eventSystem.SetSelectedGameObject(LoginButton.gameObject, null);
    }

    // 단순 UI 함수
    void HideLoginUI()
    {
        LoginScreen.SetActive(false);
    }

    // 중요
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
    private bool IsAndroid12AndUp()
    {
        // android12VersionCode is hardcoded because it might not be available in all versions of Android SDK
        const int android12VersionCode = 31;
        AndroidJavaClass buildVersionClass = new AndroidJavaClass("android.os.Build$VERSION");
        int buildSdkVersion = buildVersionClass.GetStatic<int>("SDK_INT");

        return buildSdkVersion >= android12VersionCode;
    }

    private string GetBluetoothConnectPermissionCode()
    {
        if (IsAndroid12AndUp())
        {
            // UnityEngine.Android.Permission does not contain the BLUETOOTH_CONNECT permission, fetch it from Android
            AndroidJavaClass manifestPermissionClass = new AndroidJavaClass("android.Manifest$permission");
            string permissionCode = manifestPermissionClass.GetStatic<string>("BLUETOOTH_CONNECT");

            return permissionCode;
        }

        return "";
    }
#endif

    // [중요] 마이크 사용 권한 부여 여부 함수
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

    // [중요] 권한 요청 함수
    void AskForPermissions()
    {
        string permissionCode = Permission.Microphone;

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (PermissionAskedCount == 1 && IsAndroid12AndUp())
        {
            permissionCode = GetBluetoothConnectPermissionCode();
        }
#endif
        PermissionAskedCount++;
        Permission.RequestUserPermission(permissionCode);
    }

    // [중요] 권한 거부 여부 함수
    bool IsPermissionsDenied()
    {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission
        if (IsAndroid12AndUp())
        {
            return PermissionAskedCount == 2;
        }
#endif
        return PermissionAskedCount == 1;
    }

    // [중요]
    void LoginToVivoxService()
    {
        if (IsMicPermissionGranted())
        {
            // 마이크로폰 사용 승인
            LoginToVivox();
        }
        else
        {
            // 필요한 권한이 없음
            // 사용자가 사용 권한을 거부한 경우 사용 권한을 요청하거나 기능을 사용하지 않고 계속 진행
            if (IsPermissionsDenied())
            {
                PermissionAskedCount = 0;
                LoginToVivox();
            }
            else
            {
                AskForPermissions();
            }
        }
    }

    // [중요]
    void LoginToVivox()
    {
        // 2회 동작 금지하기 위해 버튼 비활성화
        LoginButton.interactable = false;
        // 인풋필드에 텍스트가 없다면(닉네임 미작성시)
        if (string.IsNullOrEmpty(if_DisplayName.text))
        {
            Debug.LogError("표시될 이름을 적어주세요.");
            return;
        }
        // 인풋필드의 내용을 매개변수로 로그인 메소드 실행
        vivoxVoiceManager.Login(if_DisplayName.text);
    }

    #region Vivox Callbacks

    void OnUserLoggedIn()
    {
        // 단순 UI 함수
        HideLoginUI();
    }

    void OnUserLoggedOut()
    {
        // 단순 UI 함수
        ShowLoginUI();
    }

    #endregion
}