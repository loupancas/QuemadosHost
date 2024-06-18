using Fusion;
using UnityEngine;


public enum PowerUpType
{
    Jump,
    Velocity
}


public class PowerUpPickup : NetworkBehaviour
{

    public PowerUpType Type;
    public float Radius = 1f;
    public float Cooldown = 10f;
    public LayerMask LayerMask;
    public GameObject ActiveObject;
    public GameObject InactiveObject;
    public float modifier; // El multiplicador para la velocidad o el salto
    public float duration; // La duración del efecto del pickup
    
    public bool IsActive => _activationTimer.ExpiredOrNotRunning(Runner);

    [Networked]
    private TickTimer _activationTimer { get; set; }

    private static Collider[] _colliders = new Collider[4];

    public override void Spawned()
    {
        ActiveObject.SetActive(IsActive);
        InactiveObject.SetActive(!IsActive);
    }

    public override void FixedUpdateNetwork()
    {
        if (!IsActive)
            return;


        if (Runner.IsServer)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, Radius, _colliders, LayerMask);
            for (int i = 0; i < hitCount; i++)
            {
                PlayerController player = _colliders[i].GetComponent<PlayerController>();
                if (player != null)
                {
                    ApplyPowerUp(player);
                    _activationTimer = TickTimer.CreateFromSeconds(Runner, Cooldown);
                    break;
                }
            }
        }


    }

    private void ApplyPowerUp(PlayerController player)
    {
        switch (Type)
        {
            case PowerUpType.Jump:
                player.ApplyJumpBoost(modifier,duration); 
                Debug.Log("Jump Boost Applied");
                break;
            case PowerUpType.Velocity:
                player.ApplySpeedBoost(modifier,duration);
                Debug.Log("Speed Boost Applied");
                break;
        }
    }

    public override void Render()
    {
        ActiveObject.SetActive(IsActive);
        InactiveObject.SetActive(!IsActive);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, Radius);
    }
}

