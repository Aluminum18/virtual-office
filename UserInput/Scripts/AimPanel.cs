using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AimPanel : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Camera _uiCam;

    [SerializeField]
    private UnityEvent _onPointerDown;

    [SerializeField]
    private Vector3Variable _touchPoint;

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchPoint.Value = _uiCam.ScreenToWorldPoint(eventData.position);
        _onPointerDown.Invoke();
    }
}
