using Fusion;
using UnityEngine;

public enum PickUpType
{
    Jump,
    Velocity,
}

public class PickUp : NetworkBehaviour
{
    public PickUpType Type;
    public float modifier = 5f; // El multiplicador para la velocidad o el salto
    public float duration = 5f; // La duración del efecto del pickup

    [Networked]
    public NetworkBool IsCollected { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                ApplyEffect(player);
                IsCollected = true;
                gameObject.SetActive(false);
            }
        }
    }

    private void ApplyEffect(PlayerController player)
    {
        if (Type == PickUpType.Jump)
        {
            player.ApplyJumpBoost(modifier, duration);
        }
        else if (Type == PickUpType.Velocity)
        {
            player.ApplySpeedBoost(modifier, duration);
        }
    }
}
