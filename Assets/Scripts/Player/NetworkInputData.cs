using Fusion;
using UnityEngine;
using static UnityEngine.EventSystems.PointerEventData;

public struct NetworkInputData : INetworkInput
{
    public Vector2 movementInput;   
    public bool isFirePressed;
    public bool isJumpPressed;
   
}

