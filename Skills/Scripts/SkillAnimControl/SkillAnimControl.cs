using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimControl : MonoBehaviour
{
    [Header("Runtime Reference")]
    [SerializeField]
    protected CharacterAnimController _animControl;

    [Header("Config")]
    [SerializeField]
    protected GameObject _skillModelPrefab;

    private GameObject _modelObj;

    public void Setup(CharacterAnimController animControl)
    {
        _animControl = animControl;
    }

    public void AttachModel(Transform modelPos)
    {
        _modelObj = Instantiate(_skillModelPrefab, modelPos);
    }

    public void PlayArrnadeCast()
    {
        _animControl.PlayCastArrNade();
    }

    public void PlayDraw()
    {
        _animControl.PlayDraw();
    }

    public void PlayAim()
    {
        _animControl.PlayAim();
    }

    public void PlayShoot()
    {
        _animControl.PlayShoot();
    }
}
