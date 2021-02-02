using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionSync : MonoBehaviour
{
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private StringVariable _characterState;

    [Header("Config")]
    [SerializeField]
    private PhotonView _photonView;
    [SerializeField]
    private CharacterAction _characterAction;
    [SerializeField]
    private CharacterAttribute _characterAtt;

    public void CallPrepareProjectTile()
    {
        _photonView.RPC("PrepareProjectileRPC", RpcTarget.Others, _userId.Value);
    }
    [PunRPC]
    public void PrepareProjectileRPC(string userId)
    {
        if (userId != _characterAtt.AssignedUserId)
        {
            return;
        }
        //_characterAction.PrepareProjectile();
    }

    public void CallCancelAttack()
    {
        _photonView.RPC("CancelAttackRPC", RpcTarget.Others, _userId.Value);
    }
    [PunRPC]
    public void CancelAttackRPC(string userId)
    {
        if (userId != _characterAtt.AssignedUserId)
        {
            return;
        }
        //_characterAction.CancelAttack = true;
    }

    public void CallSpawnProjectTile()
    {
        _photonView.RPC("SpawnProjectTileRPC", RpcTarget.Others, _userId.Value);

    }
    [PunRPC]
    public void SpawnProjectTileRPC(string userId)
    {
        if (userId != _characterAtt.AssignedUserId)
        {
            return;
        }
        //_characterAction.Shoot();
    }

    public void CallChangeState()
    {
        _photonView.RPC("ChangeStateRPC", RpcTarget.Others, _userId.Value, _characterState.Value);

    }
    [PunRPC]
    public void ChangeStateRPC(string userId, string state)
    {
        if (userId != _characterAtt.AssignedUserId)
        {
            return;
        }

        if (state == "idle")
        {
            _characterAction.ChangeToRunning();
        }
        else
        {
            _characterAction.ChangeToWalking();
        }
    }
}
