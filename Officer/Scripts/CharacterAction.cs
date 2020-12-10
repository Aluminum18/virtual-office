using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAction : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _characterState;

    [Header("Events in (user input)")]
    [SerializeField]
    private GameEvent _onAim;
    [SerializeField]
    private GameEvent _onCancelAim;
    [SerializeField]
    private GameEvent _onShoot;

    [Header("Events out")]
    [SerializeField]
    private UnityEvent _onPrepareProjectile;
    [SerializeField]
    private UnityEvent _onCancelAttack;
    [SerializeField]
    private UnityEvent _onAttackProjectileSpawn;
    [SerializeField]
    private UnityEvent _onStateChanged;

    [Header("Config")]
    [SerializeField]
    private float _baseAttackTime;
    [SerializeField]
    private float _attackRate;
    [SerializeField]
    private float _attackFactor;

    [SerializeField]
    private float _timeToNextAttack = 0;


    private bool _cancelAttack = false;

    private bool _isAttacking = false;

    private bool _projectileReady = false;
    private bool _lateAttack = false;

    #region Action could be network awared
    public bool CancelAttack
    {
        get
        {
            return _cancelAttack;
        }
        set
        {
            if (_cancelAttack == value)
            {
                return;
            }

            _cancelAttack = value;

            if (value)
            {
                _projectileReady = false;
                _isAttacking = false;
                _lateAttack = false;
                _timeToNextAttack = 0;
                _onCancelAttack.Invoke();
            }
        }
    }

    public void PrepareProjectile(params object[] args)
    {
        if (_isAttacking || _projectileReady)
        {
            return;
        }

        if (_timeToNextAttack > 0)
        {
            return;
        }

        _cancelAttack = false;
        _projectileReady = false;
        CountingUntilProjectile();
        _onPrepareProjectile?.Invoke();
    }

    public void Shoot(params object[] args)
    {
        if (_projectileReady)
        {
            SpawnProjectile();
            return;
        }

        _lateAttack = true;
    }

    public void ChangeToAttackState(params object[] args)
    {
        _characterState.Value = "attack";
        _onStateChanged.Invoke();
    }

    public void ChangeToIdleState(params object[] args)
    {
        _characterState.Value = "idle";
        _onStateChanged.Invoke();
    }
    #endregion
    public void CheckCancelAttackOnMove()
    {
        if (_characterState.Value == CharacterState.STATE_READY_ATTACK)
        {
            return;
        }
        CancelAttack = true;
    }

    private void SpawnProjectile()
    {
        _projectileReady = false;
        _timeToNextAttack = _attackRate;
        _onAttackProjectileSpawn.Invoke();
        StartCoroutine(IE_Reload());
    }

    private void CountingUntilProjectile()
    {
        StartCoroutine(IE_AtkToProjectile());
    }

    private IEnumerator IE_AtkToProjectile()
    {
        _isAttacking = true;

        float timeToProjectile = _baseAttackTime * _attackFactor;
        
        while (timeToProjectile > 0)
        {
            if (_cancelAttack)
            {
                yield break;
            }

            timeToProjectile -= Time.deltaTime;
            yield return null;
        }

        _projectileReady = true;

        if (_lateAttack)
        {
            SpawnProjectile();
            _lateAttack = false;
        }
        _isAttacking = false;
    }

    private IEnumerator IE_Reload()
    {
        while (_timeToNextAttack > 0)
        {
            _timeToNextAttack -= Time.deltaTime;
            yield return null;
        }

        ContinuePrepareProjectile();
    }

    private void ContinuePrepareProjectile()
    {
        if (_characterState.Value == CharacterState.STATE_READY_ATTACK)
        {
            PrepareProjectile();
        }
    }

    private void SubcribeInput()
    {
        if (!IsThisPlayer())
        {
            return;
        }

        _onAim?.Subcribe(ChangeToAttackState);
        _onAim?.Subcribe(PrepareProjectile);

        _onCancelAim?.Subcribe(ChangeToIdleState);
        _onCancelAim?.Subcribe(CancelAttackFunc);

        _onShoot?.Subcribe(PrepareProjectile);
        _onShoot?.Subcribe(Shoot);
    }

    private void UnsubcribeInput()
    {
        if (!IsThisPlayer())
        {
            return;
        }

        _onAim?.Unsubcribe(ChangeToAttackState);
        _onAim?.Unsubcribe(PrepareProjectile);

        _onCancelAim?.Unsubcribe(ChangeToIdleState);
        _onCancelAim?.Unsubcribe(CancelAttackFunc);

        _onShoot?.Unsubcribe(PrepareProjectile);
        _onShoot?.Unsubcribe(Shoot);
    }    

    /// <summary>
    /// Bridge for subcribe event
    /// </summary>
    private void CancelAttackFunc(params object[] args)
    {
        CancelAttack = true;
    }

    private bool IsThisPlayer()
    {
        var characterAtt = GetComponent<CharacterAttribute>();
        if (characterAtt == null)
        {
            return false;
        }

        return characterAtt.IsThisPlayer;
    }

    private void OnEnable()
    {
        SubcribeInput();
        ChangeToIdleState();
    }

    private void OnDisable()
    {
        UnsubcribeInput();
    }
}
