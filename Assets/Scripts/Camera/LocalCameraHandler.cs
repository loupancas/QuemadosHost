using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LocalCameraHandler : MonoBehaviour
{
    public Transform _cameraAnchorPoint;
    public Camera localCamera;
    public GameObject localBall;

    Vector2 viewInput;

    float cameraRotationX = 0;
    float cameraRotationY = 0;

    NetworkPlayer playermodel;
    CinemachineVirtualCamera cinemachineVirtualCamera;
    NetworkCharacterControllerCustom _networkCharacterControllerCustom;
    private void Awake()
    {
     localCamera = GetComponent<Camera>();
     _networkCharacterControllerCustom = GetComponentInParent<NetworkCharacterControllerCustom>();
    }

    void Start()
    {
       
        cameraRotationX = GameManager.instance.CameraViewRotation.x;
        cameraRotationY = GameManager.instance.CameraViewRotation.y;
    }

    private void LateUpdate()
    {
        if (_cameraAnchorPoint==null)
        return;
        if(!localCamera.enabled)
        return;
        if(cinemachineVirtualCamera== null)
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        else 
        {
            if (NetworkPlayer.Local.is3rdPersonCamera)
            {

                if (!cinemachineVirtualCamera.enabled)
                {
                    cinemachineVirtualCamera.Follow = NetworkPlayer.Local.playermodel;
                    cinemachineVirtualCamera.LookAt = NetworkPlayer.Local.playermodel;
                    cinemachineVirtualCamera.enabled = true;
                    Utils.SetRenderLayerInChildren(NetworkPlayer.Local.playermodel,LayerMask.NameToLayer("Default"));
                    localBall.SetActive(false);
                }
               return;
            }
            else
            {
                if(cinemachineVirtualCamera.enabled)
                {
                    cinemachineVirtualCamera.enabled = false;


                    Utils.SetRenderLayerInChildren(NetworkPlayer.Local.playermodel, LayerMask.NameToLayer("Player"));

                    localBall.SetActive(true);
                }
             


            }
            

            localCamera.transform.position = _cameraAnchorPoint.position;
            cameraRotationX += viewInput.y*Time.deltaTime*40;
            cameraRotationX=Mathf.Clamp(cameraRotationX, -90, 90);

            cameraRotationY += viewInput.x * Time.deltaTime * _networkCharacterControllerCustom.rotationSpeed;

            localCamera.transform.rotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);

        }
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }

    private void OnDestroy()
    {
        if(cameraRotationX != 0 && cameraRotationY!=0)
        {
            GameManager.instance.CameraViewRotation.x = cameraRotationX;
            GameManager.instance.CameraViewRotation.y = cameraRotationY;
        }
       
    }
}



