using UnityEngine;
using Fusion;

public class NetworkCharacterControllerCustom : NetworkCharacterController
{
   
    NetworkPlayer player;
    CharacterInputHandler inputHandler;

    private void Awake()
    {
        player = GetComponent<NetworkPlayer>();
        inputHandler = GetComponent<CharacterInputHandler>();
        if (player == null)
        {
            Debug.LogError("NetworkPlayer is missing");
        }
        if (inputHandler == null)
        {
            Debug.LogError("CharacterInputHandler is missing");
        }
    }

    public override void Move(Vector3 direction)
    {
        if (player == null || inputHandler == null)
        {
            return;
        }
        Camera playerCamera = player.Camera;
        if (playerCamera == null)
        {
            Debug.LogError("Player's camera is missing");
            return;
        }

        var deltaTime = Runner.DeltaTime;
        var previousPos = transform.position;
        var moveVelocity = Velocity;

       

        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        direction = (camForward * inputHandler._yAxi + camRight * inputHandler._xAxi).normalized;


        if (Grounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0f;
        }

        moveVelocity.y += gravity * Runner.DeltaTime;

        var horizontalVel = new Vector3(direction.x, 0, direction.z) * maxSpeed;

        if (direction == default)
        {
            horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
        }
        else
        {
            horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }

        moveVelocity.x = horizontalVel.x;
        moveVelocity.z = horizontalVel.z;

        Controller.Move(moveVelocity * deltaTime);

        Velocity = (transform.position - previousPos) * Runner.TickRate;
        Grounded = Controller.isGrounded;
    }

    public void Jump()
    {
        if (Grounded)
        {
            Velocity = new Vector3(Velocity.x, jumpImpulse, Velocity.z);
            //animator.SetBool("Jumping", true);
            Grounded = false;
        }
    }

    public void Rotate(float rotationY)
    {

        transform.Rotate(0, rotationY * Runner.DeltaTime * rotationSpeed, 0);
    }



}
