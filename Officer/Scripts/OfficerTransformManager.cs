using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficerTransformManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private OfficerTranformsDBAccessor _dbAccessor;
    [SerializeField]
    private OfficersTransformList _transformList;

    [SerializeField]
    private List<AgentMoving> _officers;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onAnOfficerMoved;
    [SerializeField]
    private GameEvent _onAnOfficerStartMove;

    private Dictionary<string, AgentMoving> _officersMap = new Dictionary<string, AgentMoving>();
    private Dictionary<string, AgentMoving> OfficerMap
    {
        get
        {
            if (_officers.Count != _officersMap.Count)
            {
                _officersMap.Clear();
                for (int i = 0; i < _officers.Count; i++)
                {
                    var officer = _officers[i];
                    _officersMap[officer.Id] = officer;
                }
            }
            return _officersMap;
        }
    }

    public void AddAnOfficer(AgentMoving officer)
    {
        _officers.Add(officer);
        OfficerMap[officer.Id] = officer;
    }

    public void AskOfficerMoveTo(string id, Vector3 destination)
    {
        var officer = GetOfficerMoving(id);

        officer.MoveTo(destination);
    }

    public void OfficerTransformChangedHandler(OfficerTransform changedOfficer)
    {
        string officerId = changedOfficer.id;

        _transformList.UpdateOfficerDestination(officerId, changedOfficer.destination, out bool desChanged);
        if (desChanged)
        {
            AskOfficerMoveTo(officerId, changedOfficer.destination);
        }
    }

    public AgentMoving GetOfficerMoving(string id)
    {
        OfficerMap.TryGetValue(id, out var officer);
        if (officer == null)
        {
            Debug.LogError($"Cannot find officer with id [{id}]");
        }
        return officer;
    }

    private void RequestUpdateOfficerTransform(params object[] args)
    {
        var currentMoving = (AgentMoving)args[0];

        string id = currentMoving.Id;
        Vector3 currentPosition = currentMoving.transform.position;
        Vector3 currentDestination = currentMoving.Destination;

        Quaternion currentRotation = currentMoving.transform.rotation;
        Vector4 currentRotationV4 = new Vector4(currentRotation.x, currentRotation.y, currentRotation.z, currentRotation.w);

        _transformList.UpdateOfficerDestination(currentMoving.Id, currentDestination, out bool destinationChanged);
        if (destinationChanged)
        {
            _dbAccessor.UpdateOfficerDestination(id, currentDestination);
        }

        _transformList.UpdateOfficerPosition(id, currentPosition, out bool positionChanged);
        if (positionChanged)
        {
            _dbAccessor.UpdateOfficerPosition(id, currentPosition);
        }

        _transformList.UpdateOfficerRotation(id, currentRotationV4, out bool rotChanged);
        if (rotChanged)
        {
            _dbAccessor.UpdateOfficerRotation(id, currentRotationV4);
        }
    }

    private void RequestUpdateOfficerDestination(params object[] args)
    {
        string officerId = (string)args[0];

        var officer = GetOfficerMoving(officerId);

        if (officer == null)
        {
            return;
        }

        Vector3 destination = officer.Destination;

        _transformList.UpdateOfficerDestination(officerId, destination, out bool desChanged);
        if (desChanged)
        {
            _dbAccessor.UpdateOfficerDestination(officerId, destination);
        }
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        _onAnOfficerMoved.Subcribe(RequestUpdateOfficerTransform);
        _onAnOfficerStartMove.Subcribe(RequestUpdateOfficerDestination);
    }

    private void UnregisterEvents()
    {
        _onAnOfficerMoved.Unsubcribe(RequestUpdateOfficerTransform);
        _onAnOfficerStartMove.Unsubcribe(RequestUpdateOfficerDestination);
    }
}
