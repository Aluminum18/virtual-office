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

    public bool CancelAttack = false;

    public void StartAttack()
    {
        CancelAttack = false;
        CountingUntilProjectile();
        _startAttacking?.Invoke();
    }

    private void CountingUntilProjectile()
    {
        StartCoroutine(IE_AtkToProjectile());
    }

    private IEnumerator IE_AtkToProjectile()
    {
        float remainTime = _baseAttackTime * _attackFactor;
        
        while (remainTime > 0)
        {
            if (CancelAttack)
            {
                yield break;
            }

            remainTime -= Time.deltaTime;
            yield return null;
        }

        _onAttackProjectileSpawn?.Invoke();
    }
}
