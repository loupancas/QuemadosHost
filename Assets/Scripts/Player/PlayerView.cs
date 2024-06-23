using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerView : NetworkBehaviour
{
    //[SerializeField] private ParticleSystem _shootingParticles;

    //private NetworkMecanimAnimator _mecanim;
    //CharacterInputHandler player;
   




    //public override void Spawned()
    //{
    //    if (_mecanim == null) return;

    //    if (Object.HasInputAuthority)
    //    {
    //        _mecanim = GetComponentInChildren<NetworkMecanimAnimator>();

    //    }

    //}

    //public override void FixedUpdateNetwork()
    //{
           

    //    if (player._animator== true)
    //    {
    //        _mecanim.Animator.SetBool("Jumping", true);
    //    }


    //    if (player._rgbd.velocity.sqrMagnitude < 0.01f)
    //    {
    //        _mecanim.Animator.SetFloat("Speed",0f);
    //    }
    //    else
    //    {
    //        _mecanim.Animator.SetFloat("Speed", 1f);

    //    }

       




    //}

    //private void Awake()
    //{
    //    player = GetComponentInChildren<Player>();
    //    _mecanim = GetComponentInChildren<NetworkMecanimAnimator>();
    //}


    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //void RPC_TriggerShootingParticles()
    //{
    //    //_shootingParticles.Play();
    //}
}
