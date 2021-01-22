using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _thisClientUserId;
    [SerializeField]
    private PlayersInMapInfoSO _playerInMapInfo;
    [SerializeField]
    private RoomInfoSO _roomInfo;

    [Header("Config")]
    [SerializeField]
    private Transform _camFollow;
    [SerializeField]
    private Transform _camLook;
    [SerializeField]
    private Transform _aimLook;
    [SerializeField]
    private TargetableProjectileSpawner _arrowSpawner;
    [SerializeField]
    private CharacterAnimController _animController;
    public string AssignedUserId;

    public bool IsThisPlayer
    {
        get
        {
            var characterAtt = GetComponent<CharacterAttribute>();
            if (characterAtt == null)
            {
                return false;
            }

            if (!_thisClientUserId.Value.Equals(characterAtt.AssignedUserId))
            {
                return false;
            }
            return true;
        }
    }
    public Transform Follow
    {
        get
        {
            return _camFollow;
        }
    }
    public Transform Camlook
    {
        get
        {
            return _camLook;
        }
    }
    public Transform AimLook
    {
        get
        {
            return _aimLook;
        }

    }
    public CharacterAnimController AnimController
    {
        get
        {
            return _animController;
        }
    }
    public TargetableProjectileSpawner ArrowSpawner
    {
        get
        {
            return _arrowSpawner;
        }
    }

    private PlayerInMapInfo _inMapInfo;
    public PlayerInMapInfo InMapInfo
    {
        get
        {
            if (_inMapInfo == null)
            {
                _inMapInfo = _playerInMapInfo.GetPlayerInfo(_roomInfo.GetPlayerPos(AssignedUserId));
            }
            return _inMapInfo;
        }
    }

}
