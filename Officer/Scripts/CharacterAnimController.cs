using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    public void PlayRunAnim()
    {
        // Running param
        _animator.SetBool("isRunning", true);
    }

    public void PlayIdle()
    {
        // Running param
        _animator.SetBool("isRunning", false);
    }

    public void PlayAttack()
    {
        _animator.SetBool("isAttacking", true);
    }

    public void StopAttack()
    {
        _animator.SetBool("isAttacking", false);
    }
}
