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
    private Button _skill1Button;
    [SerializeField]
    private Button _skill2Button;

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

        //var skill1 = inputHolder.OnSkill1;
        //_skill1Button.onClick.AddListener(() =>
        //{
        //    skill1.Raise();
        //});

        //var skill2 = inputHolder.OnSkill2;
        //_skill2Button.onClick.AddListener(() =>
        //{
        //    skill2.Raise();
        //});
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

    private void OnDisable()
    {
        DetachInputHolderFromUI();
    }
}
