using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSOSync : MonoBehaviour, IOnEventCallback
{
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private InputValueHolders _inputHolders;
    [SerializeField]
    private PlayersInMapInfoSO _playersInMapInfo;
    [SerializeField]
    private CharacterAttribute _attribute;
    [SerializeField]
    private RoomInfoSO _roomInfo;

    private InputValueHolder _thisInputHolder;
    private PlayerInMapInfo _thisPlayerInMapInfo;

    public void RegisterInput()
    {
        _attribute = GetComponent<CharacterAttribute>();
        int playerPos = _roomInfo.GetPlayerPos(_attribute.AssignedUserId);
        _thisInputHolder = _inputHolders.GetInputValueHolder(playerPos);
        _thisPlayerInMapInfo = _playersInMapInfo.GetPlayerInfo(playerPos);

        if (_thisInputHolder == null ||
            _thisPlayerInMapInfo ==  null)
        {
            return;
        }

        // Input
        _thisInputHolder.CharacterState.OnValueChange += OnStateChange;
        _thisInputHolder.JoyStickDirection.OnValueChange += OnDirectionChange;
        _thisInputHolder.JoyStickRaw.OnValueChange += OnRawJoystickChange;
        _thisInputHolder.AimSpot.OnValueChange += OnAimSpotChange;

        _thisInputHolder.OnShoot.Subcribe(OnShoot);
        _thisInputHolder.OnAim.Subcribe(OnAim);
        _thisInputHolder.OnCancelAim.Subcribe(OnCancelAim);

        // Info
        _thisPlayerInMapInfo.Hp.OnValueChange += OnHpChanged;
    }

    public void OnEvent(EventData photonEvent)
    {

        byte eventCode = photonEvent.Code;

        if (eventCode != PhotonEventCode.CHARACTER_DIRECTION_SO_CHANGE &&
            eventCode != PhotonEventCode.CHARACTER_RAW_JOYSTICK_SO_CHANGE &&
            eventCode != PhotonEventCode.CHARACTER_AIM_SPOT_SO_CHANGE &&
            eventCode != PhotonEventCode.CHARACTER_ON_SHOOT &&
            eventCode != PhotonEventCode.CHARACTER_ON_AIM &&
            eventCode != PhotonEventCode.CHARACTER_ON_CANCEL_AIM &&
            eventCode != PhotonEventCode.CHARACTER_ON_HP_CHANGED)
        {
            return;
        }

        object[] data = (object[])photonEvent.CustomData;
        string changedUserId = (string)data[0];
        if (_attribute.AssignedUserId != changedUserId)
        {
            return;
        }

        switch (eventCode)
        {
            case PhotonEventCode.CHARACTER_DIRECTION_SO_CHANGE:
                {
                    UpdateDirectionChange((Vector3)data[1]);
                    break;
                }
            case PhotonEventCode.CHARACTER_RAW_JOYSTICK_SO_CHANGE:
                {
                    UpdateRawJoystickChange((Vector3)data[1]);
                    break;
                }
            case PhotonEventCode.CHARACTER_AIM_SPOT_SO_CHANGE:
                {
                    UpdateAimSpotChange((Vector3)data[1]);
                    break;
                }
            case PhotonEventCode.CHARACTER_STATE_SO_CHANGE:
                {
                    UpdateState((string)data[1]);
                    break;
                }
            case PhotonEventCode.CHARACTER_ON_SHOOT:
                {
                    OnShootResponse();
                    break;
                }
            case PhotonEventCode.CHARACTER_ON_AIM:
                {
                    OnAimResponse();
                    break;
                }
            case PhotonEventCode.CHARACTER_ON_CANCEL_AIM:
                {
                    OnCancelAimResponse();
                    break;
                }
            case PhotonEventCode.CHARACTER_ON_HP_CHANGED:
                {
                    OnHpChanged((float)data[1]);
                    break;
                }
            default:
                {
                    return;
                }
        }
    }

    private void OnStateChange(string newState)
    {
        RaiseSOChangeEvent(PhotonEventCode.CHARACTER_STATE_SO_CHANGE, newState);
    }
    private void UpdateState(string newState)
    {
        _thisInputHolder.CharacterState.Value = newState;
    }

    private void OnDirectionChange(Vector3 newDirection)
    {
        RaiseSOChangeEvent(PhotonEventCode.CHARACTER_DIRECTION_SO_CHANGE, newDirection);
    }
    private void UpdateDirectionChange(Vector3 newDirection)
    {
        Debug.Log("Update direction");
        _thisInputHolder.JoyStickDirection.Value = newDirection;
    }

    private void OnRawJoystickChange(Vector3 newRawJoystick)
    {
        RaiseSOChangeEvent(PhotonEventCode.CHARACTER_RAW_JOYSTICK_SO_CHANGE, newRawJoystick);

    }
    private void UpdateRawJoystickChange(Vector3 newRawJoystick)
    {
        _thisInputHolder.JoyStickRaw.Value = newRawJoystick;
    }

    private void OnAimSpotChange(Vector3 newAimSpot)
    {
        RaiseSOChangeEvent(PhotonEventCode.CHARACTER_AIM_SPOT_SO_CHANGE, newAimSpot);
    }
    private void UpdateAimSpotChange(Vector3 newAimSpot)
    {
        _thisInputHolder.AimSpot.Value = newAimSpot;
    }

    private void OnShoot(params object[] args)
    {
        RaiseSOChangeEvent(PhotonEventCode.CHARACTER_ON_SHOOT, null);
    }
    private void OnShootResponse()
    {
        _thisInputHolder.OnShoot.Raise();
    }

    private void OnAim(params object[] args)
    {
        RaiseSOChangeEvent(PhotonEventCode.CHARACTER_ON_AIM, null);
    }
    private void OnAimResponse()
    {
        _thisInputHolder.OnAim.Raise();
    }

    private void OnCancelAim(params object[] args)
    {
        RaiseSOChangeEvent(PhotonEventCode.CHARACTER_ON_CANCEL_AIM, null);
    }
    private void OnCancelAimResponse()
    {
        _thisInputHolder.OnCancelAim.Raise();
    }

    private void OnHpChanged(float newHp)
    {
        RaiseSOChangeEvent(PhotonEventCode.CHARACTER_ON_HP_CHANGED, newHp);
    }
    private void UpdateHpChanged(float newHp)
    {
        _thisPlayerInMapInfo.Hp.Value = newHp;
    }

    private void RaiseSOChangeEvent(byte eventCode, object eventData)
    {
        if (!_attribute.IsThisPlayer)
        {
            return;
        }

        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning($"Photon is not connected", gameObject);
            return;
        }
        var photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogError($"Missing PhotonView", gameObject);
            return;
        }

        object[] data = new object[]
        {
            _userId.Value,
            eventData
        };

        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };

        Debug.Log($"Send event [{eventCode}]");
        PhotonNetwork.RaiseEvent(eventCode, data, eventOptions, sendOptions);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void OnDestroy()
    {
        UnregisterInput();
    }

    private void UnregisterInput()
    {
        if (_thisInputHolder == null ||
            _thisPlayerInMapInfo == null)
        {
            return;
        }

        // input
        _thisInputHolder.CharacterState.OnValueChange -= OnStateChange;
        _thisInputHolder.JoyStickDirection.OnValueChange -= OnDirectionChange;
        _thisInputHolder.JoyStickRaw.OnValueChange -= OnRawJoystickChange;
        _thisInputHolder.AimSpot.OnValueChange -= OnAimSpotChange;

        _thisInputHolder.OnShoot.Unsubcribe(OnShoot);
        _thisInputHolder.OnAim.Unsubcribe(OnAim);
        _thisInputHolder.OnCancelAim.Unsubcribe(OnCancelAim);

        // info
        _thisPlayerInMapInfo.Hp.OnValueChange -= OnHpChanged;
    }
}
