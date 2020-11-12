using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAction : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _characterState;

    [Header("Events out")]
    [SerializeField]
    private UnityEvent _onPrepareProjectile;
    [SerializeField]
    private UnityEvent _onCancelAttack;
    [SerializeField]
    private UnityEvent _onAttackProjectileSpawn;

    [Header("Config")]
    [SerializeField]
    private float _baseAttackTime;
    [SerializeField]
    private float _attackRate;
    [SerializeField]
    private float _attackFactor;

    [SerializeField]
    private float _timeToNextAttack = 0;

    public bool CancelAttack
    {
        get
        {
            return _cancelAttack;
        }
        set
        {
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

    private bool _cancelAttack = false;

    private bool _isAttacking = false;

    private bool _projectileReady = false;
    private bool _lateAttack = false;

    public void PrepareProjectile()
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

    public void Shoot()
    {
        if (_projectileReady)
        {
            SpawnProjectile();
            return;
        }

        _lateAttack = true;
    }

    public void ChangeState(string state)
    {
        _characterState.Value = state;
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

    private void OnEnable()
    {
        ChangeState("idle");
    }
}
