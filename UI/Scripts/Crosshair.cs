using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("Referecene")]
    [SerializeField]
    private Vector3Variable _aimScreenPos;
    [SerializeField]
    private FloatVariable _aimingTime;
    [SerializeField]
    private FloatVariable _spread;
    [SerializeField]
    private FloatVariable _largestSpread;
    [SerializeField]
    private FloatVariable _movingCrosshairTime;

    [Header("Config")]
    [SerializeField]
    private Image _top;
    [SerializeField]
    private Image _bot;
    [SerializeField]
    private Image _left;
    [SerializeField]
    private Image _right;
    [SerializeField]
    private Transform _centerPos;

    private Vector3 _upInit;
    private Vector3 _rightInit;

    public void StartAim()
    {
        _top.transform.localPosition = _centerPos.localPosition + _upInit;
        _bot.transform.localPosition = _centerPos.localPosition - _upInit;
        _left.transform.localPosition = _centerPos.localPosition - _rightInit;
        _right.transform.localPosition = _centerPos.localPosition + _rightInit;

        LeanTween.moveLocalY(_top.gameObject, _spread.Value, _aimingTime.Value).setEase(LeanTweenType.easeOutQuart);
        LeanTween.moveLocalY(_bot.gameObject, -_spread.Value, _aimingTime.Value).setEase(LeanTweenType.easeOutQuart);
        LeanTween.moveLocalX(_right.gameObject, _spread.Value, _aimingTime.Value).setEase(LeanTweenType.easeOutQuart);
        LeanTween.moveLocalX(_left.gameObject, -_spread.Value, _aimingTime.Value).setEase(LeanTweenType.easeOutQuart);
    }

    public void ChangeCenter()
    {
        Vector3 newPos = new Vector3(_aimScreenPos.Value.x, _aimScreenPos.Value.y, transform.position.z);
        LeanTween.move(gameObject, newPos, _movingCrosshairTime.Value).setEase(LeanTweenType.easeOutSine);
    }

    private void Start()
    {
        UpdateSpread(_largestSpread.Value);
    }

    private void OnEnable()
    {
        _spread.OnValueChange += UpdateSpread;
    }

    private void OnDisable()
    {
        _spread.OnValueChange -= UpdateSpread;
    }

    private void UpdateSpread(float newSpread)
    {
        _upInit = Vector3.up * newSpread;
        _rightInit = Vector3.right * newSpread;
    }
}
