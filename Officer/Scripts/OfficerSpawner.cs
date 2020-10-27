using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OfficerSpawner : MonoBehaviour
{
    [SerializeField]
    private OfficersTransformList _transformList;
    [SerializeField]
    private OfficerTransformManager _transformManager;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onOfficerTransformDataReady;

    [Header("Events out")]
    [SerializeField]
    private UnityEvent _onAllOfficersSpawned;

    [Header("Config")]
    [SerializeField]
    private GameObject _officerPrefab;

    private void OnEnable()
    {
        _onOfficerTransformDataReady.Subcribe(SpawnOfficer);
    }

    private void OnDisable()
    {
        _onOfficerTransformDataReady.Unsubcribe(SpawnOfficer);
    }

    private void SpawnOfficer(params object[] args)
    {
        List<OfficerTransform> officers = _transformList.GetOfficers();
        for (int i = 0; i < officers.Count; i++)
        {
            var officer = officers[i];
            var rotation = officer.rotation;
            Quaternion rot = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);

            var officerObject = Instantiate(_officerPrefab, officer.position, rot);
            var officerMoving = officerObject.GetComponent<AgentMoving>();
            officerMoving.Id = officer.id;
            _transformManager.AddAnOfficer(officerMoving);
        }
        _onAllOfficersSpawned?.Invoke();
    }
}
