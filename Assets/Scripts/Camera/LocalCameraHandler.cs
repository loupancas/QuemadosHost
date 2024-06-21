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

    NetworkCharacterController _networkCharacterController;
    //NetworkPlayer playermodel;
    CharacterMovementHandler _playerController;
    CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
     localCamera = GetComponent<Camera>();
     _networkCharacterController = GetComponentInParent<NetworkCharacterController>();
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
            if (_playerController.ThirdPersonCamera)
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

                cinemachineVirtualCamera.enabled = false;
                Utils.SetRenderLayerInChildren(NetworkPlayer.Local.playermodel,LayerMask.NameToLayer("LocalPlayerModel"));

                localBall.SetActive(true);


            }
            

            localCamera.transform.position = _cameraAnchorPoint.position;
            cameraRotationX += viewInput.y*Time.deltaTime*40;
            cameraRotationX=Mathf.Clamp(cameraRotationX, -90, 90);

            cameraRotationY += viewInput.x * Time.deltaTime * _networkCharacterController.rotationSpeed;

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
            //GameManager.instance.CameraViewRotationX = cameraRotationX;
            //GameManager.instance.CameraViewRotationY = cameraRotationY;
        }
       
    }
}



