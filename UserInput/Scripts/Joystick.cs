using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private float _litmitDistance;
    [SerializeField]
    private Camera _uiCam;
    [SerializeField]
    private Canvas _parentCanvas;

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

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 touchPointToWorld = _uiCam.ScreenToWorldPoint(eventData.position);
        _positionVector = touchPointToWorld;
        _positionVector.z = touchPointToWorld.z + _parentCanvas.planeDistance;

        if (Vector3.Distance(_centerPos, _positionVector) >= _litmitDistance)
        {
            Debug.Log("touchToWorld" + touchPointToWorld);
            //BackToCenter();
            CalculateOverFlow(touchPointToWorld);
        }

        transform.position = _positionVector;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        BackToCenter();
    }

    Vector2 distance1 = Vector2.zero;
    Vector2 distance2 = Vector2.zero;
    private void CalculateOverFlow(Vector2 touchPoint)
    {
        float xAO = touchPoint.x - _centerPos.x;
        A = (touchPoint.y - _centerPos.y) / xAO;
        B = (_centerPos.x * touchPoint.y - touchPoint.x * _centerPos.y) / xAO;
        Ai = 1 + A * A;
        Bi = -2 * (_centerPos.x + A * (B + _centerPos.y));
        Ci = Mathf.Pow(_centerPos.x, 2) + Mathf.Pow(B + _centerPos.y, 2) - _distanceSqr;

        float delta = Bi * Bi - 4 * Ai * Ci;

        if (delta < 0)
        {
            return;
        }

        float x1 = (-Bi + Mathf.Sqrt(delta)) / (2 * Ai);
        float y1 = A * x1 - B;
        distance1.x = x1 - touchPoint.x;
        distance1.y = y1 - touchPoint.y;

        float x2 = (-Bi - Mathf.Sqrt(delta)) / (2 * Ai);
        float y2 = A * x2 - B;
        distance2.x = x2 - touchPoint.x;
        distance2.y = y2 - touchPoint.y;

        if (distance1.sqrMagnitude < distance2.sqrMagnitude)
        {
            _positionVector.x = x1;
            _positionVector.y = y1;
        }
        else
        {
            _positionVector.x = x2;
            _positionVector.y = y2;
        }
    }

    private void BackToCenter()
    {
        LeanTween.move(gameObject, _centerPos, 0.2f).setEase(LeanTweenType.linear);
    }


}
