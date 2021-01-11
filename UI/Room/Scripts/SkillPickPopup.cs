using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillPickPopup : ScrollList<SkillSO>
{
    [Header("Reference")]
    [SerializeField]
    private IntegerVariable _remainPoint;
    [SerializeField]
    private SkillListSO _skillList;
    [SerializeField]
    private IntegerListVariable _pickedSkills;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onRoomInfoSOChanged;

    [Header("Config")]
    [SerializeField]
    private List<SkillSlot> _skillSlots;

    private Dictionary<int, SkillItem> _skillItemsMap = new Dictionary<int, SkillItem>();
    private Dictionary<int, SkillItem> SkillItemMap
    {
        get
        {
            if (_skillItemsMap.Count != _managedItems.Count)
            {
                _skillItemsMap.Clear();
                for (int i = 0; i < _managedItems.Count; i++)
                {
                    var item = _managedItems[i].GetComponent<SkillItem>();
                    _skillItemsMap[item.SkillId] = item;
                }
            }
            return _skillItemsMap;
        }
    }

    public void PickSkill(int skillId)
    {
        _pickedSkills.Add(skillId);

        RefreshView();
    }

    public void EjectSkill(int skillId)
    {
        _pickedSkills.Remove(skillId);

        RefreshView();
    }

    protected override void DoOnEnable()
    {
        _dataList = _skillList.SkillList;

        SetSkillSlotsParent();
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

        RefreshView();

        _onRoomInfoSOChanged.Subcribe(RefreshView);
    }

    protected override void DoOnDisable()
    {
        _onRoomInfoSOChanged.Unsubcribe(RefreshView);
    }

    private void SetSkillSlotsParent()
    {
        for (int i = 0; i < _skillSlots.Count; i++)
        {
            _skillSlots[i].SetParent(this);
        }
    }

    private void RefreshView(params object[] args)
    {
        CalculateRemainPoint();
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
