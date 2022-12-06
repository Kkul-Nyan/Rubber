using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    public GameObject PlayerObject;
    public GameObject cameraHolder;
    public Vector3 offset;

    private void Update()
    {
        cameraHolder.transform.position = PlayerObject.transform.position + offset;
    }
    
}
