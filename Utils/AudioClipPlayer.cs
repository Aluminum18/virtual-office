using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private SfxList _playList;

    public void PlayAClip(int index)
    {
        _audioSource.PlayOneShot(_playList.GetClip(index));
    }

    public void PlayRandomFromList()
    {
        _audioSource.PlayOneShot(_playList.GetRamdomClip());
    }
}
