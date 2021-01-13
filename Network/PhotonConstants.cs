using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhotonEventCode
{
    // Common 0 - 19
    public const byte MANUAL_CHARACTER_SPAWNED = 0;
    public const byte LOAD_SCENE = 1;

    // Character 20 - 59
    public const byte CHARACTER_DIRECTION_SO_CHANGE = 20;
    public const byte CHARACTER_RAW_JOYSTICK_SO_CHANGE = 21;
    public const byte CHARACTER_AIM_SPOT_SO_CHANGE = 22;
    public const byte CHARACTER_STATE_SO_CHANGE = 23;
    public const byte CHARACTER_ON_SHOOT = 24;
    public const byte CHARACTER_ON_AIM = 25;
    public const byte CHARACTER_ON_CANCEL_AIM = 26;
    public const byte CHARACTER_ON_HP_CHANGED = 27;

    public const byte CHARACTER_ACTIVATE_SKILL = 28;

    // Room
    public const byte ROOM_PICKED_SKILLS_CHANGED = 99;


}
