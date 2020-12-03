using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class AgentMoving : MonoBehaviour
{
    [SerializeField]
    private string _id;
    public string Id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onStartMove;
    [SerializeField]
    private GameEvent _onFinishMove;

    [Header("Config")]
    [SerializeField]
    private NavMeshAgent _agent;

    private Vector3 _destination = Vector3.zero;
    public Vector3 Destination
    {
        get
        {
            return _destination;
        }
    }

    private void Start()
    {
        _agent.updatePosition = false;
        _agent.updateRotation = false;
    }

    /// <summary>
    /// RaiseEventWhenMoved when needing to notify server about this move
    /// </summary>
    public void MoveTo(Vector3 destination)
    {
        StopAllCoroutines();

        _destination.x = destination.x;
        _destination.y = destination.y + _agent.height / 2f;
        _destination.z = destination.z;

        _agent.destination = _destination;

        StartCoroutine(IE_MoveTo());
        StartCoroutine(IE_RotateTo());
    }

    /// <summary>
    /// notifyMoved when needing to notify server about this move
    /// </summary>
    public void MoveTo(Transform destination)
    {
        MoveTo(destination.position);
    }

    private bool _finishMove = false;
    private IEnumerator IE_MoveTo()
    {
 
        _onStartMove.Raise(Id);

        while (Vector3.Distance(_destination, transform.position) > 0.1f)
        {
            transform.position = _agent.nextPosition;
            yield return new WaitForEndOfFrame();
        }
        transform.position = _destination;

        _finishMove = true;
        _agent.velocity = Vector3.zero;

        CheckAndRaiseFinishMoveToEvent();
    }

    private bool _finishRotate = false;
    private IEnumerator IE_RotateTo()
    {
        Quaternion currentDesRotation;
        Vector3 relativePos;
        Quaternion rotateToTarget;

        Quaternion desRotation = Quaternion.LookRotation(_destination - transform.position);


        while(Quaternion.Angle(transform.rotation, desRotation) > 5f)
        {
            relativePos = _agent.steeringTarget - transform.position;
            currentDesRotation = Quaternion.LookRotation(relativePos);
            rotateToTarget = Quaternion.RotateTowards(transform.rotation, currentDesRotation, _agent.angularSpeed * Time.deltaTime);

            transform.rotation = rotateToTarget;
            yield return new WaitForEndOfFrame();
        }

        transform.rotation = desRotation;
        _finishRotate = true;

        CheckAndRaiseFinishMoveToEvent();
    }

    private void CheckAndRaiseFinishMoveToEvent()
    {
        if (_finishMove && _finishRotate)
        {
            _finishMove = false;
            _finishRotate = false;
            _onFinishMove.Raise(this);
        }
    }
}
