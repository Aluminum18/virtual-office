using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataGetter : MonoBehaviour
{
    [SerializeField]
    private StringVariable _thisUserId;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private InputValueHolders _inputHolders;

    public object[] GetSkillData(SkillId skillId, SkillState state)
    {
        var inputHolder = _inputHolders.GetInputValueHolder(_roomInfo.GetPlayerPos(_thisUserId.Value));
        if (inputHolder == null)
        {
            Debug.Log($"Cannot find pos of id [{_thisUserId}]");
            return null;
        }

        object[] data = null;

        switch (skillId)
        {
            case SkillId.ArrNade:
                {
                    if (state == SkillState.Second)
                    {
                        break;
                    }

                    data = new object[]
                    {
                        inputHolder.AimSpot.Value
                    };
                    break;
                }
            case SkillId.Crossbow:
                {
                    break;
                }
            case SkillId.MulShot:
                {
                    data = new object[]
                    {
                        inputHolder.AimSpot.Value
                    };
                    break;
                }
            case SkillId.PowerShot:
                {
                    data = new object[]
                    {
                        inputHolder.AimSpot.Value
                    };
                    break;
                }
            case SkillId.ThirdEye:
                {
                    break;
                }
        }

        return data;
    }
}
