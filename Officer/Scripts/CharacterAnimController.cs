using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    private const string IDLE = "Idle";
    private const int RUN = 1;
    private const int FULL_ATTACK = 2;
    private const int AIM = 3;
    private const int SHOOT = 4;

    [Header("Reference")]
    [SerializeField]
    private StringVariable _characterState;
    [SerializeField]
    private Vector3Variable _rawInputMovingJoystick;
    [SerializeField]
    private StringVariable _userId;

    [Header("Events in (User input)")]
    [SerializeField]
    private GameEvent _onCancelAim;

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

    private void SubcribeInput()
    {
        var characterAtt = GetComponent<CharacterAttribute>();
        if (characterAtt == null)
        {
            return;
        }

        if (characterAtt.IsThisPlayer)
        {
            return;
        }

        _onCancelAim.Subcribe(PlayerIdleFunc);
    }

    private void OnEnable()
    {
        SubcribeInput();
    }

    private void OnDisable()
    {
        _onCancelAim?.Unsubcribe(PlayerIdleFunc);
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

    private void SetLayerWeight(int layerIndex, float weight)
    {
        _animator.SetLayerWeight(layerIndex, weight);
    }

    private void PlayerIdleFunc(params object[] args)
    {
        PlayIdle();
    }

}
