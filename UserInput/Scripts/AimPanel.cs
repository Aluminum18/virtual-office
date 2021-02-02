using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Cinemachine;

public class AimPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _thisUserId;

    [Header("Runtime Reference")]
    [SerializeField]
    private CharacterRotating _characterRotating;

    [Header("Config")]
    [SerializeField]
    private CinemachineVirtualCamera _aimLookCamera;
    [SerializeField]
    private float _sensitive;
    [SerializeField]
    private CharacterSpawner _spawner;

#if UNITY_EDITOR
    [Header("Testing")]
    public CharacterRotating _testRotating;
#endif

    private CharacterRotating CharacterRotating
    {
        get
        {
            if (_characterRotating == null)
            {
                var characterObj = _spawner.GetCharacterOnScene(_thisUserId.Value);
                if (characterObj == null)
                {
#if UNITY_EDITOR
                    _characterRotating = _testRotating;
                    return _characterRotating;
#endif
                    return null;
                }

                _characterRotating = characterObj.GetComponent<CharacterRotating>();
            }
            return _characterRotating;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.delta * Time.deltaTime;
        CharacterRotating.Rotate(0f, dragDelta.x * _sensitive, 0f);
        SetScreenX(dragDelta.y);
        Debug.Log("DragDelta " + dragDelta);
    }

    // Aim ScreenY of virtual camera range = (-0.5, 1.5)
    // YLook Angle of virtual camera is 70 degree
    float _ratio = 2f / 70f; 
    CinemachineComposer _aimComposer;
    CinemachineComposer AimCompose
    {
        get
        {
            if (_aimComposer == null)
            {
                _aimComposer = _aimLookCamera.GetCinemachineComponent<CinemachineComposer>();
            }
            return _aimComposer;
        }
    }
    private void SetScreenX(float dragDeltaY)
    {
        Debug.Log("DragDeltaY " + dragDeltaY);
        AimCompose.m_ScreenY = Mathf.Clamp(AimCompose.m_ScreenY + _ratio * dragDeltaY * _sensitive, -0.5f, 1.5f);
    }

    private void OnDisable()
    {
        AimCompose.m_ScreenY = 0.5f;
    }
}
