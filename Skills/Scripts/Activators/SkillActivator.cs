using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.Events;

public class SkillActivator : MonoBehaviour
{
    [Header("Inspec")]
    [SerializeField]
    private string _owner;
    [SerializeField]
    private int _team;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onStartCastFirst;
    [SerializeField]
    private UnityEvent _onFinishCastFirst;

    [Header("Config")]
    [SerializeField]
    protected SkillSO _skillSO;

    public string Owner
    {
        get
        {
            return _owner;
        }
        set
        {
            _owner = value;
        }
    }
    public int Team
    {
        get
        {
            return _team;
        }
        set
        {
            _team = Mathf.Clamp(value, 1, 2);
        }
    }

    public void ActiveFirstState()
    {
        _onStartCastFirst.Invoke();
        Observable.Timer(TimeSpan.FromSeconds(_skillSO.CastTime)).Subscribe(_ =>
        {
            FirstState();
            _onFinishCastFirst.Invoke();
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

    public virtual void Setup(params object[] args)
    {

    }
}
