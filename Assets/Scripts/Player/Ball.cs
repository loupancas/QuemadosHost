using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

[RequireComponent(typeof(NetworkRigidbody3D))]
public class Ball : NetworkBehaviour
{
    private NetworkRigidbody3D _networkRb;
    public bool _BallOwned;
    public string Name;
    public Sprite Icon;

    TickTimer _lifeTimeTickTimer = TickTimer.None;

    [SerializeField] private byte _damage;

    private void Awake()
    {
        _networkRb = GetComponent<NetworkRigidbody3D>();
    }

    public override void Spawned()
    {
        _networkRb.Rigidbody.AddForce(transform.forward * 10, ForceMode.VelocityChange);

        if (Object.HasStateAuthority)
        {
            _lifeTimeTickTimer = TickTimer.CreateFromSeconds(Runner, 2);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (_lifeTimeTickTimer.Expired(Runner))
            {
                DespawnObject();
            }
        }
    }

    void DespawnObject()
    {
        _lifeTimeTickTimer = TickTimer.None;
        
        Runner.Despawn(Object);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LifeHandler lifeHandler))
        {
            lifeHandler.TakeDamage(_damage);
        }

        DespawnObject();
    }
}
