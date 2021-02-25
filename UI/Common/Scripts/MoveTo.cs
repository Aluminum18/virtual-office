using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField]
    private LeanTweenType _moveEase;
    [SerializeField]
    private LeanTweenType _revertMoveEase;
    [SerializeField]
    private bool _useTransform;
    [SerializeField]
    private Vector3 _to;
    [SerializeField]
    private Vector3 _from;
    [SerializeField]
    private Transform _toTrans;
    [SerializeField]
    private Transform _fromTrans;

    public void StartMove(float duration)
    {
        if (_useTransform)
        {
            _from = _fromTrans.localPosition;
            _to = _toTrans.localPosition;
        }

        transform.localPosition = _from;
        LeanTween.moveLocal(gameObject, _to, duration).setEase(_moveEase);
        
    }

    public void RevertMove(float duration)
    {
        if (_useTransform)
        {
            _from = _fromTrans.localPosition;
            _to = _toTrans.localPosition;
        }

        transform.localPosition = _to;
        LeanTween.moveLocal(gameObject, _from, duration).setEase(_revertMoveEase);

        Observable.Timer(System.TimeSpan.FromSeconds(1f)).Subscribe(_ =>
        {
            Debug.Log("test obj " + transform.localPosition);
        });
    }
}
