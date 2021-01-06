using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private SkillListSO _skillList;

    [Header("Assigned at runtime")]
    [SerializeField]
    private int _skillId;
    [SerializeField]
    private IntegerListVariable _pickedSkills;

    [Header("Config")]
    [SerializeField]
    private Image _skillIcon;
    [SerializeField]
    private Sprite _emptySkillIcon;
    [SerializeField]
    private Button _ejectButton;

    public void SetPickedSkills(IntegerListVariable pickedSkills)
    {
        _pickedSkills = pickedSkills;
    }

    public void AssignSkill(int skillId)
    {
        _skillId = skillId;
    }

    public void EjectSkill()
    {
        _pickedSkills.Remove(_skillId);
    }

    public void SetStatus(bool active)
    {
        _ejectButton.interactable = active;

        if (!active)
        {
            _skillId = 0;
            _skillIcon.sprite = _emptySkillIcon;
            return;
        }

        _skillIcon.sprite = _skillList.GetSkillIcon(_skillId);
    }
}
