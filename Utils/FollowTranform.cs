using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTranform : MonoBehaviour
{
    [SerializeField]
    private Transform _transformToFollow;
    [SerializeField]
    private Vector3 _offset;

    public void SetTransformToFollow(Transform target)
    {
        _transformToFollow = target;
    }

    private void Update()
    {
        if (_transformToFollow == null)
        {
            return;
        }

        if (!_transformToFollow.hasChanged)
        {
            return;
        }
        //_transformToFollow.hasChanged = false;

        Follow();
    }

    private void Follow()
    {
        transform.position = _transformToFollow.position + _offset;
    }
}