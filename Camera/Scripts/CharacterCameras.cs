using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CharacterCameras : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private CharacterSpawner _characterSpawner;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onPlayerDefeated;

    [Header("Config")]
    [SerializeField]
    private CinemachineFreeLook _freeLookCam;
    [SerializeField]
    private CinemachineVirtualCamera _aimLookCam;
    [SerializeField]
    private float _inspecCamSpeed = 5f;

    private CompositeDisposable _disposables = new CompositeDisposable();
    private CharacterAttribute _characterAtt;

    public void FindThisPlayerAndFollow()
    {
        var character = _characterSpawner.GetCharacterOnScene(_userId.Value);
        if (character == null)
        {
            Debug.LogWarning($"Cannot find this player with id [{_userId.Value}]", this);
            return;
        }

        _characterAtt = character.GetComponent<CharacterAttribute>();
        if (_characterAtt == null)
        {
            Debug.LogWarning($"Missing CharacterAttribute of [{character.name}]", character);
            return;
        }

        _freeLookCam.Follow = _characterAtt.Camlook;
        _freeLookCam.LookAt = _characterAtt.Camlook;

        _aimLookCam.Follow = _characterAtt.Camlook;
        _aimLookCam.LookAt = _characterAtt.Camlook;
    }

    private void ActiveSpecCam(object[] defeatedPlayer)
    {
        //if ((string)defeatedPlayer[0] != _userId.Value)
        //{
        //    return;
        //}
        _aimLookCam.gameObject.SetActive(false);
        _freeLookCam.gameObject.SetActive(true);


    }

    private void OnEnable()
    {
        _onPlayerDefeated.Subcribe(ActiveSpecCam);
    }

    private void OnDisable()
    {
        _disposables.Clear();
        _onPlayerDefeated.Unsubcribe(ActiveSpecCam);
    }
}
