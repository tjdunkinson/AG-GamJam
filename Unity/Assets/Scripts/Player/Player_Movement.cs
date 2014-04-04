using UnityEngine;

public partial class Player
{
    [SerializeField] private float _runSpeed = 45f;
    [SerializeField] private float _groundFriction = 0.5f;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _gravity = 0.981f;

    private const float JumpJoyAxisThreshold = 0.5f;

    private bool _isClimbing;
    private bool _canJump;

    private bool HasCollided
    {
        get { return Controller.collisionFlags != CollisionFlags.None; }
    }

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

    private Vector3 _force = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;

    private void MovementInputEvent(Vector2 input)
    {
        // horizontal movement
        if (!_isClimbing)
        {
            // TODO: Acceleration, inherit platform velocity
            _force.x = input.x * _runSpeed;
        }

        // jumping
        if (input.y >= JumpJoyAxisThreshold && _canJump)
        {
            _force.y = GetJumpForce(_jumpHeight);
            _canJump = false;
        }

        // climbing
        // flying
        // apply gravity here
        _force.y -= Mathf.Abs(_gravity);

        // perform movement here
        // _acceleration = _force / mass; (assume 1.0f, use force in place of acceleration)
        _velocity += _force * Time.deltaTime;

        Controller.Move((_velocity - _force * Time.deltaTime / 2f) * Time.deltaTime);
        
        // perform collisions here
        if (!HasCollided) return;

        Vector3 undoCollideMovement = _velocity * Time.deltaTime;

        if (HasCollidedSides)
        {
            undoCollideMovement.x *= -1f;

            _velocity.x = 0f;
            _force.x = 0f;
        }

        if (HasCollidedAbove || HasCollidedBelow)
        {
            undoCollideMovement.y *= -1f;

            // TODO: Flying rules
            _velocity.y = 0f;
            _force.y = 0f;

            // apply ground friction here
            if (HasCollidedBelow)
            {
                _velocity.x *= _groundFriction;
                _canJump = true;
            }
        }
        
        // perform adjustment movement
        transform.Translate(undoCollideMovement);
    }

    private float GetJumpForce(float jumpHeight)
    {
		return Mathf.Sqrt(2f * jumpHeight * _gravity);	
    }
}