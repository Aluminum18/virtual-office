using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FreeLookCam : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private CinemachineFreeLook _freelookCam;
    [SerializeField]
    private Camera _uiCam;
    [SerializeField]
    private UnityEvent _onPointerDown;

    private float _horizonRatio;
    private float _verticalRatio;

    public void ChangeVirtualCam(CinemachineFreeLook virtualCam)
    {
        _freelookCam = virtualCam;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _freelookCam.m_XAxis.Value += eventData.delta.x * _horizonRatio;
        _freelookCam.m_YAxis.Value -= eventData.delta.y * _verticalRatio;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _onPointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    private void Start()
    {
        _horizonRatio = 180f / Screen.width;
        _verticalRatio = 1f / Screen.height;
    }
}
