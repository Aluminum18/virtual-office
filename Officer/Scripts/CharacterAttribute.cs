using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _userId;

    [Header("Config")]
    [SerializeField]
    private Transform _camFollow;
    [SerializeField]
    private Transform _camLook;
    [SerializeField]
    private Transform _aimLook;

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
    public string UserId
    {
        get
        {
            return _userId.Value;
        }
        set
        {
            _userId.Value = value;
        }
    }

}
