using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillAction : MonoBehaviour
{
    [SerializeField]
    private StringVariable _thisUserId;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onActivateSkill;

    private CharacterAttribute _attribute;

    private void ActivateSkill(SkillId skillId, SkillState skillState, object[] skillData)
    {
        switch (skillId)
        {
            case SkillId.ArrNade:
                {
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

        if (!(eventParam[1] is int skillId))
        {
            return;
        }
        
        if (!(eventParam[2] is int skillState))
        {
            return;
        }

        if (!(eventParam[3] is object[] skillData))
        {
            skillData = null;
        }

        ActivateSkill((SkillId)skillId, (SkillState)skillState, skillData);
    }

    private void OnEnable()
    {
        _attribute = GetComponent<CharacterAttribute>();
        _onActivateSkill.Subcribe(HandleActivateSkill);
    }

    private void OnDisable()
    {
        _onActivateSkill.Unsubcribe(HandleActivateSkill);
    }

    
}
