using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhotonEventCode
{
    public const byte MANUAL_CHARACTER_SPAWNED = 0;
    public const byte LOAD_SCENE = 1;

    public const byte CHARACTER_DIRECTION_SO_CHANGE = 2;
    public const byte CHARACTER_RAW_JOYSTICK_SO_CHANGE = 3;
    public const byte CHARACTER_AIM_SPOT_SO_CHANGE = 4;
    public const byte CHARACTER_STATE_SO_CHANGE = 5;
    public const byte CHARACTER_ON_SHOOT = 6;
    public const byte CHARACTER_ON_AIM = 7;
    public const byte CHARACTER_ON_CANCEL_AIM = 8;
}
