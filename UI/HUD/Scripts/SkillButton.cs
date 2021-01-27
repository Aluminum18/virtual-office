using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private SkillListSO _skillList;
    [SerializeField]
    private StringVariable _thisUserId;

    [Header("Reference - assigned at runtime")]
    [SerializeField]
    private SkillSO _skillSO;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onSkillActivate;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onRequestSkillActivate;

    [Header("Config")]
    [SerializeField]
    private Button _firstButton;
    [SerializeField]
    private Button _seconButton;
    [SerializeField]
    private Image _cooldownImage;
    [SerializeField]
    private TMP_Text _cooldownText;

    [Header("Inspec")]
    [SerializeField]
    private int _skillId;

    private IEnumerator _IECooldown;

    public void SetUpButton(int skillId)
    {
        if (skillId <= 0 || _skillList.SkillList.Count < skillId)
        {
            SetNoSkill();
            return;
        }

        _skillSO = _skillList.GetSkill(skillId);

        _skillId = (int)_skillSO.SkillId;

        _firstButton.image.sprite = _skillSO.SkillIcon;
        _seconButton.image.sprite = _skillSO.SkillIcon2;

        SetInitalStatus();

        _skillSO.OnCooldownReset += CountCoolDown;
    }

    public void SetInitalStatus()
    {
        ActiveFirstButton(true);
        ActiveSecondButton(false);

        ShowCooldownElements(false);
    }

    public void RequestActivateFirstState()
    {
        _onRequestSkillActivate?.Raise(_skillSO.SkillId, SkillState.First);
    }

    public void RequestActivateSecondState()
    {
        _onRequestSkillActivate?.Raise(_skillSO.SkillId, SkillState.Second);
    }

    public void ActivateFirstState()
    {
        if (_skillSO.SkillState.Equals(SkillUsageType.DoubleState))
        {
            ActiveFirstButton(false);
            ActiveSecondButton(true);
            ShowCooldownElements(false);
            return;
        }
        ShowCooldownElements(true);

    }

    public void ActivateSecondState()
    {
        if (_skillSO.SkillState.Equals(SkillUsageType.DoubleState))
        {
            ActiveFirstButton(true);
            ActiveSecondButton(false);
            ShowCooldownElements(true);
        }
    }

    public void ActiveFirstButton(bool active)
    {
        _firstButton.enabled = _skillSO.SkillType == SkillType.Active && active;
        _firstButton.image.enabled = active;
    }

    public void ActiveSecondButton(bool active)
    {
        _seconButton.enabled = active;
        _seconButton.image.enabled = active;
    }

    public void CountCoolDown()
    {
        MainThreadDispatcher.StartUpdateMicroCoroutine(IE_StartCooldown());
    }

    public void EndCooldown()
    {
        SetInitalStatus();
    }

    public void ShowCooldownElements(bool show)
    {
        _cooldownImage.enabled = show;
        _cooldownText.enabled = show;
    }

    private IEnumerator IE_StartCooldown()
    {
        while (_skillSO.RemainCooldown > 0f)
        {
            _cooldownText.text = _skillSO.RemainCooldown.ToString("0.0");
            _cooldownImage.fillAmount = _skillSO.RemainCooldown / _skillSO.Cooldown;

            yield return null;
        }

        SetInitalStatus();
    }

    private void SetNoSkill()
    {
        ActiveFirstButton(false);
        ActiveSecondButton(false);

        _cooldownImage.enabled = false;
        _cooldownText.enabled = false;
    }

    private void HandleSkillActivate(object[] data)
    {
        if (!(data[0] is string userId))
        {
            return;
        }
        if (userId != _thisUserId.Value)
        {
            return;
        }

        if (!(data[1] is SkillId skillId))
        {
            return;
        }
        if ((int)skillId != _skillId)
        {
            return;
        }

        if (!(data[2] is SkillState skillState))
        {
            return;
        }
        if (skillState.Equals(SkillState.First))
        {
            ActivateFirstState();
        }
        else
        {
            ActivateSecondState();
        }
    }

    private void OnEnable()
    {
        _onSkillActivate.Subcribe(HandleSkillActivate);
        _IECooldown = IE_StartCooldown();
    }

    private void OnDisable()
    {
        _onSkillActivate.Unsubcribe(HandleSkillActivate);
        _skillSO.OnCooldownReset -= CountCoolDown;
    }
}
