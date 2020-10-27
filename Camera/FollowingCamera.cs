using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform TranformToFollow;
    [SerializeField]
    private FollowingBoundary _boundary;
    [SerializeField]
    private CheckTargetTransform _checkingTargetTransform;
    [SerializeField]
    private GameEvent _checkingEvent;

    public Vector3 _offset;

    private Vector3 _position = Vector3.zero;

    private void Update()
    {
        if (TranformToFollow == null)
        {
            return;
        }

        if (_checkingTargetTransform.Equals(CheckTargetTransform.OnEvent))
        {
            return;
        }

        SetCamPosition();
    }

    private void SetCamPosition()
    {
        if (!TranformToFollow.hasChanged)
        {
            return;
        }

        _position = TranformToFollow.position + _offset;

        transform.position = _position;
    }

    private void Follow(params object[] args)
    {
        if (TranformToFollow == null)
        {
            return;
        }
        StartCoroutine(IE_Follow());
    }

    private IEnumerator IE_Follow()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        while (TranformToFollow.hasChanged)
        {
            SetCamPosition();
            yield return waitForEndOfFrame;
        }
    }

    private void OnEnable()
    {
        //_checkingEvent.Subcribe(Follow);
    }

    private void OnDisable()
    {
        //_checkingEvent.Unsubcribe(Follow);
    }
}

[System.Serializable]
public class FollowingBoundary
{
    public float left;
    public float right;
    public float top;
    public float bot;
}

public enum CheckTargetTransform
{
    Always,
    OnEvent
}
