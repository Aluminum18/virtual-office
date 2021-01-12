using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "NFQ/Skill/Create Skill")]
[System.Serializable]
public class SkillSO : ScriptableObject
{
    [Header("Static content")]
    public int SkillId;
    public Sprite SkillIcon;
    public Sprite SkillIcon2;
    public Sprite SkillPic;

    [Header("Reference from server")]
    public SkillType SkillType;
    public SkillUsageType SkillState;
    public string SkillName;
    public string SkillDesc;
    public float Cooldown;
    public int Cost;
    public int Damage;
    public float Duration;
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
