using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ImpulseSource : MonoBehaviour
{
    [Header("Inspec")]
    [SerializeField]
    private static Camera _mainCamera;

    private CinemachineImpulseSource _impulseSource;
    private Camera MainCam
    {
        get
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }
            return _mainCamera;
        }
    }

    public void GenerateImpulse()
    {
        _impulseSource.GenerateImpulse(MainCam.transform.forward);
    }

    private void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

}
