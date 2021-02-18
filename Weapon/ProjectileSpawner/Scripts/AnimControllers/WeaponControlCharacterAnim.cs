using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControlCharacterAnim : MonoBehaviour
{
    [Header("Runtime reference")]
    [SerializeField]
    protected Animator _characterAnimator;
    [SerializeField]
    protected CharacterAnimController _animController;

    [Header("Config")]
    [SerializeField]
    private GameObject _weaponModelPrefab;
    [SerializeField]
    protected string _weaponAnimLayerName;
    [SerializeField]
    protected int _rigLayerIndex;
    [SerializeField]
    protected bool _enableBasicBowModel;

    private GameObject _weaponModelObj;

    public void SetAnimator(Animator animator)
    {
        _characterAnimator = animator;
    }

    public void SetAnimControl(CharacterAnimController controller)
    {
        _animController = controller;
    }

    public void AttachWeaponModelToCharacter(Transform weaponPos)
    {
        _weaponModelObj = Instantiate(_weaponModelPrefab, weaponPos);
    }

    public void ActiveWeaponLayer(bool active)
    {
        int weaponLayer = _characterAnimator.GetLayerIndex(_weaponAnimLayerName);
        if (weaponLayer == -1)
        {
            Debug.LogError($"Invalid weapon layer name [{_weaponAnimLayerName}]");
            return;
        }

        _characterAnimator.SetLayerWeight(weaponLayer, active ? 1f : 0f);
        _animController.ActiveRigLayer(_rigLayerIndex, active);

        _animController.ActiveBasicBowModel(_enableBasicBowModel);

        var rigInfo = _animController.GetRigLayerCustomInfo(_rigLayerIndex);
        if (rigInfo == null)
        {
            return;
        }

        Transform weaponTransform = rigInfo.WeaponModelPos;

        if (!active)
        {
            if (weaponTransform.childCount == 0)
            {
                return;
            }
            for (int i = 0; i < weaponTransform.childCount; i++)
            {
                Destroy(weaponTransform.GetChild(0));
            }
            return;
        }

        if (_enableBasicBowModel)
        {
            return;
        }
        AttachWeaponModelToCharacter(rigInfo.WeaponModelPos);
    }

    public virtual void PlayIdle()
    {
        _animController.PlayIdle();
    }

    public virtual void PlayRun()
    {
        _animController.PlayRun();
    }

    public virtual void PlayPrepareProjectile()
    {
        _characterAnimator.SetTrigger("Aim");
    }

    public virtual void PlayAttack()
    {
        _characterAnimator.SetTrigger("Shoot");
    }

    public virtual void PlayCancelAttack()
    {

    }

    public virtual void PlayReload()
    {
        _characterAnimator.SetTrigger("Reload");
    }

    private void OnDisable()
    {
        ActiveWeaponLayer(false);
    }
}
