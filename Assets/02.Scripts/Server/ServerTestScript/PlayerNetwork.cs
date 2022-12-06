using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerNetwork : NetworkBehaviour 
{

    [SerializeField] private GameObject cameraHolder;
    public Vector3 offset;
    [SerializeField] Camera cam;

    private Rigidbody rd;
    //private Camera cam;

    private void Start()
    {
        if (!IsOwner && LobbyAndRelayManager.isConnect == true)
        {
            cam.gameObject.SetActive(false);
            enabled = false;
            return;
        }


        rd = GetComponent<Rigidbody>();
        rd.isKinematic = false;
        gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);

    }
    private void Update() 
    {


        cameraHolder.transform.position = this.transform.position + offset;
        Vector3 moveDir = new Vector3(0,0,0);

            if(Input.GetKey(KeyCode.W)) moveDir.z = +1f;     
            if(Input.GetKey(KeyCode.S)) moveDir.z = -1f;     
            if(Input.GetKey(KeyCode.A)) moveDir.x = -1f;     
            if(Input.GetKey(KeyCode.D)) moveDir.x = +1f;     
            
            float moveSpeed = 3f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        //}
    }

}
