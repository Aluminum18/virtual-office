using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCameras : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private CharacterSpawner _characterSpawner;
    [SerializeField]
    private Vector3 _moveDirection;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onPlayerDefeated;

    [Header("Config")]
    [SerializeField]
    private CinemachineFreeLook _freeLookCam;
    [SerializeField]
    private CinemachineVirtualCamera _aimLookCam;

    public void FindThisPlayerAndFollow()
    {
        var character = _characterSpawner.GetCharacterOnScene(_userId.Value);
        if (character == null)
        {
            Debug.LogWarning($"Cannot find this player with id [{_userId.Value}]", this);
            return;
        }

        var characterAtt = character.GetComponent<CharacterAttribute>();
        if (characterAtt == null)
        {
            Debug.LogWarning($"Missing CharacterAttribute of [{character.name}]", character);
            return;
        }

        _freeLookCam.Follow = characterAtt.Camlook;
        _freeLookCam.LookAt = characterAtt.Camlook;

        _aimLookCam.Follow = characterAtt.Camlook;
        _aimLookCam.LookAt = characterAtt.Camlook;
    }

    private void ActiveSpecCam(object[] defeatedPlayer)
    {
        if ((string)defeatedPlayer[0] != _userId.Value)
        {
            return;
        }
        _aimLookCam.gameObject.SetActive(false);
        _freeLookCam.gameObject.SetActive(true);
    }

    private void ActiveSpecMove()
    {

    }

    private void OnEnable()
    {
        //_onPlayerDefeated.Subcribe(ActiveSpecCam);
    }

    private void OnDisable()
    {
        //_onPlayerDefeated.Unsubcribe(ActiveSpecCam);
    }
}
