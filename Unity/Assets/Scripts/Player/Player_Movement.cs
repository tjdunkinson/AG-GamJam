using UnityEngine;

public partial class Player
{
    [SerializeField] private float _runSpeed = 5f;

    private bool _isClimbing;

    private bool HasCollided { get { return Controller.detectCollisions; }}

    private bool HasCollidedSides
    {
        get { return (Controller.collisionFlags & CollisionFlags.CollidedSides) != 0; }
    }

    private bool HasCollidedAbove
    {
        get { return (Controller.collisionFlags & CollisionFlags.CollidedAbove) != 0; }
    }

    private bool HasCollidedBelow
    {
        get { return (Controller.collisionFlags & CollisionFlags.CollidedBelow) != 0; }
    }

    private void MovementInputEvent(Vector2 input)
    {
        Vector3 moveVector = Vector3.zero;  

        // horizontal movement
        if (!_isClimbing)
        {
            // TODO: Acceleration, inherit platform velocity
            moveVector.x = input.x*_runSpeed;
        }

        // vertical movement
        // climbing
        // jumping
        // flying

        // perform movement here
        moveVector *= Time.deltaTime;
        Controller.Move(moveVector);
        
        // perform collisions here
        if (!HasCollided) return;

        if (HasCollidedSides)
        {
            moveVector.x *= -1f;
        }

        if (HasCollidedAbove || HasCollidedBelow)
        {
            moveVector.y *= -1f;
        }
        
        // perform adjustment movement
        transform.Translate(moveVector);
    }

}