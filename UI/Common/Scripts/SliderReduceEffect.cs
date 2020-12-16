using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderReduceEffect : MonoBehaviour
{
    [SerializeField]
    private Slider _referenceSlider;
    [SerializeField]
    private Slider _effectSlider;

    [SerializeField]
    private float _delayTime = 0.5f;
    [Tooltip("percent per sec")]
    [SerializeField]
    private float _reduceSpeed = 50f;

    private bool _isReducing = false;

    private void OnEnable()
    {
        _referenceSlider.onValueChanged.AddListener(UpdateChange);
    }

    private void OnDisable()
    {
        _referenceSlider.onValueChanged.RemoveListener(UpdateChange);
    }

    private void UpdateChange(float newValue)
    {
        Debug.Log("value changed");
        float currentValue = _effectSlider.value;


        if (newValue >= currentValue)
        {
            StopAllCoroutines();
            _effectSlider.value = newValue;
            return;
        }

        if (newValue < currentValue && !_isReducing)
        {
            StartCoroutine(ReduceSliderValue());
        }
    }

    private IEnumerator ReduceSliderValue()
    {
        float delay = _delayTime;
        while (delay > 0)
        {
            yield return null;
            delay -= Time.deltaTime;
        }

        float deltaReduce = _reduceSpeed * _referenceSlider.maxValue / 100f * Time.deltaTime;
        while (_effectSlider.value > _referenceSlider.value)
        {
            yield return null;
            _effectSlider.value -= deltaReduce;

            if (_effectSlider.value < _referenceSlider.value)
            {
                _effectSlider.value = _referenceSlider.value;
            }
        }
    }
}
