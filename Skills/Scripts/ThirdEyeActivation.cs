using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ThirdEyeActivation : MonoBehaviour
{
    [Header("Events out")]
    [SerializeField]
    private GameEvent _onActivateThirdEye;
    [SerializeField]
    private GameEvent _onDeactivateThirdEye;

    [SerializeField]
    private SkillSO _thirdEyeSO;

    public void ActivateThirdEye(bool active)
    {
        if (active)
        {
            _onActivateThirdEye?.Raise();
            Observable.Timer(TimeSpan.FromSeconds(_thirdEyeSO.Duration)).Subscribe(_ =>
            {
                _onDeactivateThirdEye?.Raise();
            });
            return;
        }
        _onDeactivateThirdEye?.Raise();
    }
}
