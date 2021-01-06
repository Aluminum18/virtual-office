using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectCameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCam;
    [SerializeField]
    private Camera _xRayCam;
    [SerializeField]
    private Transform _eagleVisionSphere;
    [SerializeField]
    private float _eagleVisionLauchTime = 0.3f;
    [SerializeField]
    private Vignette _eVisionVolume;

    [SerializeField]
    private UnityEvent _onEVisionBegin;
    [SerializeField]
    private UnityEvent _onEVisionStop;

    public void ActiveXray(bool active)
    {
        StopAllCoroutines();

        StartCoroutine(IE_ScaleEagleVisionSphere(active));
    }

    private IEnumerator IE_ScaleEagleVisionSphere(bool active)
    {
        _mainCam.enabled = false;
        _xRayCam.enabled = true;

        Vector3 to;
        if (active)
        {
            to = Vector3.one * 120f;
            _onEVisionBegin.Invoke();
        }
        else
        {
            to = Vector3.zero;
            _onEVisionStop.Invoke();
        }

        LeanTween.scale(_eagleVisionSphere.gameObject, to, _eagleVisionLauchTime).setEase(LeanTweenType.easeInOutQuad);

        yield return new WaitForSeconds(_eagleVisionLauchTime);
        _xRayCam.enabled = active;
        _mainCam.enabled = !active;
    }
}
