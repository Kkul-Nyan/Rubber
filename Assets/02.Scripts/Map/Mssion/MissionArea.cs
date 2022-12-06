using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionArea : MonoBehaviour
{
    private void Start()
    {
        Initialize();
    }

    protected void Initialize()
    {
        
    }

    protected void MissionActivate()
    {

    }

    protected void MissionDeactivate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        IMymemine localPlayer = null;        
        if(other.TryGetComponent(out localPlayer))
        {
            if(localPlayer.IsLocalPlayer())
            {
                MissionActivate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IMymemine localPlayer = null;
        if (other.TryGetComponent(out localPlayer))
        {
            if (localPlayer.IsLocalPlayer())
            {
                MissionDeactivate();
            }
        }
    }
}
