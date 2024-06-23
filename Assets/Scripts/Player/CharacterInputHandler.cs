using UnityEngine;
using Fusion;

public class CharacterInputHandler : MonoBehaviour
{
    //private NetworkInputData _inputData;

    private bool _isJumpPressed;
    private bool _isFirePressed;

    LocalCameraHandler _localCameraHandler;
    CharacterMovementHandler characterMovementHandler;

    Vector2 viewInput;
    Vector2 movementInput;




    private void Awake()
    {
        _localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponent<CharacterMovementHandler>();

    }
    void Start()
    {
        //_inputData = new NetworkInputData();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        viewInput.x = Input.GetAxis("Mouse X");
        viewInput.y = Input.GetAxis("Mouse Y")*-1;

        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");


        //_inputData.movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //_isJumpPressed |= Input.GetKeyDown(KeyCode.Space);

        //_isFirePressed |= Input.GetKeyDown(KeyCode.F);

        //_3rdPersonCamera |= Input.GetMouseButtonDown(1);

        if (Input.GetKeyDown(KeyCode.Space))        
            _isJumpPressed = true;
        
        if (Input.GetKeyUp(KeyCode.F))       
            _isFirePressed = true;
        
        if (Input.GetKeyDown(KeyCode.C))
            NetworkPlayer.Local.is3rdPersonCamera = !NetworkPlayer.Local.is3rdPersonCamera;


        _localCameraHandler.SetViewInputVector(viewInput);

    }

    public NetworkInputData GetLocalInputs()
    {
        NetworkInputData _inputData = new NetworkInputData
        {
            isFirePressed = _isFirePressed,
            isJumpPressed = _isJumpPressed,
            aimForwardVector = _localCameraHandler.transform.forward
        };

        _isFirePressed = false;
        _isJumpPressed = false;

        return _inputData;
    }
}
