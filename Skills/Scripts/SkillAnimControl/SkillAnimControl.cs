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
#if UNITY_EDITOR
        if (_animControl == null)
        {
            return;
        }
#endif
        _animControl.PlayCastArrNade();
    }

    public void PlayDraw()
    {
#if UNITY_EDITOR
        if (_animControl == null)
        {
            return;
        }
#endif
        _animControl.PlayDraw();
    }

    public void PlayAim()
    {
#if UNITY_EDITOR
        if (_animControl == null)
        {
            return;
        }
#endif
        _animControl.PlayAim();
    }

    public void PlayShoot()
    {
#if UNITY_EDITOR
        if (_animControl == null)
        {
            return;
        }
#endif
        _animControl.PlayShoot();
    }
}
