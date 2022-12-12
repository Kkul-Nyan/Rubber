using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VivoxUnity;
using Unity.Services.Vivox;
using UnityEditor.PackageManager;
using Unity.Services.Core;
using UnityEngine.InputSystem;

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

    [Header("UI - Option / Output")]
    // ���� : ��� - �ȳ�(�ؽ�Ʈ)
    [SerializeField] TMP_Text tmpt_Output;

    // ���� : ��� - ��ü ����(�����̴� | 0~100 | 100)
    [SerializeField] Slider s_VolumeAll;
    [SerializeField] int volumeAll;
    [SerializeField] TMP_Text tmpt_VolumeAll;

    // ���� : ��� - ���� ����(�����̴� | 0~100 | 50)
    [SerializeField] Slider s_VolumeMusic;
    [SerializeField] int volumeMusic;
    [SerializeField] TMP_Text tmpt_VolumeMusic;

    // ���� : ��� - �÷��̾� ����(�����̴� | 0~100 | 75)
    [SerializeField] Slider s_VolumePlayer;
    [SerializeField] int volumePlayer;
    [SerializeField] TMP_Text tmpt_VolumePlayer;

    // ���� : ��� - Ư�� ȿ�� ����(�����̴� | 0~100 | 50)
    [SerializeField] Slider s_VolumeSFX;
    [SerializeField] int volumeSFX;
    [SerializeField] TMP_Text tmpt_VolumeSFX;

    [Header("UI - Option / Input")]
    // ���� : �Է� - �ȳ�(�ؽ�Ʈ)
    [SerializeField] TMP_Text tmpt_Input;

    // ���� : �Է� - �ڵ� ����(���)
    [SerializeField] Toggle t_Automatic;

    // ���� : �Է� - ������ ���ϱ�(���)
    [SerializeField] Toggle t_PressToSpeak;

    // ���� : �Է� - ��ġ(��Ӵٿ�)
    [SerializeField] TMP_Dropdown tmpd_Device;

    // ���� : �Է� - ����ũ ����(�����̴� | 0~10 | 0)
    [SerializeField] Slider s_MicAmplification;
    [SerializeField] int micAmplification;
    [SerializeField] TMP_Text tmpt_MicAmplification;

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

    private void Start()
    {
        SliderAddListener(s_VolumeAll, volumeAll, tmpt_VolumeAll);
        SliderAddListener(s_VolumeMusic, volumeMusic, tmpt_VolumeMusic);
        SliderAddListener(s_VolumePlayer, volumePlayer, tmpt_VolumePlayer);
        SliderAddListener(s_VolumeSFX, volumeSFX, tmpt_VolumeSFX);
        SliderAddListener(s_MicAmplification, micAmplification, tmpt_MicAmplification);

        //Debug.Log("Avaiable Devices Count:" + VivoxService.Instance.Client.AudioInputDevices.AvailableDevices.Count);
        //foreach (IAudioDevice device in VivoxService.Instance.Client.AudioInputDevices.AvailableDevices)
        //{
        //    Debug.Log("Device: " + device.Name);
        //}

        /// Dropdown Dictionary Value
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {

    }

    private void OnApplicationQuit()
    {
        SliderRemoveListener(s_VolumeAll);
        SliderRemoveListener(s_VolumeMusic);
        SliderRemoveListener(s_VolumePlayer);
        SliderRemoveListener(s_VolumeSFX);
        SliderRemoveListener(s_MicAmplification);
    }

    #endregion

    #region Methods

    /// <summary>
    /// ��Slider onValueChanged ������ �߰�
    /// ��SliderValueSendText(float value) �� TMP_Text ����ȭ
    /// </summary>
    /// <param name="slider">Slider Name</param>
    /// <param name="parseValue">���� ������ �����Ǵ� ����</param>
    /// <param name="tmp_text">UI Text</param>
    void SliderAddListener(Slider slider, int parseValue, TMP_Text tmp_text)
    {
        slider.onValueChanged.AddListener(SliderValueSendText);

        void SliderValueSendText(float value)
        {
            parseValue = (int)value;
            tmp_text.text = parseValue.ToString();
        }
    }

    /// <summary>
    /// Slider onValueChanged ������ ����
    /// </summary>
    /// <param name="slider">Slider Name</param>
    void SliderRemoveListener(Slider slider)
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    public void SetAudioDevices(IAudioDevice targetInput = null, IAudioDevice targetOutput = null)
    {
        IAudioDevices inputDevices = VivoxService.Instance.Client.AudioInputDevices;
        IAudioDevices outputDevices = VivoxService.Instance.Client.AudioOutputDevices;
        if (targetInput != null && targetInput != VivoxService.Instance.Client.AudioInputDevices.ActiveDevice)
        {
            VivoxService.Instance.Client.AudioInputDevices.BeginSetActiveDevice(targetInput, ar =>
            {
                if (ar.IsCompleted)
                {
                    VivoxService.Instance.Client.AudioInputDevices.EndSetActiveDevice(ar);
                }
            });
        }
        if (targetOutput != null && targetOutput != VivoxService.Instance.Client.AudioOutputDevices.ActiveDevice)
        {
            VivoxService.Instance.Client.AudioOutputDevices.BeginSetActiveDevice(targetOutput, ar =>
            {
                if (ar.IsCompleted)
                {
                    VivoxService.Instance.Client.AudioOutputDevices.EndSetActiveDevice(ar);
                }
            });
        }
    }

    #endregion
}