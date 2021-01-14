using System.Collections;
using System.Collections.Generic;
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
    private Collider _collider;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private float _maxAngular = -10f;
    [SerializeField]
    private float _maxDistanceForMaxAngular = 30f;

    private float _rotateSpeed;
    private static Vector3 _inactiveArrowPos = new Vector3(-99f, -99f, -99f);

    public void HeadTo(Vector3 destination, float speed)
    {
        Vector3 direction = destination - transform.position;

        Quaternion headToTarget = Quaternion.LookRotation(direction);
        transform.rotation = headToTarget;

        transform.Rotate(_maxAngular * direction.magnitude / _maxDistanceForMaxAngular, 0f , 0f);

        if (transform.rotation.eulerAngles.y < -90f)
        {
            transform.rotation = headToTarget;
        }

        StartCoroutine(IE_RotateToTarget(destination, speed));
    }

    public void HeadForward(float speed)
    {
        _rb.velocity = transform.forward * speed;
        _onArrowStartMove.Invoke();
    }

    private IEnumerator IE_RotateToTarget(Vector3 target, float speed)
    {
        yield return null;
        Quaternion rotateTo = Quaternion.LookRotation(target - transform.position);

        float distanceAngle = Quaternion.Angle(transform.rotation, rotateTo);
        float timeToTarget = (transform.position - target).magnitude / speed;
        _rotateSpeed = Quaternion.Angle(transform.rotation, rotateTo) / timeToTarget * 2f;

        _onArrowStartMove.Invoke();

        while (distanceAngle > 1f)
        {
            Debug.Log("rotate " + Quaternion.Angle(transform.rotation, rotateTo));
            _rb.velocity = transform.forward * speed;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, _rotateSpeed * Time.deltaTime);
            rotateTo = Quaternion.LookRotation(target - transform.position);
            yield return null;
        }

        transform.rotation = rotateTo;
    }

    public void StopMoving()
    {
        StopAllCoroutines();

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
            StopMoving();
        }
    }
}
