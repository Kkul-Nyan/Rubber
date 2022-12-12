using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRController : MonoBehaviour
{
    [Header("[Input Action Asset]")]
    [SerializeField] InputActionAsset inputActionAsset;

    [Header("[Transform]")]
    [SerializeField] Transform controller;

    #region Unity Event

    private void Awake()
    {
        // ��Ʈ�ѷ� ����
        controller = transform;

        // XR Canvas ��/�� ��ġ ���� �̺�Ʈ ����
        XRIActionEventConnect("[Controller] L", "XRI LeftHand Interaction",  "Activate", XRI_Swap);
        XRIActionEventConnect("[Controller] R", "XRI RightHand Interaction", "Activate", XRI_Swap);
    }
    

    private void OnApplicationQuit()
    {
        // XR Canvas ��/�� ��ġ ���� �̺�Ʈ ���� ����
        XRIActionEventDisConnect("[Controller] L", "XRI LeftHand Interaction",  "Activate", XRI_Swap);
        XRIActionEventDisConnect("[Controller] R", "XRI RightHand Interaction", "Activate", XRI_Swap);
    }

    #endregion

    /// <summary>
    /// XRI Action Event ���� �Լ�
    /// </summary>
    /// <param name="ControllerName"></param>
    /// <param name="inputActionMapName"></param>
    /// <param name="inputActionName"></param>
    /// <param name="actionFunc"></param>
    void XRIActionEventConnect(string ControllerName, string inputActionMapName, string inputActionName, System.Action<InputAction.CallbackContext> actionFunc)
    {
        if (name == ControllerName) inputActionAsset.FindActionMap(inputActionMapName).FindAction(inputActionName).started += actionFunc;
#if UNITY_EDITOR
        Debug.Log($"XRI Action Event ���� : {ControllerName}, {inputActionMapName}, {inputActionName}, {actionFunc}");
#endif
    }

    /// <summary>
    /// XRI Action Event ���� ���� �Լ�
    /// </summary>
    /// <param name="ControllerName"></param>
    /// <param name="inputActionMapName"></param>
    /// <param name="inputActionName"></param>
    /// <param name="actionFunc"></param>
    void XRIActionEventDisConnect(string ControllerName, string inputActionMapName, string inputActionName, System.Action<InputAction.CallbackContext> actionFunc)
    {
        if (name == ControllerName) inputActionAsset.FindActionMap(inputActionMapName).FindAction(inputActionName).started -= actionFunc;
#if UNITY_EDITOR
        Debug.Log($"XRI Action Event ���� ���� : {ControllerName}, {inputActionMapName}, {inputActionName}, {actionFunc}");
#endif
    }

    /// <summary>
    /// [XRI Input Action Binding(Any Trigger Button or "[", "]")]
    /// ��XR Canvas ��/�� ��ġ ����
    /// </summary>
    /// <param name="context">Any Trigger Button or "[", "]"</param>
    internal void XRI_Swap(InputAction.CallbackContext context)
    {
        XRInteractionUIManager.Instance.gameObject.transform.SetParent(controller, false);
#if UNITY_EDITOR
        Debug.Log("XRI_Swap CallBack Context : " + context);
#endif
    }
}