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
    // ���� : ��� - �ȳ�(�ؽ�Ʈ)
    [SerializeField] TMP_Text tmpt_Output;
    // ���� : ��� - ��ü ����(�����̴� | 0~100 | 100)
    [SerializeField] Slider s_VolumeAll;
    [SerializeField] [Range(0, 100)] int volumeAll;
    // ���� : ��� - ���� ����(�����̴� | 0~100 | 50)
    [SerializeField] Slider s_VolumeMusic;
    [SerializeField] [Range(0, 100)] int volumeMusic;
    // ���� : ��� - �÷��̾� ����(�����̴� | 0~100 | 75)
    [SerializeField] Slider s_VolumePlayer;
    [SerializeField] [Range(0, 100)] int volumePlayer;
    // ���� : ��� - Ư�� ȿ�� ����(�����̴� | 0~100 | 50)
    [SerializeField] Slider s_VolumeSFX;
    [SerializeField] [Range(0, 100)] int volumeSFX;
    // ���� : �Է� - �ȳ�(�ؽ�Ʈ)
    [SerializeField] TMP_Text tmpt_Input;
    // ���� : �Է� - ����ũ ���� ���(���)
    [SerializeField] Toggle t_Mic;
    // ���� : �Է� - ��ġ(��Ӵٿ�)
    [SerializeField] TMP_Dropdown tmpd_Device;
    // ���� : �Է� - ����ũ ����(�����̴� | 0~10 | 0)
    [SerializeField] Slider s_MicAmplification;
    [SerializeField] [Range(0, 10)] int micAmplification;
    // ���� : �Է� - �Է� ���(��Ӵٿ�)
    [SerializeField] TMP_Dropdown tmpd_InputMode;
    // ���� : �Է� - �ΰ���(�����̴� | 0~100 | 20)
    [SerializeField] Slider s_Sensitivity;
    [SerializeField] [Range(0, 100)] int sensitivity;

    //[Header("UI - Map")]

    #endregion

    #region Unity Event

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Debug.LogWarning("���� Scene���� ���� UIManager �߰ߵƽ��ϴ�. �ϳ��� UIManager �����ؾ� �ϹǷ� �ߺ��� UIManager �����մϴ�.");
            Destroy(this);
            return;
        }
    }

    #endregion
}