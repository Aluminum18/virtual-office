using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRayToWorld : MonoBehaviour
{
    [SerializeField]
    private Vector3Variable _output;
    [SerializeField]
    private RectTransform _screenTransform;
    [SerializeField]
    private Camera _worldCam;
    [SerializeField]
    private Camera _uiCam;

    public void ScreenRayToWorldPoint()
    {
        Vector3 screenPoint = _uiCam.WorldToScreenPoint(_screenTransform.position);
        var ray = _worldCam.ScreenPointToRay(screenPoint);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 10f);

        if (!Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            return;
        }

        _output.Value = hit.point;

    }
}
