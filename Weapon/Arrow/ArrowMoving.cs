using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMoving : MonoBehaviour
{
    [SerializeField]
    private Transform _rootTransform;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private Joint _joint;
    [SerializeField]
    private Rigidbody _rb;

    public void HeadTo(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void MoveForward(float speed)
    {
        _rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit target: " + other.name, other.gameObject);
        //var hitBody = other.GetComponent<Rigidbody>();
        //if (hitBody != null)
        //{
        //    JointToHit(hitBody);
        //    return;
        //}

        StopMoving();
    }

    private void JointToHit(Rigidbody rb)
    {
        _joint.connectedBody = rb;
    }

    private void StopMoving()
    {
        _rb.velocity = Vector3.zero;
        _rootTransform.position += _rootTransform.forward * 0.1f;
    }
}
