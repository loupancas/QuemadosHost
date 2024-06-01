using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

[RequireComponent(typeof(NetworkRigidbody3D))]
public class Bullet : NetworkBehaviour
{
    private NetworkRigidbody3D _networkRb;

    TickTimer _lifeTimeTickTimer = TickTimer.None;
    
    private void Awake()
    {
        _networkRb = GetComponent<NetworkRigidbody3D>();
    }

    public override void Spawned()
    {
        _networkRb.Rigidbody.AddForce(transform.right * 10, ForceMode.VelocityChange);

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
        if (Object && Object.HasStateAuthority)
        {
            //if (other)
            
            DespawnObject();
        }
    }
}
