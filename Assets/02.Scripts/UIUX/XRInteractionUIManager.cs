using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class XRInteractionUIManager : MonoBehaviour
{
    #region Singleton

    static XRInteractionUIManager instance;
    public static XRInteractionUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Search for existing instance.
                instance = (XRInteractionUIManager)FindObjectOfType(typeof(XRInteractionUIManager));

                // Create new instance if one doesn't already exist.
                if (instance == null)
                {
                    // Need to create a new GameObject to attach the singleton to.
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<XRInteractionUIManager>();
                    singletonObject.name = typeof(XRInteractionUIManager).ToString() + " (Singleton)";
                }
            }
            // Make instance persistent even if its already in the scene
            DontDestroyOnLoad(instance.gameObject);
            return instance;
        }
    }

    #endregion

    #region Fields

    [Header("UI - Option")]
    // 사운드 : 출력 - 안내(텍스트)
    [SerializeField] TMP_Text tmpt_Output;
    // 사운드 : 출력 - 전체 볼륨(슬라이더 | 0~100 | 100)
    [SerializeField] Slider s_VolumeAll;
    [SerializeField] [Range(0, 100)] int volumeAll;
    // 사운드 : 출력 - 음악 볼륨(슬라이더 | 0~100 | 50)
    [SerializeField] Slider s_VolumeMusic;
    [SerializeField] [Range(0, 100)] int volumeMusic;
    // 사운드 : 출력 - 플레이어 볼륨(슬라이더 | 0~100 | 75)
    [SerializeField] Slider s_VolumePlayer;
    [SerializeField] [Range(0, 100)] int volumePlayer;
    // 사운드 : 출력 - 특수 효과 볼륨(슬라이더 | 0~100 | 50)
    [SerializeField] Slider s_VolumeSFX;
    [SerializeField] [Range(0, 100)] int volumeSFX;
    // 사운드 : 입력 - 안내(텍스트)
    [SerializeField] TMP_Text tmpt_Input;
    // 사운드 : 입력 - 마이크 꺼짐 모드(토글)
    [SerializeField] Toggle t_Mic;
    // 사운드 : 입력 - 장치(드롭다운)
    [SerializeField] TMP_Dropdown tmpd_Device;
    // 사운드 : 입력 - 마이크 증폭(슬라이더 | 0~10 | 0)
    [SerializeField] Slider s_MicAmplification;
    [SerializeField] [Range(0, 10)] int micAmplification;
    // 사운드 : 입력 - 입력 모드(드롭다운)
    [SerializeField] TMP_Dropdown tmpd_InputMode;
    // 사운드 : 입력 - 민감도(슬라이더 | 0~100 | 20)
    [SerializeField] Slider s_Sensitivity;
    [SerializeField] [Range(0, 100)] int sensitivity;

    //[Header("UI - Map")]

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
    }

    #endregion
}