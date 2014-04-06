using UnityEngine;

public partial class Player
{
    private Animator _animator;

    private bool IsSetupForAnimation()
    {
        _animator = (Animator) gameObject.GetComponentInChildren(typeof(Animator));

        return (_animator != null);
    }

    private void MecanimUpdate()
    {
        
    }
}