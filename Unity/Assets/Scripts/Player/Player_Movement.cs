using UnityEngine;

public partial class Player
{
    private const float JumpJoyAxisThreshold = 0.5f;

    [SerializeField] private float _airFrictionFactor = 0.9f;
    [SerializeField] private float _groundFrictionFactor = 0.5f;
    [SerializeField] private float _climbFrictionFactor = 0.5f;

    [SerializeField] private bool _canFly;

    [SerializeField] private float _flySpeed = 33f;
    [SerializeField] private float _climbSpeed = 25f;
    [SerializeField] private float _runSpeed = 45f;
    [SerializeField] private float _jumpHeight = 3f; 
    [SerializeField] private float _gravity = 0.981f;

    private Vector3 _force = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;

    private bool _canClimb;
    private bool _canJump;

    private bool _isClimbing;
    private bool _isClimbingApex;
    private bool _isFlying;

    private bool _isWallLeftSide; // else right side

    private int GetWallSideNormal
    {
        get { return _isWallLeftSide ? -1 : 1; }
    }

    private float GetWallClimbDist
    {
        get { return Controller.radius*1.1f;  }
    }

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
            // climbing verticality
            _force.y = input.y*_climbSpeed;

            _isFlying = false;
            _canJump = false;

            // leaping from wall (sideways motion)
            if (input.x >= (JumpJoyAxisThreshold * -GetWallSideNormal) && _canJump)
            {
                // force away from wall
                _canJump = false;
                _isFlying = true & _canFly;
                _isClimbing = false;

                float jumpSpeed = GetJumpSpeed(Mathf.Sqrt(_jumpHeight));
                _velocity.x = jumpSpeed;
                _velocity.y = jumpSpeed;
            }
            else if (HasFoundClimbingApex())
            {
                // force into wall apex
                _canJump = false;
                _isClimbing = false;

                _velocity.x = GetJumpSpeed(Controller.radius * 8f) * GetWallSideNormal;
                _velocity.y = GetJumpSpeed(Controller.height * 8f);
            }
        }

        if (_canClimb)
        {
            _canClimb = false;

            // check for valid surface
            _isClimbing = HasFoundClimbingSurface();
        }

        // jumping
        if (input.y >= JumpJoyAxisThreshold && _canJump && !_isClimbing)
        {
            _canJump = false;
            _isFlying = true & _canFly;

            _velocity.y = GetJumpSpeed(_jumpHeight);
        }

        if (_isFlying && _canFly)
        {
            _force.y = input.y*_flySpeed;
        }
        else if (!_isClimbing)
        {
            // apply gravity here
            _force.y -= Mathf.Abs(_gravity);
        }

        // perform movement here
        // _acceleration = _force / mass; (assume 1.0f, use force in place of acceleration)
        _velocity += _force*Time.deltaTime;
        _velocity.x = Mathf.Clamp(_velocity.x, -_runSpeed, _runSpeed);

        //Debug.Log(_velocity + ", " + _force);

        Controller.Move((_velocity - _force*Time.deltaTime/2f)*Time.deltaTime);

        // post-move hackery
        if (_isFlying && _canFly)
        {
            _velocity.y *= _airFrictionFactor;
            _velocity.x *= _airFrictionFactor;
        }
        else if (_isClimbing & !_isClimbingApex)
        {
            _velocity.y *= _climbFrictionFactor;
        }

        // end normal movement functions
        if (!HasCollided) return;

        // perform collisions here
        Vector3 undoCollideMovement = _velocity*Time.deltaTime;

        if (HasCollidedSides)
        {
            undoCollideMovement.x *= -1f;

            if (!_isClimbing) _isWallLeftSide = _velocity.x < 0f;
            if (_isClimbingApex) return;

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
                _isClimbing = false;
            }
        }

        // perform adjustment movement
        transform.Translate(undoCollideMovement);
    }

    private bool HasFoundClimbingSurface()
    {
        // raycast in direction
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, Vector3.right * GetWallSideNormal, out hitInfo, GetWallClimbDist))
        {
            if (!(hitInfo.collider is BoxCollider)) return false;

            return true;
        }

        return false;
    }

    private bool HasFoundClimbingApex()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, Vector3.right * GetWallSideNormal, out hitInfo, GetWallClimbDist))
        {
            if (!(hitInfo.collider is BoxCollider)) return false;
            
            // look for floors (0, 1, 0), not walls (1, 0, 0)
            return (!Equals(hitInfo.normal.x, 1f));
        }

        return true;
    }

    private float GetJumpSpeed(float jumpHeight)
    {
        return Mathf.Sqrt(2f*jumpHeight*_gravity);
    }
}