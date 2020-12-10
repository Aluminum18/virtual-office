using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRayToWorld : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Vector3Variable _output;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private InputValueHolders _inputHolders;

    [Header("Config")]
    [SerializeField]
    private RectTransform _screenTransform;
    [SerializeField]
    private Camera _worldCam;
    [SerializeField]
    private Camera _uiCam;

    public void SetOutput()
    {
        var holder = _inputHolders.GetInputValueHolder(_roomInfo.GetPlayerPos(_userId.Value));
        if (holder == null)
        {
            Debug.Log($"Cannot assign output for userId [{_userId.Value}]");
            return;
        }
        _output = holder.AimSpot;
    }

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
