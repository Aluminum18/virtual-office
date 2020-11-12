using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    private const int IDLE = 0;
    private const int RUN = 1;
    private const int FULL_ATTACK = 2;
    private const int AIM = 3;
    private const int SHOOT = 4;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private BoolVariable _decideToAttack;

    public void PlayIdle()
    {
        _animator.SetTrigger("Idle");
    }

    public void PlayRun()
    {
        var currentAnimState = _animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimState.IsName("RunForward_1"))
        {
            return;
        }

        _animator.SetTrigger("Run");
    }

    public void PlayAim()
    {

        _animator.SetTrigger("Aim");
    }

    public void PlayShoot()
    {
        _animator.SetTrigger("Shoot");
    }

    public void SetRunAimBlend(float blend)
    {
        _animator.SetFloat("MovingAim", blend);
    }

    /// <summary>
    /// from 0 to 1; 0 ->
    /// </summary>
    public void SetStrafe(float strafe)
    {

    }
}
