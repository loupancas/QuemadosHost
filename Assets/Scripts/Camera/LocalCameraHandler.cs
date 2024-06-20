using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCameraHandler : MonoBehaviour
{
    public Transform _cameraAnchorPoint;
    public Camera localCamera;
    public GameObject localBall;

    Vector2 viewInput;

    float cameraRotationX = 0;
    float cameraRotationY = 0;

    NetworkCharacterControllerCustom _networkCharacterControllerCustom;
    PlayerController _playerController;
    CinemachineVirtualCamera _virtualCamera;
    Vector2 cameraAnchorPoint;

    private void Awake()
    {
     localCamera = GetComponent<Camera>();
     _networkCharacterControllerCustom = GetComponentInParent<NetworkCharacterControllerCustom>();
    }

    void Start()
    {
        cameraRotationX = _cameraAnchorPoint.localEulerAngles.x;
        cameraRotationY = _cameraAnchorPoint.localEulerAngles.y;
        //cameraRotationX = GameManager.instance.CameraViewRotationX;
        //cameraRotationY = GameManager.instance.CameraViewRotationY;
    }

    private void LateUpdate()
    {
        if (cameraAnchorPoint==null)
        return;
        if(!localCamera.enabled)
        return;
        if(_virtualCamera== null)
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        else 
        {
            if (_playerController.ThirdPersonCamera)
            {

                if (!_virtualCamera.enabled)
                {
                    //CinemachineVirtualCamera.follow = NetworkPlayer.Local.playermodel;
                    //CinemachineVirtualCamera.lookAt = NetworkPlayer.Local.playermodel;
                    //CinemachineVirtualCamera.enabled = true;
                    //Utils.SetRenderLayerInChildren(NetworkPlayer.Local.playermodel,LayerMask.NameToLayer("Default"));
                    localBall.SetActive(false);
                }
               return;
            }
            else
            {
                //if(CinemachineVirtualCamera.enabled)

                //CinemachineVirtualCamera.enabled = false;
                //Utils.SetRenderLayerInChildren(NetworkPlayer.Local.playermodel,LayerMask.NameToLayer("LocalPlayerModel"));

                localBall.SetActive(true);


            }
            

            //localCamera.transform.position = cameraAnchorPoint.position;
            //cameraX += viewInput.y*Time.deltaTime*NetworkCharacterControllerCustom.ViewUpDownSpeed;
            //cameraX=Mathf.Clamp(cameraX, -90, 90);

            //cameraY += viewInput.x * Time.deltaTime * NetworkCharacterControllerCustom.RotationSpeed;

            localCamera.transform.rotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);

        }
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }

}
