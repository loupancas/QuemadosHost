using Fusion;
using UnityEngine;

public class PowerUpPickup : NetworkBehaviour
{
    public enum PowerUpType
    {
        Jump,
        Velocity
    }

    public PowerUpType Type;
    public float Radius = 1f;
    public float Cooldown = 10f;
    public LayerMask LayerMask;
    public GameObject ActiveObject;
    public GameObject InactiveObject;

    public bool IsActive => _activationTimer.ExpiredOrNotRunning(Runner);

    [Networked]
    private TickTimer _activationTimer { get; set; }

    private static Collider[] _colliders = new Collider[8];

    public override void Spawned()
    {
        ActiveObject.SetActive(IsActive);
        InactiveObject.SetActive(!IsActive);
    }

    public override void FixedUpdateNetwork()
    {
        if (!IsActive)
            return;

        // Obtener todos los colisionadores alrededor del pickup dentro del radio.
        int collisions = Runner.GetPhysicsScene().OverlapSphere(transform.position + Vector3.up, Radius, _colliders, LayerMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < collisions; i++)
        {
            var player = _colliders[i].GetComponentInParent<PlayerController>();
            if (player != null)
            {
                ApplyPowerUp(player);
                _activationTimer = TickTimer.CreateFromSeconds(Runner, Cooldown);
                break;
            }
        }
    }

    private void ApplyPowerUp(PlayerController player)
    {
        switch (Type)
        {
            case PowerUpType.Jump:
                player.ApplyJumpBoost(5f, 10f); 
                break;
            case PowerUpType.Velocity:
                player.ApplySpeedBoost(10f, 10f); 
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

