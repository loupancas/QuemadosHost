using Fusion;
using Unity.VisualScripting;
using UnityEngine;


public class BallPickup : NetworkBehaviour
{

   
    public float Radius = 1f;
    public LayerMask LayerMask;
    public GameObject ActiveObject;
    public GameObject InactiveObject;

    [Networked]  
    public bool IsPickedUp { get; set; }

    private static Collider[] _colliders = new Collider[4];

    public override void Spawned()
    {
        UpdateBallState();
    }

    public override void FixedUpdateNetwork()
    {
        if (IsPickedUp)
            return;


        if (Runner.IsServer)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, Radius, _colliders, LayerMask);
            for (int i = 0; i < hitCount; i++)
            {
                CharacterMovementHandler player = _colliders[i].GetComponent<CharacterMovementHandler>();
                if (player != null)
                {
                    PickUp(player);
                    break;
                }
            }
        }


    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_PickUp(CharacterMovementHandler player)
    {    
       if(IsPickedUp)
        {
            return;
        }
     
        PickUp(player);
    }

    private void PickUp(CharacterMovementHandler player)
    {
        IsPickedUp = true;
        player.HasBall = true;
    }

    public void Drop()
    {
        IsPickedUp = false;
    }

    private void UpdateBallState()
    {
        ActiveObject.SetActive(!IsPickedUp);
        InactiveObject.SetActive(IsPickedUp);
    }

    public override void Render()
    {
        UpdateBallState();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}

