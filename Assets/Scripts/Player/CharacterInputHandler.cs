using UnityEngine;
using Fusion;

public class CharacterInputHandler : MonoBehaviour
{
    private NetworkInputData _inputData;

    private bool _isJumpPressed;
    private bool _isFirePressed;

    LocalCameraHandler _localCameraHandler;
    CharacterMovementHandler characterMovementHandler;
    Vector2 viewInput=Vector2.zero;
    private void Awake()
    {
        _localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponent<CharacterMovementHandler>();

    }
    void Start()
    {
        _inputData = new NetworkInputData();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        _inputData.movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _isJumpPressed |= Input.GetKeyDown(KeyCode.Space);

        _isFirePressed |= Input.GetKeyDown(KeyCode.F);

        if (Input.GetKeyDown(KeyCode.C))
        {
          NetworkPlayer.Local.is3rdPersonCamera = !NetworkPlayer.Local.is3rdPersonCamera;
        }
        _localCameraHandler.SetViewInputVector(viewInput);

    }

    public NetworkInputData GetLocalInputs()
    {
        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        _inputData.aimForwardVector = _localCameraHandler.transform.forward;

        return _inputData;
    }
}
