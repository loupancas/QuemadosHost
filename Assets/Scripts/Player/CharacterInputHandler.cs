using UnityEngine;
using Fusion;

public class CharacterInputHandler : MonoBehaviour
{
    private NetworkInputData _inputData;

    private bool _isJumpPressed;
    private bool _isFirePressed;

    public float _xAxi;
    public float _yAxi;

    [SerializeField] private float _groundCheckDistance = 1.1f;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded = true;
    //Animator _animator;
    private void Awake()
    {
        _inputData = new NetworkInputData();


    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      
    }

    void Update()
    {
        //CheckGrounded();
        _xAxi = Input.GetAxis("Horizontal");
        _yAxi = Input.GetAxis("Vertical");

        _inputData.movementInput = new Vector2(_xAxi, _yAxi);
        //if (_inputData.movementInput.sqrMagnitude < 0.01f)
        //{
        //   _animator.SetFloat("Speed", 0f);
        //}
        //else
        //{
        //    _animator.SetFloat("Speed", 1f);

        //}


        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isJumpPressed = true;
            //_animator.SetBool("Jumping", true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _isFirePressed = true;
            //_animator.SetBool("Shooting", true);
        }    



    }

    public NetworkInputData GetLocalInputs()
    {
        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;


        // Debug log to see input data
        //Debug.Log("NetworkInputData: " + _inputData);

        return _inputData;
    }

    private void CheckGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        bool wasGrounded = _isGrounded;
        _isGrounded = Physics.Raycast(origin, Vector3.down, _groundCheckDistance, _groundLayer);

        Debug.DrawRay(origin, Vector3.down * _groundCheckDistance, _isGrounded ? Color.green : Color.red);

        if (!_isGrounded && wasGrounded)
        {
            //_animator.SetBool("Jumping", false);
        }
    }
}
