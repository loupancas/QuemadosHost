using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }
    public Transform playermodel;
    public bool is3rdPersonCamera { get; set; } 
    public LocalCameraHandler localCameraHandler;
    private NickNameBarLifeItem _myItemUI;
    public event Action OnPlayerDespawned = delegate { };
    [Networked, OnChangedRender(nameof(OnNickNameChanged))]
    public string NickName { get; set; }
    LifeHandler lifeHandler;
    public LifeHandler SendMyLiFeHandler() => lifeHandler;


    public override void Spawned()
    {
        _myItemUI = NickNameBarLifeManager.Instance.CreateNewItem(this);
        lifeHandler = GetComponent<LifeHandler>();
        lifeHandler.GetMyUI(_myItemUI);
        if (Object.HasInputAuthority)
        {
            Local = this;
            Utils.SetRenderLayerInChildren(playermodel, LayerMask.NameToLayer("LocalPlayerModel"));
            Camera.main.gameObject.SetActive(false);
            localCameraHandler.localCamera.enabled = true;
            localCameraHandler.gameObject.SetActive(true);
            localCameraHandler.transform.parent = null;
            RPC_SetNewName(PlayerPrefs.GetString("UserNickName"));

            GetComponentInChildren<MeshRenderer>().material.color = Color.blue;

        }
        else
        {
            localCameraHandler.localCamera.enabled = false;
            localCameraHandler.gameObject.SetActive(false);
            GetComponentInChildren<MeshRenderer>().material.color = Color.red;

            //Camera localCamera = GetComponentInChildren<Camera>();
            //localCamera.enabled = false;
            //AudioListener audioListener = GetComponentInChildren<AudioListener>();
            //audioListener.enabled = false;
        }

        //Runner.SetPlayerObject(Object.InputAuthority, Object);
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SetNewName(string newNickName)
    {
        NickName = newNickName;
    }

    void OnNickNameChanged()
    {
        _myItemUI.UpdateNickName(NickName);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnPlayerDespawned();
    }


}
