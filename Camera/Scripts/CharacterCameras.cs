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

        _freeLookCam.Follow = characterAtt.Follow;
        _freeLookCam.LookAt = characterAtt.Camlook;

        _aimLookCam.Follow = characterAtt.Follow;
        _aimLookCam.LookAt = characterAtt.AimLook;
    }
}
