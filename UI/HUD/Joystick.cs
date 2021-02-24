using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Reference")]
    [SerializeField]
    private Vector3Variable _rawInputJoystick;
    [SerializeField]
    private Vector3Variable _joystickDirection;

    [Header("Events out")]
    [SerializeField]
    private UnityEvent _onPointerDown;
    [SerializeField]
    private UnityEvent _onDrag;
    [SerializeField]
    private UnityEvent _onPointerUp;

    [Header("Config")]
    [Tooltip("Joystick up directs to forward")]
    [SerializeField]
    private bool _mapUpToForward;
    [SerializeField]
    private float _triggerDistance;
    [SerializeField]
    private float _litmitDistance;
    [SerializeField]
    private Camera _uiCam;
    [SerializeField]
    private Transform _worldCamTransform;
    [SerializeField]
    private Canvas _parentCanvas;

    private float _directionX = 0;
    private float _directionY = 0;

    private Vector3 _centerPos;
    private Vector3 _positionVector;
    private float A;
    private float B;

    private float Ai;
    private float Bi;
    private float Ci;
    private float _distanceSqr;

    private void Start()
    {
        _centerPos = transform.position;
        _distanceSqr = _litmitDistance * _litmitDistance;
    }

    public void SetDirectionOutHolder(Vector3Variable directionHolder)
    {
        _joystickDirection = directionHolder;
    }

    public void SetRawJoyStickInputHolder(Vector3Variable rawInputHolder)
    {
        _rawInputJoystick = rawInputHolder;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 touchPointToWorld = _uiCam.ScreenToWorldPoint(eventData.position);
        _positionVector = touchPointToWorld;
        _positionVector.z = touchPointToWorld.z + _parentCanvas.planeDistance;

        if (Vector3.Distance(_centerPos, _positionVector) < _triggerDistance)
        {
            _joystickDirection.Value = Vector3.zero;
            transform.position = _positionVector;
            return;
        }

        if (Vector3.Distance(_centerPos, _positionVector) >= _litmitDistance)
        {
            CalculateOverFlow(touchPointToWorld);
        }

        transform.position = _positionVector;
        _directionX = (transform.position.x - _centerPos.x) / _litmitDistance;
        _directionY = (transform.position.y - _centerPos.y) / _litmitDistance;

        _rawInputJoystick.Value = (_positionVector - _centerPos) / _litmitDistance;

        _joystickDirection.Value = _worldCamTransform.right * _directionX + 
           (_mapUpToForward ? _worldCamTransform.forward : _worldCamTransform.up) * _directionY;

        _onDrag.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _onPointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _onPointerUp.Invoke();
        BackToCenter();
    }

    private void CalculateOverFlow(Vector2 touchPoint)
    {
        float x = touchPoint.x - _centerPos.x;
        float y = touchPoint.y - _centerPos.y;

        if (x == 0f)
        {
            _positionVector.x = 0f;
            _positionVector.y = 1f * Mathf.Sign(y);
            return;
        }

        float tan = y / x;
        if (tan == 0f)
        {
            _positionVector.y = 0f;
            _positionVector.x = 1 * Mathf.Sign(x);
            return;
        }

        float tanSqr = Mathf.Pow(tan, 2);

        float xSqr = 1 / (tanSqr + 1);
        float ySqr = tanSqr * xSqr;

        _positionVector.x = Mathf.Sqrt(xSqr) * Mathf.Sign(x) * _litmitDistance + _centerPos.x;
        _positionVector.y = Mathf.Sqrt(ySqr) * Mathf.Sign(y) * _litmitDistance + _centerPos.y;
    }

    private void BackToCenter()
    {
        LeanTween.move(gameObject, _centerPos, 0.2f).setEase(LeanTweenType.linear);
        _joystickDirection.Value = Vector3.zero;
        _rawInputJoystick.Value = Vector3.zero;
    }

    private int NormalizeDirection(float value)
    {
        if (value > 0)
        {
            return 1;
        }
        else if (value < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }


}
