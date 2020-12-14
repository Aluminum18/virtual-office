using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SfxList", menuName = "NFQ/Sound/Create SfxList")]
public class SfxList : ScriptableObject
{
    [SerializeField]
    private List<AudioClip> _clips;

    public AudioClip GetRamdomClip()
    {
        int clipCount = _clips.Count;
        if (clipCount == 0)
        {
            Debug.Log($"There is no clip in the list", this);
            return null;
        }
        return _clips[UnityEngine.Random.Range(0, clipCount)];
    }
}
