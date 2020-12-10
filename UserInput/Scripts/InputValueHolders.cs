using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "inputValueHolders", menuName = "NFQ/Input/Create inputValueHolders")]
public class InputValueHolders : ScriptableObject
{
    [SerializeField]
    private List<InputValueHolder> _inputValueHolderList;
    private Dictionary<int, InputValueHolder> _inputValueHolderMap = new Dictionary<int, InputValueHolder>();
    private Dictionary<int, InputValueHolder> InputValueHolderMap
    {
        get
        {
            if (_inputValueHolderList.Count != _inputValueHolderMap.Count)
            {
                for (int i = 0; i < _inputValueHolderList.Count; i++)
                {
                    var inputPack = _inputValueHolderList[i];
                    _inputValueHolderMap[inputPack.PlayerNo] = inputPack;
                }
            }
            return _inputValueHolderMap;
        }
    }

    public InputValueHolder GetInputValueHolder(int playerNo)
    {
        InputValueHolderMap.TryGetValue(playerNo, out var inputHolder);
        if (inputHolder == null)
        {
            Debug.LogError($"Cannot get InputValueHolder for playerNo [{playerNo}]", this);
        }
        return inputHolder;
    }
}

[System.Serializable]
public class InputValueHolder
{
    public int PlayerNo;
    public Vector3Variable JoyStickDirection;
    public Vector3Variable JoyStickRaw;
    public Vector3Variable AimSpot;
    public StringVariable CharacterState;
    public GameEvent OnShoot;
    public GameEvent OnAim;
    public GameEvent OnCancelAim;
    public GameEvent OnSkill1;
    public GameEvent OnSkill2;
}
