using UnityEngine;
using Fusion;

public class CharacterInputHandler : MonoBehaviour
{
    private NetworkInputData _inputData;

    private bool _isJumpPressed;
    private bool _isFirePressed;
    
    void Start()
    {
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        _inputData.movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _isJumpPressed |= Input.GetKeyDown(KeyCode.Space);

    }

    public NetworkInputData GetLocalInputs()
    {
        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;
        
        return _inputData;
    }
}
