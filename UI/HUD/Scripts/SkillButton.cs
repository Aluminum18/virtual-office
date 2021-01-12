using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private SkillListSO _skillList;

    [Header("Reference - assigned at runtime")]
    [SerializeField]
    private SkillSO _skillSO;

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

    private float _remainCooldown = 0f;
    private double _activateTime;

    public void SetUpButton(int skillId)
    {
        if (skillId <= 0 || _skillList.SkillList.Count < skillId)
        {
            SetNoSkill();
            return;
        }

        _skillSO = _skillList.GetSkill(skillId);

        _skillId = _skillSO.SkillId;

        _firstButton.image.sprite = _skillSO.SkillIcon;
        _seconButton.image.sprite = _skillSO.SkillIcon2;

        SetInitalStatus();
    }

    public void SetInitalStatus()
    {
        ActiveFirstButton(_skillSO.SkillType.Equals(SkillType.Active));
        ActiveSecondButton(false);

        _cooldownImage.enabled = false;
        _cooldownText.enabled = false;
    }

    public void ActiveFirstButton(bool active)
    {
        _firstButton.enabled = active;
        _firstButton.image.enabled = active;
    }

    public void ActiveSecondButton(bool active)
    {
        _seconButton.enabled = active;
        _seconButton.image.enabled = active;
    }

    public void StartCooldown()
    {
        _activateTime = TimeUtils.GetCurrentTimeInMiliSec();
        StopAllCoroutines();
        StartCoroutine(IE_StartCooldown());
    }

    public void EndCooldown()
    {
        _remainCooldown = 0;
        SetInitalStatus();
    }

    private IEnumerator IE_StartCooldown()
    {
        while (_remainCooldown > 0)
        {
            _remainCooldown -= Time.deltaTime;
            _cooldownText.text = _remainCooldown.ToString("#.#");
            _cooldownImage.fillAmount = _remainCooldown / _skillSO.Cooldown;

            yield return null;
        }
    }

    private void SetNoSkill()
    {
        ActiveFirstButton(false);
        ActiveSecondButton(false);

        _cooldownImage.enabled = false;
        _cooldownText.enabled = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            return;
        }

        CalculateCooldown();
    }

    private void CalculateCooldown()
    {
        double timePassed = TimeUtils.GetCurrentTimeInMiliSec() - _activateTime;
        _remainCooldown -= (float)timePassed;
    }
}
