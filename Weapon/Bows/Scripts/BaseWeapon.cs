using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseWeapon : MonoBehaviour
{
    [Header("Unity Events")]
    [SerializeField]
    protected UnityEvent _onStartAttack;
    [SerializeField]
    protected UnityEvent _onPrepareProjectile;
    [SerializeField]
    protected UnityEvent _onCancelAttack;
    [SerializeField]
    protected UnityEvent _onAttack;
    [SerializeField]
    protected UnityEvent _onFinishAttack;
    [SerializeField]
    protected UnityEvent _onReload;

    [Header("Config")]
    [SerializeField]
    protected float _attackDelay;
    [SerializeField]
    protected float _reloadTime;
    [SerializeField]
    protected int _rounds;

    protected int _currentRounds;

    protected bool _cancelAttack = false;
    protected bool _isAttacking = false;
    protected bool _isReloading = false;
    protected bool _projectileReady = false;

    public virtual void StartAttack()
    {
        if (_projectileReady)
        {
            AttackImmediately();
            return;
        }

        if (_isAttacking || _isReloading || _currentRounds <= 0)
        {
            return;
        }

        _onStartAttack.Invoke();
        MainThreadDispatcher.StartUpdateMicroCoroutine(IE_PrepareProjectileAndAttack());
    }

    public virtual void PrepareProjectile()
    {
        _onPrepareProjectile.Invoke();
        MainThreadDispatcher.StartUpdateMicroCoroutine(IE_PrepareProjectile());
    }

    public virtual void CancelAttack()
    {
        _cancelAttack = true;
        _projectileReady = false;
        _onCancelAttack.Invoke();
    }

    public virtual void AttackImmediately()
    {
        _onAttack.Invoke();
        _isAttacking = false;
        _projectileReady = false;

        _currentRounds--;

        if (_currentRounds <= 0)
        {
            Reload();
        }
    }

    public virtual void Reload()
    {
        _onReload.Invoke();
        MainThreadDispatcher.StartUpdateMicroCoroutine(IE_StartReload());
    }

    private IEnumerator IE_PrepareProjectile()
    {
        float remainDelay = _attackDelay;

        _isAttacking = true;
        while (remainDelay > 0f)
        {
            if (_cancelAttack)
            {
                _cancelAttack = false;
                yield break;
            }

            remainDelay -= Time.deltaTime;
            yield return null;
        }

        _projectileReady = true;
    }

    private IEnumerator IE_PrepareProjectileAndAttack()
    {
        float remainDelay = _attackDelay;

        _isAttacking = true;
        while (remainDelay > 0f)
        {
            if (_cancelAttack)
            {
                _cancelAttack = false;
                yield break;
            }

            remainDelay -= Time.deltaTime;
            yield return null;
        }

        _projectileReady = true;
        StartAttack();
    }

    private IEnumerator IE_StartReload()
    {
        float remainReloadTime = _reloadTime;

        _isReloading = true;
        while (remainReloadTime > 0f)
        {
            remainReloadTime -= Time.deltaTime;
            yield return null;
        }
        _isReloading = false;

        _currentRounds = _rounds;
    }

    private void Awake()
    {
        _currentRounds = _rounds;
    }
}
