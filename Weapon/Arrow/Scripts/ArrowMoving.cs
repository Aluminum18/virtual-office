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
    private Transform _rootTransform;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private Rigidbody _rb;

    private static Vector3 _inactiveArrowPos = new Vector3(-99f, -99f, -99f);

    public void HeadTo(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void MoveForward(float speed)
    {
        _rb.velocity = transform.forward * speed;
        _onArrowStartMove.Invoke();
    }

    public void StopMoving()
    {
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
}
