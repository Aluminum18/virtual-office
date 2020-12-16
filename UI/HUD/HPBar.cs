using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private FloatVariable _hp;
    [SerializeField]
    private Slider _hpSlider;

    private void OnEnable()
    {
        _hp.OnValueChange += UpdateHPValue;
    }

    private void OnDisable()
    {
        _hp.OnValueChange -= UpdateHPValue;
    }

    private void UpdateHPValue(float newValue)
    {
        _hpSlider.value = newValue;
    }
}
