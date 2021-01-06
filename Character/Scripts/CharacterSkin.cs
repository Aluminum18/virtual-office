using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkin : MonoBehaviour
{
    [SerializeField]
    private List<SkinnedMeshRenderer> _meshes;
    [SerializeField]
    private Material _team1Mat;
    [SerializeField]
    private Material _team2Mat;

    public void SetMaterial(int team)
    {
        for (int i = 0; i < _meshes.Count; i++)
        {
            _meshes[i].material = team == 1 ? _team1Mat : _team2Mat;
        }
    }
}
