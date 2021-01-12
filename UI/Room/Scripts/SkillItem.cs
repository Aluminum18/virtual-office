using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour, IScrollListItem<SkillSO>
{
    [Header("Reference")]
    [SerializeField]
    private IntegerVariable _remainPoint;
    [SerializeField]
    private SkillListSO _skillList;

    [Header("Assigned at runtime")]
    [SerializeField]
    private int _skillId;

    [Header("Config")]
    [SerializeField]
    private Image _selectedFrame;
    [SerializeField]
    private Image _skillIcon;
    [SerializeField]
    private Image _skillPic;

    [SerializeField]
    private TMP_Text _skillName;
    [SerializeField]
    private TMP_Text _skillDesc;
    [SerializeField]
    private TMP_Text _costText;
    [SerializeField]
    private TMP_Text _damageText;
    [SerializeField]
    private TMP_Text _durationText;
    [SerializeField]
    private TMP_Text _cooldownText;

    [SerializeField]
    private Button _pickButton;

    private SkillPickPopup _parent;

    public int SkillId
    {
        get
        {
            return _skillId;
        }
    }

    private const string COST_PAT = "Cost: {0}";
    private const string DAMAGE_PAT = "Damage: {0}";
    private const string DURATION_PAT = "Duration: {0} second(s)";
    private const string COOLDOWN_PAT = "Cooldown: {0} second(s)";

    public void SetUp(SkillSO data)
    {
        _skillId = (int)data.SkillId;

        _skillIcon.sprite = data.SkillIcon;
        _skillPic.sprite = data.SkillPic;

        _skillName.text = data.SkillName;
        _skillDesc.text = data.SkillDesc;

        _costText.text = string.Format(COST_PAT, data.Cost);
        _damageText.text = string.Format(DAMAGE_PAT, data.Damage);
        _durationText.text = string.Format(DURATION_PAT, data.Duration);
        _cooldownText.text = string.Format(COOLDOWN_PAT, data.Cooldown);

        UpdateSkillItemStatus(false);
    }

    public void SetParent(SkillPickPopup parent)
    {
        _parent = parent;
    }

    public void PickSkill()
    {
        _parent.PickSkill(_skillId);
    }

    private Color32 _originCostTextColor = new Color32(147, 57, 0, 255);
    public void UpdateSkillItemStatus(bool isPicked)
    {
        SetSelectedFrameStatus(isPicked);

        int cost = _skillList.GetSkillCost(_skillId);
        bool unavailable = _remainPoint.Value < cost && !isPicked;
        if (unavailable)
        {
            SetPickButtonStatus(false);
            _costText.color = Color.red;
            return;
        }

        SetPickButtonStatus(!isPicked);
        _costText.color = _originCostTextColor;
    }

    private void SetPickButtonStatus(bool active)
    {
        _pickButton.interactable = active;
    }

    private void SetSelectedFrameStatus(bool active)
    {
        _selectedFrame.enabled = active;
    }
}
