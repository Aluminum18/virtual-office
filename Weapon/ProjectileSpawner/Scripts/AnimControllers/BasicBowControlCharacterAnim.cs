using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBowControlCharacterAnim : WeaponControlCharacterAnim
{
    private void OnEnable()
    {
        ActiveWeaponLayer(true);
    }
}
