using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _thisClientUserId;

    [Header("Config")]
    [SerializeField]
    private Transform _camFollow;
    [SerializeField]
    private Transform _camLook;
    [SerializeField]
    private Transform _aimLook;
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
}
