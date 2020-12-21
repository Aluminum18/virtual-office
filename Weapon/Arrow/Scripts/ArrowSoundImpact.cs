using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSoundImpact : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private SfxList _nearMissClips;
    [SerializeField]
    private SfxList _hitObjectClips;
    [SerializeField]
    private SfxList _hitFleshClips;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private float _triggerNearMissDistance;

    private static Camera _mainCamera;
    private Rigidbody _rb;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        _rb = GetComponent<Rigidbody>();
    }

    public void PlayNearMiss()
    {
        _audioSource.PlayOneShot(_nearMissClips.GetRamdomClip());
    }

    public void PlayHitObject()
    {
        _audioSource.PlayOneShot(_hitObjectClips.GetRamdomClip());
    }

    public void PlayHitFlesh()
    {
        _audioSource.PlayOneShot(_hitFleshClips.GetRamdomClip());
    }

    public void CheckAndPlayNearMiss()
    {
        StartCoroutine(IE_CheckAndPlayNearMiss());
    }

    private IEnumerator IE_CheckAndPlayNearMiss()
    {
        float lastDistanceToMainCam = Vector3.Distance(gameObject.transform.localPosition, _mainCamera.gameObject.transform.localPosition);
        for (; ; )
        {
            yield return null;
            yield return null;
            yield return null;
            float currentDistanceToMainCam = Vector3.Distance(gameObject.transform.localPosition, _mainCamera.gameObject.transform.localPosition);
            Debug.Log("distance To mainCam" + currentDistanceToMainCam);

            if (currentDistanceToMainCam >= lastDistanceToMainCam)
            {
                yield break;
            }

            if (currentDistanceToMainCam < _triggerNearMissDistance)
            {
                PlayNearMiss();
                yield break;
            }
        }
    }
}
