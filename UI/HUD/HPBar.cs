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

    public void SetHpVariable(FloatVariable hp)
    {
        _hp = hp;
        _hp.OnValueChange += UpdateHPValue;
    }

    private void OnDestroy()
    {
        _hp.OnValueChange -= UpdateHPValue;
    }

    private void UpdateHPValue(float newValue)
    {
        _hpSlider.value = newValue;
    }
}
