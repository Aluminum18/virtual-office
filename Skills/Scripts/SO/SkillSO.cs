using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "NFQ/Skill/Create Skill")]
[System.Serializable]
public class SkillSO : ScriptableObject
{
    [Header("Static content")]
    public SkillId SkillId;
    public Sprite SkillIcon;
    public Sprite SkillIcon2;
    public Sprite SkillPic;
    public GameObject SkillActivationObject;

    [Header("Reference from server")]
    public SkillType SkillType;
    public SkillUsageType SkillState;
    public string SkillName;
    public string SkillDesc;
    public float Cooldown;
    public int Cost;
    public int Damage;
    public float Duration;
    public float CastTime;
    public float CastTimeSecond;

    [Header("Update during runtime")]
    [SerializeField]
    private float _remainCooldown;

    public float RemainCooldown
    {
        get
        {
            return _remainCooldown;
        }
    }

    public delegate void OnCooldownResetDel();
    public event OnCooldownResetDel OnCooldownReset;

    public void ResetRemainCooldown(float value)
    {
        _remainCooldown = value;
        OnCooldownReset?.Invoke();
    }

    public void ChangeRemainCooldownValue(float value)
    {
        _remainCooldown = value;
    }
}

public enum SkillId
{
    PowerShot = 1,
    MulShot = 2,
    ArrNade = 3,
    ThirdEye = 4,
    Crossbow = 5
}

public enum SkillType
{
    Active = 0,
    Passive = 1
}

public enum SkillUsageType
{
    SingleState = 0,
    DoubleState = 1
}

public enum SkillState
{
    First = 1,
    Second = 2
}
