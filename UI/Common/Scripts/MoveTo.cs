using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField]
    private LeanTweenType _moveEase;
    [SerializeField]
    private LeanTweenType _revertMoveEase;
    [SerializeField]
    private Vector3 _to;
    [SerializeField]
    private Vector3 _from;

    public void StartMove(float duration)
    {
        transform.localPosition = _from;
        LeanTween.moveLocal(gameObject, _to, duration).setEase(_moveEase);
    }

    public void RevertMove(float duration)
    {
        transform.localPosition = _to;
        LeanTween.moveLocal(gameObject, _from, duration).setEase(_revertMoveEase);
    }
}
