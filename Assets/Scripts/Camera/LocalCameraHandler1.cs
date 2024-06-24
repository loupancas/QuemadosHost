using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LocalCameraHandler1 : MonoBehaviour
{
    public Transform _cameraAnchorPoint;
    public Camera localCamera;

    Vector2 viewInput;

    float cameraRotationX = 0;
    float cameraRotationY = 0;

    NetworkPlayer playermodel;
    //CinemachineVirtualCamera cinemachineVirtualCamera;
    NetworkCharacterControllerCustom _networkCharacterControllerCustom;
    private void Awake()
    {

     localCamera = GetComponent<Camera>();
     _networkCharacterControllerCustom = GetComponentInParent<NetworkCharacterControllerCustom>();
    }

    void Start()
    {
       if(localCamera.enabled)
            localCamera.transform.parent = null;

        cameraRotationX = GameManager.instance.CameraViewRotation.x;
        cameraRotationY = GameManager.instance.CameraViewRotation.y;
    }

    private void LateUpdate()
    {
        if (_cameraAnchorPoint == null)
            return;
        if (!localCamera.enabled)
            return;   
       
            localCamera.transform.position = _cameraAnchorPoint.position;
            cameraRotationX += viewInput.y * Time.deltaTime * 40;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

            cameraRotationY += viewInput.x * Time.deltaTime * _networkCharacterControllerCustom.rotationSpeed;

            localCamera.transform.rotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);

        
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



