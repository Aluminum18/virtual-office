using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private UnityEvent _onEVisionBegin;
    [SerializeField]
    private UnityEvent _onEVisionStop;

    public void ActiveXray(bool active)
    {
        StopAllCoroutines();

        StartCoroutine(IE_ScaleEableVisionSphere(active));
    }

    private IEnumerator IE_ScaleEableVisionSphere(bool active)
    {
        _mainCam.enabled = false;
        _xRayCam.enabled = true;

        if (active)
        {
            _onEVisionBegin.Invoke();
        }
        else
        {
            _onEVisionStop.Invoke();
        }

        float time = _eagleVisionLauchTime;
        float scaleFactor = 120 / time * Time.deltaTime * (active ? 1 : -1);
        float scale = _eagleVisionSphere.localScale.x;
        while (time > 0)
        {
            scale += scaleFactor;
            Debug.Log("Scale" + scale);

            time -= Time.deltaTime;
            _eagleVisionSphere.localScale = Vector3.one * scale;
            yield return null;
        }

        _xRayCam.enabled = active;
        _mainCam.enabled = !active;

        _eagleVisionSphere.localScale = active ? 120 * Vector3.one : Vector3.zero;
    }
}
