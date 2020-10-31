using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FreeLookCam : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private CinemachineFreeLook _freelookCam;

    private float _horizonRatio;
    private float _verticalRatio;

    private void Start()
    {
        _horizonRatio = 180f / Screen.width;
        _verticalRatio = 1f / Screen.height;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("delta" + eventData.delta);
        _freelookCam.m_XAxis.Value += eventData.delta.x * _horizonRatio;
        _freelookCam.m_YAxis.Value -= eventData.delta.y * _verticalRatio;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
