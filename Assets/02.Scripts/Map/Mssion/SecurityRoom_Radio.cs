using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;
using Unity.Netcode;

public class SecurityRoom_Radio : MonoBehaviour 
{
    public GameObject radioBtn;
    
    public GameObject light;

    [SerializeField] bool isSucessLightMission = false;

    [SerializeField]
    LightningBoltScript lightning;

 
    // Update is called once per frame
    void Update()
    {
        RadioElectricity();
    }

    private void RadioElectricity()
    {
        lightning.ChaosFactor = radioBtn.transform.rotation.z;
        
        if (lightning.ChaosFactor > -0.06 && lightning.ChaosFactor < 0.06)
        {
            isSucessLightMission=true;
           
        }
        else if(lightning.ChaosFactor < -0.06 || lightning.ChaosFactor > 0.06)
        {
            isSucessLightMission = false;
            
        }
      

        
    }
}
