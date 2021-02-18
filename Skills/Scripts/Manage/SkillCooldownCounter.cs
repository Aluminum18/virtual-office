using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkillCooldownCounter : MonoBehaviour
{
    [SerializeField]
    private StringVariable _thisUserId;
    [SerializeField]
    private SkillListSO _skillList;
    [SerializeField]
    private GameEvent _onSkillActivate;

    private double _lostFocusTime = 0;
    private float _timePassedFromLostFocus = 0;

    private void CountCooldown(object[] eventParam)
    {
        if (!(eventParam[0] is string userId))
        {
            return;
        }
        if (userId != _thisUserId.Value)
        {
            return;
        }

        if (!(eventParam[1] is SkillId skillId))
        {
            return;
        }
        var skillSO = _skillList.GetSkill((int)skillId);
        if (skillSO == null)
        {
            return;
        }

        if (!(eventParam[2] is SkillState skillState))
        {
            return;
        }

        if (skillState.Equals(SkillState.Second))
        {
            return;
        }

        MainThreadDispatcher.StartUpdateMicroCoroutine(IE_StartCoolDown(skillSO));
    }

    private IEnumerator IE_StartCoolDown(SkillSO skill)
    {
        skill.ResetRemainCooldown(skill.Cooldown);

        float remain = skill.RemainCooldown;
        while (remain > 0f)
        {
            if (_timePassedFromLostFocus > 0)
            {
                remain -= _timePassedFromLostFocus;
                yield return null;
                _timePassedFromLostFocus = 0f;
            }

            remain -= Time.deltaTime;

            skill.ChangeRemainCooldownValue(remain);
            yield return null;
        }
        skill.ChangeRemainCooldownValue(0f);
    }

    private void OnEnable()
    {
        _onSkillActivate.Subcribe(CountCooldown);
    }

    private void OnDisable()
    {
        _onSkillActivate.Unsubcribe(CountCooldown);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (_lostFocusTime == 0)
            {
                return;
            }
            _timePassedFromLostFocus = (float)(TimeUtils.GetCurrentTimeInCentiSec() - _lostFocusTime) * 0.01f;
            Debug.Log("Time pass " + _timePassedFromLostFocus);
        }
        else
        {
            _lostFocusTime = TimeUtils.GetCurrentTimeInCentiSec();
        }
    }
}
