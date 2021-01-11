using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RealTimeDBAccessor))]
public class OfficerTranformsDBAccessor : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private OfficersTransformList _officerTransformList;
    [SerializeField]
    private OfficerTransformManager _officerTransformManager;

    [Header("Events out")]
    [SerializeField]
    private UnityEvent _onOfficerTransformFetched;

    [Header("Configs")]
    [SerializeField]
    private RealTimeDBAccessor _dbAccessor;

    private StringBuilder _sb = new StringBuilder();

    public void Init()
    {
        ListenOfficerTranformChanged();
    }

    public void FetchOfficerTransformInfos()
    {
        _dbAccessor.GetData(OfficerDBConstants.OFFICERS_TRANSFORM,
            officerTranforms =>
            {
                _officerTransformList.Init(officerTranforms);
            });
    }

    public void UpdateAnOfficerTransform(OfficerTransformData officer)
    {
        _sb.Clear();

        string childKey = _sb.Append(OfficerDBConstants.OFFICERS_TRANSFORM).Append("/").Append(officer.id).ToString();
        _dbAccessor.UpdateAChildWithCustomData(childKey, officer);
    }

    public void UpdateOfficerPosition(string officerId, Vector3 pos)
    {
        _sb.Clear();

        string posChildKey = _sb.Append(OfficerDBConstants.OFFICERS_TRANSFORM)
            .Append("/").Append(officerId)
            .Append("/").Append("position").ToString();
        _dbAccessor.UpdateAChildWithCustomData(posChildKey, pos);
    }

    public void UpdateOfficerRotation(string officerId, Vector4 rot)
    {
        _sb.Clear();

        string rotChildKey = _sb.Append(OfficerDBConstants.OFFICERS_TRANSFORM)
            .Append("/").Append(officerId)
            .Append("/").Append("rotation").ToString();
        _dbAccessor.UpdateAChildWithCustomData(rotChildKey, rot);
    }

    public void UpdateOfficerDestination(string officerId, Vector3 destination)
    {
        _sb.Clear();
        string childKey = _sb.Append(OfficerDBConstants.OFFICERS_TRANSFORM)
            .Append("/").Append(officerId)
            .Append("/").Append("destination").ToString();
        _dbAccessor.UpdateAChildWithCustomData(childKey, destination);
    }

    private void ListenOfficerTranformChanged()
    {
        _dbAccessor.ListenChildChange(OfficerDBConstants.OFFICERS_TRANSFORM, 
            (sender, changedChild) =>
            {
                OfficerTransform changedOfficer = JsonUtility.FromJson<OfficerTransform>(changedChild.Snapshot.GetRawJsonValue());
                _officerTransformManager.OfficerTransformChangedHandler(changedOfficer);
            });
    }
}
