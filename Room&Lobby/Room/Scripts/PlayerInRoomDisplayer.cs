using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInRoomDisplayer : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _thisUserId;
    [SerializeField]
    private StringVariable _thisUserName;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private SkillListSO _skillList;

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

        if (playerId == _thisUserId.Value)
        {
            _playerName.text = _thisUserName.Value;
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

        var list = GetPickedSkills(_position);
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

    private List<int> GetPickedSkills(int pos)
    {
        switch (pos)
        {
            case 1:
                return _roomInfo.Pos1Picks;
            case 2:
                return _roomInfo.Pos2Picks;
            case 3:
                return _roomInfo.Pos3Picks;
            case 4:
                return _roomInfo.Pos4Picks;
            case 5:
                return _roomInfo.Pos5Picks;
            case 6:
                return _roomInfo.Pos6Picks;
            default:
                {
                    Debug.LogWarning($"invalid pos [{pos}]", this);
                    return new List<int>();
                }
        }
    }

    private void OnEnable()
    {

        _onRoomInfoSOChange.Subcribe(Refresh);

        Refresh();
    }

    private void OnDisable()
    {
        _onRoomInfoSOChange.Unsubcribe(Refresh);
    }
}
