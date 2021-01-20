using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEventManager : MonoBehaviour, IOnEventCallback
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _thisUserId;
    [Header("Events in")]
    [SerializeField]
    private GameEvent _onRequestActivateSkill;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onSkillActivate;

    [SerializeField]
    private SkillDataGetter _skillDataGetter;
    [SerializeField]
    private PunEventSender _eventSender;

    private void HandleSkillRequest(object[] eventParam)
    {
        SkillId skillId = (SkillId)eventParam[0];
        SkillState skillState = (SkillState)eventParam[1];

        object[] data =
        {
            _thisUserId.Value,
            skillId,
            skillState,
            _skillDataGetter.GetSkillData(skillId, skillState)
        };

        _eventSender.SendEvent(PhotonEventCode.CHARACTER_ACTIVATE_SKILL, ReceiverGroup.All, data);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != PhotonEventCode.CHARACTER_ACTIVATE_SKILL)
        {
            return;
        }

        object[] eventData = (object[])photonEvent.CustomData;

        if (!(eventData[0] is string userId))
        {
            Debug.LogWarning("skill event data issue", this);
            return;
        }

        if (!(eventData[1] is int skillId))
        {
            Debug.LogWarning("skill event data issue", this);
            return;
        }

        if (!(eventData[2] is int skillState))
        {
            Debug.LogWarning("skill event data issue", this);
            return;
        }

        if (!(eventData[3] is object[] skillData))
        {
            Debug.LogWarning("skill event data issue", this);
            skillData = null;
        }

        _onSkillActivate?.Raise(userId, (SkillId)skillId, (SkillState)skillState, skillData);
    }

    private void OnEnable()
    {
        _onRequestActivateSkill.Subcribe(HandleSkillRequest);
        PhotonNetwork.AddCallbackTarget(this);

    }

    private void OnDisable()
    {
        _onRequestActivateSkill.Unsubcribe(HandleSkillRequest);
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    
}
