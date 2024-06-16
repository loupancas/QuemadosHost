using UnityEngine;
using Fusion;
using System.Collections;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(WeaponHandler))]
[RequireComponent(typeof(LifeHandler))]
public class PlayerController : NetworkBehaviour
{
    private NetworkCharacterControllerCustom _myCharacterController;
    private WeaponHandler _myWeaponHandler;

    private float _defaultSpeed;
    private float _defaultJumpForce;
    private Coroutine _speedCoroutine;
    private Coroutine _jumpCoroutine;

    private void Awake()
    {
        _myCharacterController = GetComponent<NetworkCharacterControllerCustom>();
        _myWeaponHandler = GetComponent<WeaponHandler>();

       

        var lifeHandler = GetComponent<LifeHandler>();

        lifeHandler.OnDeadChange += (isDead) =>
        {
            _myCharacterController.Controller.enabled = !isDead;
            enabled = !isDead;
        };

        lifeHandler.OnRespawn += () =>
        {
            _myCharacterController.Teleport(transform.position);
        };
    }

    public override void Spawned()
    {
        _defaultSpeed = _myCharacterController.maxSpeed;
        _defaultJumpForce = _myCharacterController.jumpForce;
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData networkInputData)) return;

        // Movimiento
        Vector3 moveDirection = new Vector3(networkInputData.movementInput.x, 0, networkInputData.movementInput.y);
        _myCharacterController.Move(moveDirection);

        // Salto
        if (networkInputData.isJumpPressed)
        {
            _myCharacterController.Jump();
        }

        // Disparo
        if (networkInputData.isFirePressed)
        {
            _myWeaponHandler.Fire();
        }
    }

    public void ApplySpeedBoost(float modifier, float duration)
    {
        if (_speedCoroutine != null)
        {
            StopCoroutine(_speedCoroutine);
        }
        _speedCoroutine = StartCoroutine(BoostSpeed(modifier, duration));
    }

    public void ApplyJumpBoost(float modifier, float duration)
    {
        if (_jumpCoroutine != null)
        {
            StopCoroutine(_jumpCoroutine);
        }
        _jumpCoroutine = StartCoroutine(BoostJump(modifier, duration));
    }

    private IEnumerator BoostSpeed(float modifier, float duration)
    {
        _myCharacterController.maxSpeed = modifier;
        yield return new WaitForSeconds(duration);
        _myCharacterController.maxSpeed = _defaultSpeed;
    }

    private IEnumerator BoostJump(float modifier, float duration)
    {
        _myCharacterController.jumpForce = modifier;
        yield return new WaitForSeconds(duration);
        _myCharacterController.jumpForce = _defaultJumpForce;
    }
}
