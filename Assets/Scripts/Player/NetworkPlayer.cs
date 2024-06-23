using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }
    public Transform playermodel;
    public bool is3rdPersonCamera { get; set; } 
    public LocalCameraHandler localCameraHandler;
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Utils.SetRenderLayerInChildren(playermodel, LayerMask.NameToLayer("LocalPlayerModel"));
            Camera.main.gameObject.SetActive(false);

        }
        else
        {
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;
            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;
        }
    }

  
}

