using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OfficerTransform
{
    public string id;
    public Vector3 position;
    public Vector4 rotation;
    public Vector3 destination;
}

[System.Serializable]
public class OfficerTransformData
{
    public string id;
    public Dictionary<string, float> position = new Dictionary<string, float>();
    public Dictionary<string, float> rotation = new Dictionary<string, float>();
    public Dictionary<string, float> destination = new Dictionary<string, float>();

    public OfficerTransformData(OfficerTransform officerTransform)
    {
        id = officerTransform.id;

        position["x"] = officerTransform.position.x;
        position["y"] = officerTransform.position.y;
        position["z"] = officerTransform.position.z;

        destination["x"] = officerTransform.destination.x;
        destination["y"] = officerTransform.destination.y;
        destination["z"] = officerTransform.destination.z;

        rotation["x"] = officerTransform.rotation.x;
        rotation["y"] = officerTransform.rotation.y;
        rotation["z"] = officerTransform.rotation.z;
        rotation["w"] = officerTransform.rotation.w;
    }
}
