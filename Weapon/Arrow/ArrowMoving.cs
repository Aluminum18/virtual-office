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

    public bool IsMoving { get; set; }

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit target: " + other.name, other.gameObject);

        StopMoving();
    }

    private void StopMoving()
    {
        _rb.velocity = Vector3.zero;
        _rootTransform.position += _rootTransform.forward * 0.1f;
        _onArrowStop.Invoke();
    }
}
