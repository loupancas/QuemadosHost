using UnityEngine;
using Fusion;

public class NetworkCharacterControllerCustom : NetworkCharacterController
{
   
    //public float rotationSpeed = 10f;
    public float viewUpDownRotationSpeed = 50f;
    //public Vector2 viewInputVector;
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
            Grounded = false;
        }
    }

    public void Rotate(float rotationY)
    {
        
        transform.Rotate(0, rotationY*Runner.DeltaTime*rotationSpeed, 0);
    }

   

}
