using Fusion;
using Unity.VisualScripting;
using UnityEngine;


public class BallPickup : NetworkBehaviour
{

   
    public float Radius = 1f;
    //public float Cooldown = 5f;
    public LayerMask LayerMask;
    public GameObject ActiveObject;
    public GameObject InactiveObject;
   
    public bool IsPickedUp { get; private set; }
    public bool IsActive => !IsPickedUp;

    //[Networked]
    //private TickTimer _activationTimer { get; set; }

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
                PlayerController player = _colliders[i].GetComponent<PlayerController>();
                if (player != null)
                {
                    PickUp(player);
                    break;
                }
            }
        }


    }

    private void PickUp(PlayerController player)
    {    

        IsPickedUp = true;
        player.HasBall = true;
        UpdateBallState();
    }

    public void Drop()
    {
        IsPickedUp = false;
        UpdateBallState();
    }

    private void UpdateBallState()
    {
        ActiveObject.SetActive(IsActive);
        InactiveObject.SetActive(!IsActive);
    }

    private void CanShoot(PlayerController player)
    {
        player.HasBall = true;
    }



    public override void Render()
    {
        ActiveObject.SetActive(IsActive);
        InactiveObject.SetActive(!IsActive);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}

