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
    private float _baseAttackTime = 0.8f;
    [SerializeField]
    private float _attackFactor = 1;

    private float _timeToNextAttack = 0;

    public bool CancelAttack = false;

    private bool _projectileReady = false;
    private bool _lateAttack = false;

    public void StartAttack()
    {
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
            _onAttackProjectileSpawn.Invoke();
            return;
        }

        _lateAttack = true;
    }

    private void CountingUntilProjectile()
    {
        StartCoroutine(IE_AtkToProjectile());
    }

    private IEnumerator IE_AtkToProjectile()
    {
        _timeToNextAttack = _baseAttackTime * _attackFactor;
        
        while (_timeToNextAttack > 0)
        {
            if (CancelAttack)
            {
                _timeToNextAttack = 0;
                yield break;
            }

            _timeToNextAttack -= Time.deltaTime;
            yield return null;
        }

        _projectileReady = true;

        if (_lateAttack)
        {
            _onAttackProjectileSpawn.Invoke();
            _lateAttack = false;
        }
    }
}
