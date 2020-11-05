using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAction : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _startAttacking;
    [SerializeField]
    private UnityEvent _onAttackProjectileSpawn;
    [SerializeField]
    private float _baseAttackTime;
    [SerializeField]
    private float _attackRate;
    [SerializeField]
    private float _attackFactor;

    [SerializeField]
    private float _timeToNextAttack = 0;

    public bool CancelAttack = false;

    private bool _isAttacking = false;

    private bool _projectileReady = false;
    private bool _lateAttack = false;

    public void StartAttack()
    {
        if (_isAttacking)
        {
            return;
        }

        if (_timeToNextAttack > 0)
        {
            return;
        }

        CancelAttack = false;
        _projectileReady = false;
        CountingUntilProjectile();
        _startAttacking?.Invoke();
    }

    public void DecideToAttack()
    {
        if (_projectileReady)
        {
            SpawnProjectile();
            return;
        }

        _lateAttack = true;
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
            if (CancelAttack)
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
    }
}
