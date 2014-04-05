using UnityEngine;

public partial class Player
{
    [SerializeField] private float _runSpeed = 45f;
    [SerializeField] private float _flySpeed = 33f;
    [SerializeField] private float _groundFrictionFactor = 0.5f;
    [SerializeField] private float _airFrictionFactor = 0.9f;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _gravity = 0.981f;
    [SerializeField] private bool _canFly;

    private const float JumpJoyAxisThreshold = 0.5f;

    private bool _isClimbing;
    private bool _isFlying;

    private bool _canJump;
    private bool _canClimb;

    private float GetFeetHeight
    {
        get { return transform.position.y - (Controller.height/2f); }
    }

    private float GetNewJumpTarget
    {
        get { return GetFeetHeight + _jumpHeight; }
    }

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
            // TODO: inherit platform velocity
            _force.x = input.x*_runSpeed;
        }

        // vertical movement

        // climbing
        if (_isClimbing)
        {
            ClimbingLogic(input);
        }
        if (_canClimb)
        {
            _canClimb = false;

            // check for valid surface
            _isClimbing = HasFoundClimbingSurface(_velocity.x);
        }

        // collide side
        // if collision with box collider of no rotation (???)
        // attach to face


        // jumping
        if (input.y >= JumpJoyAxisThreshold && _canJump)
        {
            _canJump = false;
            _isFlying = true & _canFly;

            _velocity.y = GetJumpSpeed(_jumpHeight);
        }

        if (_isFlying)
        {
            _force.y = input.y*_flySpeed;
        }
        else
        {
            // apply gravity here
            _force.y -= Mathf.Abs(_gravity);
        }

        // perform movement here
        // _acceleration = _force / mass; (assume 1.0f, use force in place of acceleration)
        _velocity += _force * Time.deltaTime;
        _velocity.x = Mathf.Clamp(_velocity.x, -_runSpeed, _runSpeed);

        Controller.Move((_velocity - _force * Time.deltaTime / 2f) * Time.deltaTime);

        // post-move hackery
        if (_isFlying)
        {
            _velocity.y *= _airFrictionFactor;
            _velocity.x *= _airFrictionFactor;
        }
        
        // end normal movement functions
        if (!HasCollided) return;

        // perform collisions here
        Vector3 undoCollideMovement = _velocity * Time.deltaTime;

        if (HasCollidedSides)
        {
            undoCollideMovement.x *= -1f;

            _velocity.x = 0f;
            _force.x = 0f;

            _canClimb = true;
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
                _velocity.x *= _groundFrictionFactor;
                _canJump = true;
                _isFlying = false;
            }
        }
        
        // perform adjustment movement
        transform.Translate(undoCollideMovement);
    }

    private bool HasFoundClimbingSurface(float horizontalDirection)
    {
        // raycast in direction
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, (horizontalDirection > 0f ? Vector3.right : Vector3.left), out hitInfo,
            Controller.radius * 2f))
        {
            if (!(hitInfo.collider is BoxCollider)) return false;

            // get surface x-pos

            // get surface apex

            return true;
        }

        return false;
    }

    private void ClimbingLogic(Vector2 input)
    {
        // climbing verticality

        // leaping from wall (sideways motion)
        
        // climbing over

        // touching ground (and not reattaching to wall)
    }

    private float GetJumpSpeed(float jumpHeight)
    {
        return Mathf.Sqrt(2f * jumpHeight * _gravity); 
    }
}