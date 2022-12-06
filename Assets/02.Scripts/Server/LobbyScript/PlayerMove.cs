using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{

    /* private void Awake() {
         transform.rotation = Quaternion.Euler(-90,0,0);
     }*/

    /*public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
    }*/
    public Camera cam;
    private Rigidbody rd;
    private void Start()
    {
        rd = GetComponent<Rigidbody>();
        rd.isKinematic = false;
        cam.transform.position = this.transform.position;
    }
    private void Update()
    {


        //if(!IsOwner) return;
        //else
        //{
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        //}
    }

}