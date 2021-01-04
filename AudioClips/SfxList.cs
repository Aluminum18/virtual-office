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

    public AudioClip GetClip(int index)
    {
        if (_clips.Count <= index)
        {
            Debug.Log($"invalid index [{index}]");
            return null;
        }
        return _clips[index];
    }
}
