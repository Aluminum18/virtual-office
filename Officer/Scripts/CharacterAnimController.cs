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

    [Header("Reference")]
    [SerializeField]
    private StringVariable _characterState;
    [SerializeField]
    private Vector3Variable _rawInputMovingJoystick;

    [Header("Config")]
    [SerializeField]
    private Animator _animator;

    private bool IsReadyAttackParam
    {
        get
        {
            return _animator.GetBool("ReadyAttack");
        }
    }

    private bool IsMovingParam
    {
        get
        {
            return _animator.GetBool("Moving");
        }
    }

    public void PlayIdle()
    {
        _animator.SetTrigger("Idle");
    }

    public void PlayRun()
    {
        CheckAndSetLayerFollowingState();

        _animator.SetTrigger("Run");
    }

    public void PlayAim()
    {
        CheckAndSetLayerFollowingState();

        _animator.SetTrigger("Aim");
    }

    public void PlayShoot()
    {
        CheckAndSetLayerFollowingState();
        _animator.SetTrigger("Shoot");
    }

    public void SetStrafe()
    {
        _animator.SetFloat("MovingAimX", _rawInputMovingJoystick.Value.x);
        _animator.SetFloat("MovingAimY", _rawInputMovingJoystick.Value.y);
    }

    public void SetLayerWeight(int layerIndex, float weight)
    {
        _animator.SetLayerWeight(layerIndex, weight);
    }

    public void UpdateReadyAttackState()
    {
        bool readyAttack = _characterState.Value == CharacterState.STATE_READY_ATTACK;
        _animator.SetBool("ReadyAttack", readyAttack);
    }

    public void CheckAndSetLayerFollowingState()
    {
        if (_characterState.Value == CharacterState.STATE_READY_ATTACK)
        {
            SetLayerWeight(1, 1f);
            SetLayerWeight(2, 1f);
            return;
        }

        SetLayerWeight(1, 0f);
        SetLayerWeight(2, 0f);
    }
}
