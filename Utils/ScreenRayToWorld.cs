using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRayToWorld : MonoBehaviour
{
    [Header("Runtime Reference")]
    [SerializeField]
    private Vector3Variable _output;

    [Header("Reference")]
    [SerializeField]
    private Vector3Variable _boundDirection;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private InputValueHolders _inputHolders;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onRequestUpdateAimSpot;

    [Header("Config")]
    [SerializeField]
    private RectTransform _actualCrosshairCenter;
    [SerializeField]
    private RectTransform _crosshairVisualCenter;
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

    public void ScreenRayToWorldPoint(object[] args)
    {
        Vector3 screenPoint = _uiCam.WorldToScreenPoint(_actualCrosshairCenter.position);
        var ray = _worldCam.ScreenPointToRay(screenPoint);

        //Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red, 0.1f);

        if (!Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            return;
        }

        _output.Value = hit.point;
    }

    private void ScreenRayToWorldBound()
    {
        Vector3 screenPoint = _uiCam.WorldToScreenPoint(_crosshairVisualCenter.position);
        var ray = _worldCam.ScreenPointToRay(screenPoint);

        //Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red, 0.1f);

        if (!Physics.Raycast(ray, out RaycastHit hit, 200f, 1 << 14))
        {
            return;
        }

        _boundDirection.Value = hit.point;
    }

    private void OnEnable()
    {
        _onRequestUpdateAimSpot.Subcribe(ScreenRayToWorldPoint);
    }

    private void Update()
    {
        ScreenRayToWorldBound();
    }

    private void OnDisable()
    {
        _onRequestUpdateAimSpot.Unsubcribe(ScreenRayToWorldPoint);
    }


}
