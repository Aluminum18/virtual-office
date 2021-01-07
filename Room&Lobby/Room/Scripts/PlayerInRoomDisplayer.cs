using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInRoomDisplayer : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private SkillListSO _skillList;
    [SerializeField]
    private List<IntegerListVariable> _allPlayersPickedSkills;

    [Header("Reference - assigned at runtimie")]
    [SerializeField]
    private IntegerListVariable _pickedSkills;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onRoomInfoSOChange;

    [Header("Config")]
    [SerializeField]
    private int _position;
    [SerializeField]
    private TMP_Text _playerName;
    [SerializeField]
    private Sprite _emptySkillIcon;
    [SerializeField]
    private List<Image> _skillIcons;

    public void Refresh(params object[] args)
    {
        UpdateSkillIcons();

        string playerId = _roomInfo.GetPlayerIdAtPos(_position);
        if (string.IsNullOrEmpty(playerId))
        {
            _playerName.text = "Open";
            return;
        }

        UserInfoFetcher.Instance.GetUserInfo(playerId,
            userInfo =>
            {
                _playerName.text = userInfo.displayName;
            },
            () =>
            {

            });

    }

    private void UpdateSkillIcons()
    {
        for (int i = 0; i < _skillIcons.Count; i++)
        {
            _skillIcons[i].enabled = false;
        }

        var list = _pickedSkills.List;
        for (int i = 0; i < list.Count; i++)
        {
            var icon = _skillList.GetSkillIcon(list[i]);
            if (icon == null)
            {
                continue;
            }
            _skillIcons[i].enabled = true;
            _skillIcons[i].sprite = icon;
        }
    }

    private void SetPickedSkillsSO()
    {
        _pickedSkills = _allPlayersPickedSkills[_position - 1];
        _pickedSkills.OnListChanged += UpdateSkillIcons;
    }

    private void OnEnable()
    {
        SetPickedSkillsSO();

        _onRoomInfoSOChange.Subcribe(Refresh);

        Refresh();
    }

    private void OnDisable()
    {
        _onRoomInfoSOChange.Unsubcribe(Refresh);
        _pickedSkills.OnListChanged -= UpdateSkillIcons;
    }
}
