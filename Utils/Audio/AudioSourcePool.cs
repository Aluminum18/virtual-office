using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class AudioSourcePool : MonoSingleton<AudioSourcePool>
{
    [SerializeField]
    private ObjectPool _audioSourcePool;

    public AudioSource GetAudioSource()
    {
        var audioSource = _audioSourcePool.Spawn().GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSourcePool config error!");
            return null;
        }

        return audioSource;
    }


}
