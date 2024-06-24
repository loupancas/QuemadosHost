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
        if (HasInputAuthority)
        {
            Local = this;
            Utils.SetRenderLayerInChildren(playermodel, LayerMask.NameToLayer("LocalPlayerModel"));
            Camera.main.gameObject.SetActive(false);
            localCameraHandler.localCamera.enabled = true;
            localCameraHandler.gameObject.SetActive(true);
            localCameraHandler.transform.parent = null;

        }
        else
        {
            localCameraHandler.localCamera.enabled = false;
            localCameraHandler.gameObject.SetActive(false);

            //Camera localCamera = GetComponentInChildren<Camera>();
            //localCamera.enabled = false;
            //AudioListener audioListener = GetComponentInChildren<AudioListener>();
            //audioListener.enabled = false;
        }

        Runner.SetPlayerObject(Object.InputAuthority, Object);
    }
}
