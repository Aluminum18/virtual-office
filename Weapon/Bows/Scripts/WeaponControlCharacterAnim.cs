using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControlCharacterAnim : MonoBehaviour
{
    [Header("Runtime reference")]
    [SerializeField]
    private CharacterAnimController _characterAnimControl;

    public void SetAnimControl(CharacterAnimController animControl)
    {
        _characterAnimControl = animControl;
    }

    public void PlayIdle(int targetLayer)
    {

    }
}
