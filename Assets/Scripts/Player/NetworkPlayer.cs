using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }
    public Camera Camera;
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Camera = Camera.main;
            Camera.GetComponent<ThirdPersonCamera>().Target = transform;            

        }
       
    }

  
}

