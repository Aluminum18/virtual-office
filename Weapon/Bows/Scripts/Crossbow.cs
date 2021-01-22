using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : BaseWeapon
{
    [Header("Reference - assign at runtime")]
    [SerializeField]
    private Vector3Variable _aimTo;

    [Header("Config")]
    [SerializeField]
    private ArrowSpawner _arrowSpawner;

    private void OnEnable()
    {
        _arrowSpawner.SetAimSpotInput(_aimTo);
    }

    public void SetTarget(Vector3Variable target)
    {
        _arrowSpawner.SetAimSpotInput(target);
    }

    public override void AttackImmediately()
    {
        base.AttackImmediately();
        _arrowSpawner.FireArrow();
    }
}
