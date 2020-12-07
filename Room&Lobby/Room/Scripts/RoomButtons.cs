using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButtons : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private RoomInfoAccessor _roomDbAccessor;

    [Header("Config")]
    [SerializeField]
    private Button _startButton;

    private void Start()
    {
        SetStartButtonStatus();
    }

    private void SetStartButtonStatus()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            _startButton.gameObject.SetActive(false);
            return;
        }

        _startButton.gameObject.SetActive(true);
    }

    public void ChangeToTeam1()
    {
        _roomDbAccessor.RequestSwitchToTeam(_userId.Value, 1);
    }

    public void ChangeToTeam2()
    {
        _roomDbAccessor.RequestSwitchToTeam(_userId.Value, 2);
    }
}
