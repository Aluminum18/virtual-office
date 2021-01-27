using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigLayerCustomInfo : MonoBehaviour
{
    [SerializeField]
    private Transform _weaponModelPos;

    public Transform WeaponModelPos
    {
        get
        {
            return _weaponModelPos;
        }
    }
}
