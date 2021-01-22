using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class World3dSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private float _minDistance = 1f;
    [SerializeField]
    private float _maxDistance = 20f;
    public void PlayOneShot(AudioClip audio)
    {
        if (audio == null)
        {
            return;
        }

        var audioSource = AudioSourcePool.Instance.GetAudioSource();
        audioSource.gameObject.transform.position = gameObject.transform.position;
        audioSource.maxDistance = _maxDistance;
        audioSource.minDistance = _minDistance;

        audioSource.PlayOneShot(audio);

        float audioLenght = audio.length;

        Observable.Timer(TimeSpan.FromSeconds(audio.length)).Subscribe(_ =>
        {
            audioSource.gameObject.SetActive(false);
        });
    }
}
