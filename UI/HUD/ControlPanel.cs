using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private InputValueHolders _inputValueHolders;
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private IntegerListVariable _thisPickedSkill;

    [Header("Config")]
    [SerializeField]
    private Joystick _moveJoyStick;
    [SerializeField]
    private Button _aimButton;
    [SerializeField]
    private Button _cancelAimButton;
    [SerializeField]
    private Button _shootButton;
    [SerializeField]
    private List<SkillButton> _skillButtons;

    public void AttachInputHolderToUI()
    {
        int playerNo = _roomInfo.GetPlayerPos(_userId.Value);

        InputValueHolder inputHolder = _inputValueHolders.GetInputValueHolder(playerNo);
        if (inputHolder == null)
        {
            return;
        }

        _moveJoyStick.SetDirectionOutHolder(inputHolder.JoyStickDirection);
        _moveJoyStick.SetRawJoyStickInputHolder(inputHolder.JoyStickRaw);

        var aim = inputHolder.OnAim;
        _aimButton.onClick.AddListener(() =>
        {
            aim.Raise();
        });

        var cancelAim = inputHolder.OnCancelAim;
        _cancelAimButton.onClick.AddListener(() =>
        {
            cancelAim.Raise();
        });

        var shoot = inputHolder.OnShoot;
        _shootButton.onClick.AddListener(() =>
        {
            shoot.Raise();
        });
    }

    public void DetachInputHolderFromUI()
    {
        // skip dettact direction;

        _aimButton.onClick.RemoveAllListeners();
        _cancelAimButton.onClick.RemoveAllListeners();
        _shootButton.onClick.RemoveAllListeners();
        //_skill1Button.onClick.RemoveAllListeners();
        //_skill2Button.onClick.RemoveAllListeners();
    }

    public void SetUpSkillButtons()
    {
        var pick = _thisPickedSkill.List;
        for (int i = 0; i < _skillButtons.Count; i++)
        {
            if (i >= pick.Count)
            {
                _skillButtons[i].SetUpButton(0);
                continue;
            }
            _skillButtons[i].SetUpButton(pick[i]);
        }
    }

    private void OnDisable()
    {
        DetachInputHolderFromUI();
    }
}
