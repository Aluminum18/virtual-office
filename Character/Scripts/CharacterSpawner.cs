using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSpawner : MonoBehaviour, IOnEventCallback
{
    [Header("Inspec")]
    [SerializeField]
    private List<GameObject> _managedCharacters = new List<GameObject>();

    [Header("Reference")]
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private InputValueHolders _inputHolders;
    [SerializeField]
    private PlayersInMapInfoSO _playersInMapInfo;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onAllPlayersReady;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onAllCharSpawned;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onAllCharactersSpawned;

    [Header("Config")]
    [SerializeField]
    private GameObject _archerPrefab;
    [SerializeField]
    private Transform _team1SpawnPos;
    [SerializeField]
    private Transform _team2SpawnPos;

    private Dictionary<string, GameObject> _playerMap = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> PlayerMap
    {
        get
        {
            if (_playerMap.Count != _managedCharacters.Count)
            {
                _playerMap.Clear();
                for (int i = 0; i < _managedCharacters.Count; i++)
                {
                    var playerAtt = _managedCharacters[i].GetComponent<CharacterAttribute>();
                    if (playerAtt == null)
                    {
                        continue;
                    }
                    _playerMap[playerAtt.AssignedUserId] = _managedCharacters[i];
                }
            }
            return _playerMap;
        }
    }

    public GameObject GetCharacterOnScene(string userId)
    {
        PlayerMap.TryGetValue(userId, out GameObject character);
        if (character == null)
        {
            Debug.LogWarning($"Cannot find character with id [{userId}]", this);           
        }
        return character;
    }

    private void OnEnable()
    {
        _onAllPlayersReady.Subcribe(SpawnThisPlayer);
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        _onAllPlayersReady.Unsubcribe(SpawnThisPlayer);
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void ManageCharacter(GameObject character)
    {
        _managedCharacters.Add(character);
        if (_managedCharacters.Count == _roomInfo.PlayerCount)
        {
            _onAllCharactersSpawned?.Raise();
            _onAllCharSpawned.Invoke();
        }
    }

    private void MapPlayerToInputHolder(GameObject character)
    {
        var characterAtt = character.GetComponent<CharacterAttribute>();
        if (characterAtt == null)
        {
            return;
        }

        int chaPos = _roomInfo.GetPlayerPos(characterAtt.AssignedUserId);
        if (chaPos == -1)
        {
            return;
        }

        InputValueHolder holder = _inputHolders.GetInputValueHolder(chaPos);
        PlayerInMapInfo playerInMapInfo = _playersInMapInfo.GetPlayerInfo(chaPos);
        if (holder == null ||
            playerInMapInfo == null)
        {
            return;
        }

        character.GetComponent<CharacterMoving>().SetInputJoystickDirection(holder.JoyStickDirection);

        var rotating = character.GetComponent<CharacterRotating>();
        rotating.SetJoystickInputDirection(holder.JoyStickDirection);
        rotating.SetCharacterState(holder.CharacterState);
        rotating.SetAimSpot(holder.AimSpot);
        rotating.SetOnAimEvent(holder.OnAim);
        rotating.SubcribeInput();

        var charAction = character.GetComponent<CharacterAction>();
        charAction.SetInput(holder);
        charAction.SetInMapInfo(playerInMapInfo);

        characterAtt.AnimController.SetInput(holder);

        character.GetComponent<CharacterSOSync>().RegisterInput();
    }

    private void SpawnThisPlayer(params object[] args)
    {
        int teamNo = _roomInfo.Team1.Contains(_userId.Value) ? 1 : 2;
        Vector3 teamPos = teamNo == 1 ? _team1SpawnPos.localPosition : _team2SpawnPos.localPosition;

        List<string> team = teamNo == 1 ? _roomInfo.Team1 : _roomInfo.Team2;

        var character = Instantiate(
            _archerPrefab,
            teamPos + Vector3.right * team.Count * 5f,
            Quaternion.identity);

        character.GetComponent<CharacterSkin>().SetMaterial(teamNo);

        character.GetComponent<CharacterAttribute>().AssignedUserId = _userId.Value;

        AddPhotonPropsToObject(character, teamNo);
        ManageCharacter(character);
        MapPlayerToInputHolder(character);

        character.GetComponent<CharacterAction>()?.SetupDefaultWeapon();
        character.GetComponent<CharacterSkillAction>()?.SetUpSkills();
    }

    private void SpawnCharacterResponseToEvent(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;
        Vector3 position = (Vector3)data[0];
        Quaternion rotation = (Quaternion)data[1];
        int viewId = (int)data[2];
        int team = (int)data[3];
        string userId = (string)data[4];

        var character = Instantiate(_archerPrefab, position, rotation);

        character.GetComponent<CharacterSkin>().SetMaterial(team);
        character.GetComponent<PhotonView>().ViewID = viewId;
        character.GetComponent<CharacterAttribute>().AssignedUserId = userId;

        ManageCharacter(character);
        MapPlayerToInputHolder(character);

        character.GetComponent<CharacterSkillAction>()?.SetUpSkills();
    }

    private void AddPhotonPropsToObject(GameObject go, int team)
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning($"Photon is not connected, object will not be network aware", go);
            return;
        }
        var photonView = go.GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogError($"Missing PhotonView [{go.name}]", go);
            return;
        }

        if (!PhotonNetwork.AllocateViewID(photonView))
        {
            Debug.LogError($"Fail to allocate Photon view Id [{go.name}]", go);
            return;
        }

        object[] data = new object[]
        {
            go.transform.position,
            go.transform.rotation,
            photonView.ViewID,
            team,
            _userId.Value
        };

        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent(PhotonEventCode.MANUAL_CHARACTER_SPAWNED, data, eventOptions, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != PhotonEventCode.MANUAL_CHARACTER_SPAWNED)
        {
            return;
        }

        SpawnCharacterResponseToEvent(photonEvent);
    }
}
