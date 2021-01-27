using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillAction : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _thisUserId;
    [SerializeField]
    private IntegerListVariable _thisPickedSkills;
    [SerializeField]
    private SkillListSO _skillList;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onActivateSkill;

    [Header("Config")]
    [SerializeField]
    private Transform _skillObjTransform;
    [SerializeField]
    private CharacterAction _characterAction;

    private CharacterAttribute _attribute;
    private Dictionary<int, GameObject> _skillObjectMap = new Dictionary<int, GameObject>();

    private void SetUpSkills()
    {
        _skillObjectMap.Clear();

        List<int> pickedSkills = _thisPickedSkills.List;

        if (pickedSkills == null)
        {
            return;
        }

        for (int i = 0; i < pickedSkills.Count; i++)
        {
            SkillId skillId = (SkillId)pickedSkills[i];
            var skillObj = _skillList.GetSkillActivationObject(skillId);
            if (skillObj == null)
            {
                continue;
            }

            _skillObjectMap[(int)skillId] = Instantiate(skillObj, _skillObjTransform);

            if (_skillList.GetSkillType(skillId).Equals(SkillType.Passive))
            {
                ActivateSkill(skillId, SkillState.First, null);
            }
        }
    }

    private void ActivateSkill(SkillId skillId, SkillState skillState, object[] skillData)
    {
        switch (skillId)
        {
            case SkillId.ArrNade:
                {
                    _skillObjectMap.TryGetValue((int)skillId, out var skillObj);
                    if (skillObj == null)
                    {
                        return;
                    }

                    if (skillState.Equals(SkillState.Second))
                    {
                        skillObj.GetComponent<ArrNadeSpawner>()?.ExploseArrnade();
                    }
                    else
                    {
                        skillObj.GetComponent<ArrNadeSpawner>()?.SpawnArrnade();
                    }

                    break;
                }
            case SkillId.ThirdEye:
                {
                    if (_thisUserId.Value != _attribute.AssignedUserId)
                    {
                        return;
                    }

                    _skillObjectMap.TryGetValue((int)skillId, out var skillObj);
                    if (skillObj == null)
                    {
                        return;
                    }

                    skillObj.GetComponent<ThirdEyeActivation>()?.ActivateThirdEye(true);
                    break;
                }
            case SkillId.Crossbow:
                {
                    var skillObject = _skillObjectMap[(int)skillId];
                    if (skillObject == null)
                    {
                        return;
                    }
                    var crossbowObject = skillObject.GetComponent<RangeTargetableWeapon>();
                    if (crossbowObject == null)
                    {
                        return;
                    }
                    _characterAction.SetWeapon(crossbowObject);
                    crossbowObject.SetTarget(_attribute.AimSpot);

                    var crossbowAnimControl = skillObject.GetComponent<CrossbowControlCharacterAnim>();
                    if (crossbowAnimControl == null)
                    {
                        return;
                    }
                    crossbowAnimControl.SetAnimator(_attribute.Animator);
                    crossbowAnimControl.SetAnimControl(_attribute.AnimController);
                    crossbowAnimControl.ActiveWeaponLayer(true);
                    break;
                }
            default:
                {
                    return;
                }
        }
    }

    private void HandleActivateSkill(object[] eventParam)
    {
        if (!(eventParam[0] is string userId))
        {
            userId = "";
        }

        string thisCharacterUserId = _attribute.AssignedUserId;
        if (!userId.Equals(thisCharacterUserId))
        {
            return;
        }

        if (!(eventParam[1] is SkillId skillId))
        {
            return;
        }
        
        if (!(eventParam[2] is SkillState skillState))
        {
            return;
        }

        if (!(eventParam[3] is object[] skillData))
        {
            skillData = null;
        }

        ActivateSkill(skillId, skillState, skillData);
    }

    private void OnEnable()
    {
        _attribute = GetComponent<CharacterAttribute>();
        _onActivateSkill.Subcribe(HandleActivateSkill);

        SetUpSkills();
    }

    private void OnDisable()
    {
        _onActivateSkill.Unsubcribe(HandleActivateSkill);
    }

    
}
