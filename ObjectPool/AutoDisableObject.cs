using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisableObject : MonoBehaviour
{
    [SerializeField]
    private Component _disabledObject;
    [SerializeField]
    private float _disableAfter;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(IE_Disable());
    }

    private IEnumerator IE_Disable()
    {
        float disableAfter = _disableAfter;
        while (disableAfter > 0)
        {
            disableAfter -= Time.deltaTime;
            yield return null;
        }
        _disabledObject.gameObject.SetActive(false);
    }
}
