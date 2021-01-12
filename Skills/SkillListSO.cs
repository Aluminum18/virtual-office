using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillList", menuName = "NFQ/Skill/Create Skill List")]
public class SkillListSO : ScriptableObject
{
    [SerializeField]
    private List<SkillSO> _skillList;

    public List<SkillSO> SkillList
    {
        get
        {
            return _skillList;
        }
    }

    public void Init()
    {
        
    }

    public Sprite GetSkillIcon(int skillId)
    {
        if (skillId == 0)
        {
            return null;
        }

        if (_skillList == null || _skillList.Count == 0 || _skillList.Count < skillId)
        {
            Debug.LogWarning("skill list is empty or invalid skill id", this);
            return null;
        }

        return _skillList[skillId - 1].SkillIcon;
    }

    public int GetSkillCost(int skillId)
    {
        if (skillId == 0)
        {
            return 0;
        }

        if (_skillList == null || _skillList.Count == 0 || _skillList.Count < skillId)
        {
            Debug.LogWarning("skill list is empty or invalid skill id", this);
            return 0;
        }

        return _skillList[skillId - 1].Cost;
    }

    public SkillSO GetSkill(int skillId)
    {
        return _skillList[skillId - 1];
    }

}
