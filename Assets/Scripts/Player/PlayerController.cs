using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(WeaponHandler))]
[RequireComponent(typeof(LifeHandler))]
public class PlayerController : NetworkBehaviour
{
    private NetworkCharacterControllerCustom _myCharacterController;
    private WeaponHandler _myWeaponHandler;

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
}
