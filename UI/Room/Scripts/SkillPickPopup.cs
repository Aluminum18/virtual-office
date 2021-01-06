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

    [Header("Config")]
    [SerializeField]
    private TMP_Text _pointText;
    [SerializeField]
    private List<SkillSlot> _skillSlots;

    Dictionary<int, SkillItem> _skillItemsMap = new Dictionary<int, SkillItem>();

    protected override void DoOnEnable()
    {
        _dataList = _skillList.SkillList;
        SetPickedSkillSO();
    }

    protected override void DoOnDisable()
    {
        _pickedSkills.OnListChanged -= UpdateInfos;
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

            item.SetPickedSkills(_pickedSkills);
        }

        UpdateInfos();
    }

    private void SetPickedSkillSO()
    {
        _pickedSkills = _allPlayersPickedSkills[_roomInfo.GetPlayerPos(_thisUserId.Value)];
        _pickedSkills.OnListChanged += UpdateInfos;

        SetSkillSlotPickSkillsSO();
    }

    private void SetSkillSlotPickSkillsSO()
    {
        var pickedList = _pickedSkills.List;
        for (int i = 0; i < _skillSlots.Count; i++)
        {
            _skillSlots[i].SetPickedSkills(_pickedSkills);
        }
    }

    private void UpdateInfos()
    {
        UpdatePoint();

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

    private void UpdatePoint()
    {
        var list = _pickedSkills.List;
        _remainPoint.Value = 10;
        for (int i = 0; i < list.Count; i++)
        {
            _remainPoint.Value -= _skillList.GetSkillCost(list[i]);
        }

        _pointText.text = _remainPoint.Value.ToString();
    }
}
