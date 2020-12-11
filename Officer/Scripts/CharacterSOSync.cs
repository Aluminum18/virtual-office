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
    private CharacterAttribute _attribute;
    [SerializeField]
    private RoomInfoSO _roomInfo;

    private InputValueHolder _thisInputHolder;

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode != PhotonEventCode.CHARACTER_DIRECTION_SO_CHANGE &&
            eventCode != PhotonEventCode.CHARACTER_RAW_JOYSTICK_SO_CHANGE &&
            eventCode != PhotonEventCode.CHARACTER_AIM_SPOT_SO_CHANGE)
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
            default:
                {
                    return;
                }
        }
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

        PhotonNetwork.RaiseEvent(eventCode, data, eventOptions, sendOptions);
    }

    private void Start()
    {
        _attribute = GetComponent<CharacterAttribute>();
        _thisInputHolder = _inputHolders.GetInputValueHolder(_roomInfo.GetPlayerPos(_attribute.AssignedUserId));

        RegisterEvents();
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
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        if (_thisInputHolder == null)
        {
            return;
        }

        _thisInputHolder.JoyStickDirection.OnValueChange += OnDirectionChange;
        _thisInputHolder.JoyStickRaw.OnValueChange += OnRawJoystickChange;
        _thisInputHolder.AimSpot.OnValueChange += OnAimSpotChange;
    }

    private void UnregisterEvents()
    {
        if (_thisInputHolder == null)
        {
            return;
        }

        _thisInputHolder.JoyStickDirection.OnValueChange -= OnDirectionChange;
        _thisInputHolder.JoyStickRaw.OnValueChange -= OnRawJoystickChange;
        _thisInputHolder.AimSpot.OnValueChange -= OnAimSpotChange;
    }
}
