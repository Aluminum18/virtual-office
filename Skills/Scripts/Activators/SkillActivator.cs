using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class SkillActivator : MonoBehaviour
{
    [SerializeField]
    protected SkillSO _skillSO;

    public void ActiveFirstState()
    {
        Observable.Timer(TimeSpan.FromSeconds(_skillSO.CastTime)).Subscribe(_ =>
        {
            FirstState();
        }
        );
    }

    public void ActiveSecondState()
    {
        Observable.Timer(TimeSpan.FromSeconds(_skillSO.CastTimeSecond)).Subscribe(_ =>
        {
            SecondState();
        }
        );
    }

    public virtual void FirstState()
    {

    }

    public virtual void SecondState()
    {

    }
}
