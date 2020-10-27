using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OfficerTransforms", menuName = "NFQ/Create OfficerTransforms")]
[System.Serializable]
public class OfficersTransformList : ScriptableObject
{
    private const float IGNORE_DISTANCE = 0.1f;
    private const float IGNORE_ANGLE = 5f;

    [SerializeField]
    private List<OfficerTransform> _officerTransforms;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onDataReady;

    private Dictionary<string, OfficerTransform> _tranformMap = new Dictionary<string, OfficerTransform>();
    private Dictionary<string, OfficerTransform> TransformMap
    {
        get
        {
            if (_officerTransforms.Count != _tranformMap.Count)
            {
                _tranformMap.Clear();
                for (int i = 0; i < _officerTransforms.Count; i++)
                {
                    var officerTrans = _officerTransforms[i];
                    _tranformMap[officerTrans.id] = officerTrans;
                }
            }
            return _tranformMap;
        }
    }

    public void Init(DataSnapshot infosFromServer)
    {
        _officerTransforms.Clear();
        _tranformMap.Clear();

        var officers = infosFromServer.Children;
        foreach (var officerSnapShot in officers)
        {
            var officer = JsonUtility.FromJson<OfficerTransform>(officerSnapShot.GetRawJsonValue());
            AddOfficer(officer);
        }

        Debug.Log("Officer transforms initialized!");
        _onDataReady.Raise();
    }

    public List<OfficerTransform> GetOfficers()
    {
        return _officerTransforms;
    }

    public void AddOfficer(OfficerTransform newOfficer)
    {
        _officerTransforms.Add(newOfficer);
        _tranformMap[newOfficer.id] = newOfficer;
    }

    public void UpdateOfficer(OfficerTransform changedOfficer)
    {
        string officerId = changedOfficer.id;
        UpdateOfficerDestination(officerId, changedOfficer.destination, out bool desChanged);
        UpdateOfficerPosition(officerId, changedOfficer.position, out bool posChanged);
        UpdateOfficerRotation(officerId, changedOfficer.rotation, out bool rotChanged);
    }

    public void UpdateOfficer(DataSnapshot changedOfficer)
    {
        var officer = JsonUtility.FromJson<OfficerTransform>(changedOfficer.GetRawJsonValue());
        UpdateOfficer(officer);
    }

    public void UpdateOfficerDestination(string id, Vector3 destination, out bool valueChanged)
    {
        var officer = GetOfficerTransform(id);
        if (officer == null)
        {
            valueChanged = false;
            return;
        }

        if (Vector3.Distance(destination, officer.destination) < IGNORE_DISTANCE)
        {
            valueChanged = false;
            return;
        }

        officer.destination = destination;
        valueChanged = true;
    }

    public void UpdateOfficerPosition(string id, Vector3 pos, out bool valueChanged)
    {
        var officer = GetOfficerTransform(id);
        if (officer == null)
        {
            valueChanged = false;
            return;
        }

        if (Vector3.Distance(pos, officer.position) < IGNORE_DISTANCE)
        {
            valueChanged = false;
            return;
        }

        officer.position = pos;
        valueChanged = true;
    }

    public void UpdateOfficerRotation(string id, Vector4 rot, out bool valueChanged)
    {
        var officer = GetOfficerTransform(id);
        if (officer == null)
        {
            valueChanged = false;
            return;
        }

        var currentRot = officer.rotation;

        Quaternion current = new Quaternion(currentRot.x, currentRot.y, currentRot.z, currentRot.w);
        Quaternion update = new Quaternion(rot.x, rot.y, rot.z, rot.w);
        if (Quaternion.Angle(current, update) < IGNORE_ANGLE)
        {
            valueChanged = false;
            return;
        }

        officer.rotation = rot;
        valueChanged = true;
    }

    public OfficerTransform GetOfficerTransform(string id)
    {
        TransformMap.TryGetValue(id, out var officerTrans);
        if (officerTrans == null)
        {
            Debug.LogError($"Cannot find officer with id [{id}] on map");
            return null;
        }

        return officerTrans;
    }
}
