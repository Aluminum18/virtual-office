using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class ArrowMoving : MonoBehaviour
{
    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onArrowStartMove;
    [SerializeField]
    private UnityEvent _onArrowStop;

    [Header("Config")]
    [SerializeField]
    private LayerMask _blockBy;
    [SerializeField]
    private Transform _rootTransform;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private float _maxAngular = -10f;
    [SerializeField]
    private float _maxDistanceForMaxAngular = 30f;

    private float _rotateSpeed;
    private static Vector3 _inactiveArrowPos = new Vector3(-99f, -99f, -99f);

    private IDisposable _cancelRotateToTarget;
    private IDisposable _cancelDropAngle;

    public void CurveMoveToTarget(Vector3 destination, float speed)
    {
        Vector3 direction = destination - transform.position;

        Quaternion headToTarget = Quaternion.LookRotation(direction);
        transform.rotation = headToTarget;

        transform.Rotate(_maxAngular * direction.magnitude / _maxDistanceForMaxAngular, 0f , 0f);

        if (transform.rotation.eulerAngles.x < -90f)
        {
            transform.rotation = headToTarget;
        }

        _cancelRotateToTarget = IE_RotateToTarget(destination, speed).ToObservable().Subscribe();
    }

    public void CurveDropMove(Vector3 target, float speed, float dropAngleSpeed)
    {
        transform.rotation = Quaternion.LookRotation(target - transform.position);
        _cancelDropAngle = IE_DropAngle(speed, dropAngleSpeed).ToObservable().Subscribe();
    }

    public void MoveForward(Vector3 target, float speed)
    {
        transform.rotation = Quaternion.LookRotation(target - transform.position);

        _rb.velocity = transform.forward * speed;
        _onArrowStartMove.Invoke();
    }

    public void MoveForward(float speed)
    {
        _rb.velocity = transform.forward * speed;
        _onArrowStartMove.Invoke();
    }

    private IEnumerator IE_RotateToTarget(Vector3 target, float speed)
    {
        Quaternion rotateTo = Quaternion.LookRotation(target - transform.position);

        float distanceAngle = Quaternion.Angle(transform.rotation, rotateTo);
        float timeToTarget = (transform.position - target).magnitude / speed;
        _rotateSpeed = Quaternion.Angle(transform.rotation, rotateTo) / timeToTarget * 2f;

        _onArrowStartMove.Invoke();
        _rb.velocity = transform.forward * speed;

        while (distanceAngle > 1f)
        {
            _rb.velocity = transform.forward * speed;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, _rotateSpeed * Time.deltaTime);
            rotateTo = Quaternion.LookRotation(target - transform.position);
            yield return null;
        }

        transform.rotation = rotateTo;
    }

    private IEnumerator IE_DropAngle(float speed, float dropAngleSpeed)
    {
        _onArrowStartMove.Invoke();
        _rb.velocity = transform.forward * speed;

        float xAngle = transform.rotation.eulerAngles.x;
        while (Mathf.Sin(xAngle * Mathf.PI / 180f) < 0.99f)
        {
            _rb.velocity = transform.forward * speed;

            transform.Rotate(dropAngleSpeed * Time.deltaTime, 0f, 0f);
            yield return null;
            xAngle = transform.rotation.eulerAngles.x;
            Debug.Log(transform.rotation.eulerAngles);
        }
    }

    public void StopMoving()
    {
        if (_cancelRotateToTarget != null)
        {
            _cancelRotateToTarget.Dispose();
        }

        if (_cancelDropAngle != null)
        {
            _cancelDropAngle.Dispose();
        }

        _rb.velocity = Vector3.zero;
        _rootTransform.localPosition -= _rootTransform.forward * 0.5f;
        _onArrowStop.Invoke();
    }

    public void MoveToPoolPos()
    {
        _rb.velocity = Vector3.zero;
        _onArrowStop.Invoke();
        _rootTransform.localPosition = _inactiveArrowPos;
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & _blockBy) != 0)
        {
            Debug.Log($"Arrow blocked by [{other.name}]");
            StopMoving();
        }
    }
}
