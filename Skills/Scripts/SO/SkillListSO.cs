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

    private Dictionary<int, SkillSO> _skillMap = new Dictionary<int, SkillSO>();
    private Dictionary<int, SkillSO> SkillMap
    {
        get
        {
            if (_skillMap.Count != _skillList.Count)
            {
                _skillMap.Clear();
                for (int i = 0; i < _skillList.Count; i++)
                {
                    var skill = _skillList[i];
                    _skillMap[(int)skill.SkillId] = skill;
                }
            }
            return _skillMap;
        }
    }

    public Sprite GetSkillIcon(int skillId)
    {
        SkillMap.TryGetValue(skillId, out var skill);

        if (skill == null)
        {
            Debug.LogWarning("skill list is empty or invalid skill id", this);
            return null;
        }

        return skill.SkillIcon;
    }

    public int GetSkillCost(int skillId)
    {
        SkillMap.TryGetValue(skillId, out var skill);

        if (skill == null)
        {
            Debug.LogWarning("skill list is empty or invalid skill id", this);
            return 0;
        }

        return skill.Cost;
    }

    public GameObject GetSkillActivationObject(SkillId skillId)
    {
        SkillMap.TryGetValue((int)skillId, out var skill);
        if (skill == null)
        {
            Debug.LogWarning("skill list is empty or invalid skill id", this);
        }
        return skill.SkillActivationObject;
    }

    public SkillType GetSkillType(SkillId skillId)
    {
        SkillMap.TryGetValue((int)skillId, out var skill);
        if (skill == null)
        {
            Debug.LogWarning("skill list is empty or invalid skill id", this);
        }
        return skill.SkillType;
    }

    public int GetSkillDamage(SkillId skillId)
    {
        SkillMap.TryGetValue((int)skillId, out var skill);
        if (skill == null)
        {
            Debug.LogWarning("skill list is empty or invalid skill id", this);
        }
        return skill.Damage;
    }

    public SkillSO GetSkill(int skillId)
    {
        SkillMap.TryGetValue(skillId, out var skill);

        if (skill == null)
        {
            Debug.LogWarning("skill list is empty or invalid skill id", this);
        }

        return skill;
    }

}
