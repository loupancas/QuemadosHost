using Fusion;
using System.Windows.Input;
using UnityEngine;
using UnityEngine.InputSystem;




public class NetworkCharacterControllerCustomMov : NetworkCharacterController
{
    private Vector2 _accumulatedInput;

    // Update method to handle keyboard input
    void Update()
    {
        if (Keyboard.current != null)
        {
            var keyboard = Keyboard.current;
            var moveDirection = Vector2.zero;

            if (keyboard.wKey.isPressed) { moveDirection += Vector2.up; }
            if (keyboard.sKey.isPressed) { moveDirection += Vector2.down; }
            if (keyboard.aKey.isPressed) { moveDirection += Vector2.left; }
            if (keyboard.dKey.isPressed) { moveDirection += Vector2.right; }

            _accumulatedInput = moveDirection.normalized;

            // Handle other inputs if necessary
            // _accumulatedInput.Buttons.Set(EInputButton.Jump, keyboard.spaceKey.isPressed);
            // _accumulatedInput.Buttons.Set(EInputButton.Reload, keyboard.rKey.isPressed);
            // _accumulatedInput.Buttons.Set(EInputButton.Pistol, keyboard.digit1Key.isPressed || keyboard.numpad1Key.isPressed);
            // _accumulatedInput.Buttons.Set(EInputButton.Rifle, keyboard.digit2Key.isPressed || keyboard.numpad2Key.isPressed);
            // _accumulatedInput.Buttons.Set(EInputButton.Shotgun, keyboard.digit3Key.isPressed || keyboard.numpad3Key.isPressed);
            // _accumulatedInput.Buttons.Set(EInputButton.Spray, keyboard.fKey.isPressed);
        }
    }

    // FixedUpdate to handle character movement
    void FixedUpdate()
    {
        Move(new Vector3(_accumulatedInput.x, 0, _accumulatedInput.y));
    }

    public override void Move(Vector3 direction)
    {
        var deltaTime = Runner.DeltaTime;
        var previousPos = transform.position;
        var moveVelocity = Velocity;

        direction = direction.normalized;

        if (Grounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0f;
        }

        moveVelocity.y += gravity * Runner.DeltaTime;

        var horizontalVel = default(Vector3);
        horizontalVel.z = moveVelocity.x;

        if (direction == default)
        {
            horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
        }
        else
        {
            horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
            transform.rotation = Quaternion.Euler(Vector3.up * (Mathf.Sign(direction.z) < 0 ? 180 : 0));
        }

        moveVelocity.x = horizontalVel.z;

        Controller.Move(moveVelocity * deltaTime);

        Velocity = (transform.position - previousPos) * Runner.TickRate;
        Grounded = Controller.isGrounded;
    }
}
