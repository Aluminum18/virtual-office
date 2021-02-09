using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("Referecene")]
    [SerializeField]
    private FloatVariable _spread;

    [Header("Config")]
    [SerializeField]
    private float _crosshairAdjustTime;
    [SerializeField]
    private Image _top;
    [SerializeField]
    private Image _bot;
    [SerializeField]
    private Image _left;
    [SerializeField]
    private Image _right;
    [SerializeField]
    private Transform _visualCenterPos;
    [SerializeField]
    private Transform _actualCenterPos;

    private Vector3 _upInit;
    private Vector3 _rightInit;

    public void RandomActualCenter()
    {
        float x = _visualCenterPos.localPosition.x + Random.Range(-_spread.Value, _spread.Value);
        float yRandomRange = Mathf.Sqrt(Mathf.Pow(_spread.Value, 2) - x * x);
        float y = _visualCenterPos.localPosition.y + Random.Range(-yRandomRange, yRandomRange);

        _actualCenterPos.localPosition = new Vector3(x, y, _visualCenterPos.localPosition.z);
    }

    private void AdjustCrosshair(float newSpread)
    {
        LeanTween.moveLocalY(_top.gameObject, _spread.Value, _crosshairAdjustTime).setEase(LeanTweenType.easeOutQuart);
        LeanTween.moveLocalY(_bot.gameObject, -_spread.Value, _crosshairAdjustTime).setEase(LeanTweenType.easeOutQuart);
        LeanTween.moveLocalX(_right.gameObject, _spread.Value, _crosshairAdjustTime).setEase(LeanTweenType.easeOutQuart);
        LeanTween.moveLocalX(_left.gameObject, -_spread.Value, _crosshairAdjustTime).setEase(LeanTweenType.easeOutQuart);
    }

    private void OnEnable()
    {
        _spread.OnValueChange += AdjustCrosshair;
    }

    private void OnDisable()
    {
        _spread.OnValueChange -= AdjustCrosshair;

    }

    private void Start()
    {
        AdjustCrosshair(_spread.Value);
    }

}
