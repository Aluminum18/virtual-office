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

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onAllCharSpawned;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onAllCharactersSpawned;

    [Header("Config")]
    [SerializeField]
    private GameObject _archerBlue;
    [SerializeField]
    private GameObject _archerRed;
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
                    _playerMap[playerAtt.UserId] = _managedCharacters[i];
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
            Debug.LogError($"Cannot find character with id [{userId}]", this);           
        }
        return character;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        SpawnCharacters();
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

    private void SpawnCharacters()
    {
        List<string> team1 = _roomInfo.Team1;
        for (int i = 0; i < team1.Count; i++)
        {
            var character = Instantiate(_archerBlue, _team1SpawnPos.position + Vector3.right * i, Quaternion.identity);
            AddPhotonPropsToObject(character, 1);

            ManageCharacter(character);
        }

        List<string> team2 = _roomInfo.Team2;
        for (int i = 0; i < team1.Count; i++)
        {
            var character = Instantiate(_archerRed, _team2SpawnPos.position + Vector3.left * i, Quaternion.identity);
            AddPhotonPropsToObject(character, 2);

            ManageCharacter(character);
        }
    }

    private void SpawnCharacterResponseToEvent(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;
        Vector3 position = (Vector3)data[0];
        Quaternion rotation = (Quaternion)data[1];
        int viewId = (int)data[2];
        int team = (int)data[3];

        var character = Instantiate(team == 1 ? _archerBlue : _archerRed, position, rotation);
        character.GetComponent<PhotonView>().ViewID = viewId;

        ManageCharacter(character);
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
            team
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
