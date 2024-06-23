using UnityEngine;
using Fusion;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(WeaponHandler))]
[RequireComponent(typeof(LifeHandler))]
public class CharacterMovementHandler : NetworkBehaviour
{
    private NetworkCharacterControllerCustom _myCharacterController;
    private WeaponHandler _myWeaponHandler;
    private float _defaultSpeed;
    private float _defaultJumpForce;
    private Coroutine _speedCoroutine;
    private Coroutine _jumpCoroutine;
    public Animator Animator;
    [Networked]
    public NetworkBool HasBall { get; set; }
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
        _defaultJumpForce = _myCharacterController.jumpImpulse;
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData networkInputData)) return;


        // Movimiento
        Vector3 moveDirection =new Vector3(networkInputData.movementInput.x, 0, networkInputData.movementInput.y);
        _myCharacterController.Move(moveDirection);

        // Salto
        if (networkInputData.isJumpPressed)
        {
            _myCharacterController.Jump();


        }

        // Disparo
        if (networkInputData.isFirePressed && HasBall)
        {
            RPC_FireAndDropBall();
        }
        
    }

    public override void Render()
    {
        if (!GetInput(out NetworkInputData networkInputData)) return;
        Animator.SetFloat("Speed", 0f);
        //Animator.SetFloat("Speed", 1f);
        Animator.SetBool("Jumping", false);
        Animator.SetBool("Shooting", false);
        if (networkInputData.isJumpPressed)
            Animator.SetBool("Jumping", true);
        if (networkInputData.isFirePressed)
            Animator.SetBool("Shooting", true);
        if (networkInputData.movementInput.sqrMagnitude < 0.01f)
        {
            Animator.SetFloat("Speed", 0f);
        }
        else
        {
            Animator.SetFloat("Speed", 1f);

        }

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode =RpcHostMode.SourceIsHostPlayer)]
    private void RPC_FireAndDropBall()
    {
        if (!HasBall) return;
        _myWeaponHandler.Fire();
        DropBall();
    }


    private void DropBall()
    {
        HasBall = false;

        if (Runner.IsServer)
        {
            FindObjectOfType<BallPickup>().Drop();

        }
        else
        {
            RPC_DropBallOnServer();
        }

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPC_DropBallOnServer()
    {
        FindObjectOfType<BallPickup>().Drop();
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
        ChangeColorRecursively(transform, Color.red);
        yield return new WaitForSeconds(duration);
        _myCharacterController.maxSpeed = _defaultSpeed;
        ChangeColorRecursively(transform, Color.white);
    }

    private IEnumerator BoostJump(float modifier, float duration)
    {
        _myCharacterController.jumpImpulse = modifier;
         ChangeColorRecursively(transform, Color.blue);
        yield return new WaitForSeconds(duration);
        _myCharacterController.jumpImpulse = _defaultJumpForce;
        ChangeColorRecursively(transform, Color.white);
    }

    void ChangeColorRecursively(Transform parent, Color color)
    {
        foreach (Transform child in parent)
        {
            SkinnedMeshRenderer renderer = child.GetComponent<SkinnedMeshRenderer>();
            if (renderer != null)
            {
                foreach (var material in renderer.materials)
                {
                    material.color = color;
                }
            }

            ChangeColorRecursively(child, color);
        }
    }

   


}
