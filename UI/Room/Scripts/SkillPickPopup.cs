using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillPickPopup : ScrollList<SkillSO>
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _thisUserId;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private List<IntegerListVariable> _allPlayersPickedSkills;
    [SerializeField]
    private IntegerVariable _remainPoint;

    [SerializeField]
    private SkillListSO _skillList;

    [Header("Refererence - assigned at runtime")]
    [SerializeField]
    private IntegerListVariable _pickedSkills;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onRoomInfoSOChanged;

    [Header("Config")]
    [SerializeField]
    private List<SkillSlot> _skillSlots;

    Dictionary<int, SkillItem> _skillItemsMap = new Dictionary<int, SkillItem>();

    public void PickSkill(int skillId)
    {
        _pickedSkills.Add(skillId);

        CalculateRemainPoint();

        UpdateSkillItemsAndSlots();
    }

    public void EjectSkill(int skillId)
    {
        _pickedSkills.Remove(skillId);

        CalculateRemainPoint();

        UpdateSkillItemsAndSlots();
    }

    protected override void DoOnEnable()
    {
        _dataList = _skillList.SkillList;

        SetSkillSlotsParent();

        _onRoomInfoSOChanged.Subcribe(SetPickedSkillSO);

        SetPickedSkillSO();
    }

    protected override void DoOnDisable()
    {
        _onRoomInfoSOChanged.Unsubcribe(SetPickedSkillSO);
    }

    protected override void DoOnAllItemsCreated()
    {
        if (_skillItemsMap.Count == _managedItems.Count)
        {
            return;
        }

        for (int i = 0; i < _managedItems.Count; i++)
        {
            var item = _managedItems[i].GetComponent<SkillItem>();
            if (item == null)
            {
                Debug.LogError("There is an item missing SkillItem component");
                continue;
            }
            _skillItemsMap[item.SkillId] = item;
            item.SetParent(this);
        }
    }

    private int _lastPos = -1;
    private void SetPickedSkillSO(params object[] args)
    {
        int pos = _roomInfo.GetPlayerPos(_thisUserId.Value);
        if (_lastPos == pos)
        {
            return;
        }

        _lastPos = pos;

        var newPickedSkills = _allPlayersPickedSkills[pos - 1];
        if (_pickedSkills != null)
        {
            newPickedSkills.AssignNew(_pickedSkills.List);
            _pickedSkills.Clear();
        }

        _pickedSkills = newPickedSkills;

        CalculateRemainPoint();
        UpdateSkillItemsAndSlots();
    }

    private void SetSkillSlotsParent()
    {
        for (int i = 0; i < _skillSlots.Count; i++)
        {
            _skillSlots[i].SetParent(this);
        }
    }

    private void UpdateSkillItemsAndSlots()
    {
        UpdateSkillItems();

        UpdateSkillSlots();
    }

    private void UpdateSkillItems()
    {
        var pickedList = _pickedSkills.List;

        for (int i = 0; i < _managedItems.Count; i++)
        {
            _managedItems[i].GetComponent<SkillItem>().UpdateSkillItemStatus(false);
        }

        for (int i = 0; i < pickedList.Count; i++)
        {
            _skillItemsMap[pickedList[i]].UpdateSkillItemStatus(true);
        }
    }

    private void UpdateSkillSlots()
    {
        for (int i = 0; i < _skillSlots.Count; i++)
        {
            _skillSlots[i].SetStatus(false);
        }

        var pickedSkills = _pickedSkills.List;
        for (int i = 0; i < pickedSkills.Count; i++)
        {
            var skillSlot = _skillSlots[i];
            skillSlot.AssignSkill(pickedSkills[i]);
            skillSlot.SetStatus(true);
        }
    }

    private void CalculateRemainPoint()
    {
        var list = _pickedSkills.List;
        _remainPoint.Value = 10;
        for (int i = 0; i < list.Count; i++)
        {
            _remainPoint.Value -= _skillList.GetSkillCost(list[i]);
        }
    }
}
