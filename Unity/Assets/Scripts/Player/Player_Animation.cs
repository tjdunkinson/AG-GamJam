using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private Animator _animator;

    private bool IsSetupForAnimation()
    {
        _animator = (Animator) gameObject.GetComponentInChildren(typeof(Animator));

        return (_animator != null);
    }

    enum AnimState
    {
        Idle = -1,
        Jumping = 0,
        Running = 1,
        Climbing = 2,
        Attacking = 3,
    }

    private static readonly Dictionary<AnimState, int> AnimIds = new Dictionary<AnimState, int>()
    {
        { AnimState.Jumping, Animator.StringToHash("Jumping") },
        { AnimState.Running, Animator.StringToHash("Running") },
        { AnimState.Climbing, Animator.StringToHash("Climbing") },
        { AnimState.Attacking, Animator.StringToHash("Attacking") },
    };

    private AnimState _lastState = AnimState.Idle;

    private void SetMecanimTransition(AnimState nextState)
    {

        // update flags from movement
        if (nextState == _lastState) return;

        switch (nextState)
        {
            case AnimState.Idle:
                {
                    _animator.SetBool(AnimIds[_lastState], false);

                    _lastState = nextState;
                }
                break;

            default:
            {
                if (_lastState != AnimState.Idle)
                    _animator.SetBool(AnimIds[_lastState], false);

                _lastState = nextState;
                _animator.SetBool(AnimIds[_lastState], true);
            }
                break;
        }
    }
}