using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AimPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private Camera _uiCam;

    [SerializeField]
    private UnityEvent _onPointerDown;

    [SerializeField]
    private Vector3Variable _touchPoint;

    [SerializeField]
    private float _sensitive;
    [SerializeField]
    private Transform _aimCamYRotate;
    [SerializeField]
    private Transform _aimCam;

    public void OnPointerDown(PointerEventData eventData)
    {
        //_touchPoint.Value = _uiCam.ScreenToWorldPoint(eventData.position);
        //_onPointerDown.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.delta * Time.deltaTime;
        _aimCamYRotate.transform.Rotate(0f, -dragDelta.x, 0f);
        _aimCam.transform.Rotate(dragDelta.y, 0f, 0f);
    }
}
