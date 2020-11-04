using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private BoolVariable _decideToAttack;

    public void PlayRunAnim()
    {
        _animator.SetBool("isRunning", true);
    }

    public void PlayIdle()
    {
        _animator.SetBool("isRunning", false);
    }

    public void PlayAttack()
    {
        _animator.SetBool("isAttacking", true);
    }

    public void ResetAttackingFlag()
    {
        _animator.SetBool("isAttacking", false);
    }

    public void HoldAttack()
    {
        if (_decideToAttack.Value)
        {
            _animator.speed = 1f;
            return;
        }
        _animator.speed = 0f;
    }

}
