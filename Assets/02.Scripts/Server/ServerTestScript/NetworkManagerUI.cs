using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;


public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField]private Button serverBtn;
    [SerializeField]private Button hostBtn;
    [SerializeField]private Button clientBtn;
    [SerializeField]private Button changeBtn;
    [SerializeField]private TMP_Text changeText; 
    [SerializeField]private Button makeRoomBtn;

    private void Awake()
    {
        serverBtn.onClick.AddListener(()=> {
        NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(()=> {
        NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(()=> {
        NetworkManager.Singleton.StartClient();
        });
        changeBtn.onClick.AddListener(()=>{
        //NetworkManager.Singleton.SendMessage("check","HostSuccess");
        changeText.text = "123123";
        });

    }


}
 