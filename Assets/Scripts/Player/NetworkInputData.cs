using Fusion;
using UnityEngine;
using static UnityEngine.EventSystems.PointerEventData;
using UnityEngine.InputSystem;

public struct NetworkInputData : INetworkInput
{
    public Vector2 movementInput;
    //public NetworkBool isFirePressed;
    public bool isFirePressed;
    public bool isJumpPressed;
}

