using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionCollider : MonoBehaviour
{
    public PartitionManager manager;
    public string areaName;


    private void OnTriggerEnter(Collider other)
    {
        IMymemine localPlayer = null;
        if(TryGetComponent(out localPlayer))
        {
            if (localPlayer.IsLocalPlayer())
            {
                OnEnterArea();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IMymemine localPlayer = null;
        if (TryGetComponent(out localPlayer))
        {
            if (localPlayer.IsLocalPlayer())
            {

            }
        }
    }

    private void OnEnterArea()
    {
        manager.CurrentPartition = this;
    }
}
