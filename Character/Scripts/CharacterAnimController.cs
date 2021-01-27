using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimController : MonoBehaviour
{
    // Animation State key
    private const string TRANSFORM_STATUS = "TransformStatus";

    // Transform Status values
    private const int IDLE = 1;
    private const int RUNNING = 2;

    [Header("Reference - assigned in run time")]
    [SerializeField]
    private StringVariable _characterState;
    [SerializeField]
    private Vector3Variable _rawInputMovingJoystick;

    [Header("Reference")]
    [SerializeField]
    private StringVariable _userId;

    [Header("Events in (User input)")]
    [SerializeField]
    private GameEvent _onCancelAim;

    [Header("Config")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private CharacterAttribute _characterAtt;
    [SerializeField]
    private GameObject _basicBowModel;

    [SerializeField]
    private RigBuilder _rigBuilder;
    private List<RigLayer> _rigLayers;

    private List<RigLayer> RigLayers
    {
        get
        {
            if (_rigLayers == null || _rigLayers.Count == 0)
            {
                _rigLayers = _rigBuilder.layers;
            }
            return _rigLayers;
        }
    }

    public void SetInput(InputValueHolder inputHolder)
    {
        _characterState = inputHolder.CharacterState;
        _onCancelAim = inputHolder.OnCancelAim;

        SubcribeInput();
    }

    public void SubcribeInput()
    {
        if (_characterAtt == null)
        {
            Debug.LogError("Missing CharacterAttribute!", this);
            return;
        }

        _onCancelAim.Subcribe(PlayerIdleFunc);
    }

    public void UnsubcribeInput()
    {
        if (_characterAtt == null)
        {
            Debug.LogError("Missing CharacterAttribute!", this);
            return;
        }

        _onCancelAim.Unsubcribe(PlayerIdleFunc);
    }

    private bool IsReadyAttackParam
    {
        get
        {
            return _animator.GetBool("ReadyAttack");
        }
    }

    private bool IsMovingParam
    {
        get
        {
            return _animator.GetBool("Moving");
        }
    }

    private void OnDestroy()
    {
        UnsubcribeInput();
    }

    public void ActiveRigLayer(int layerIndex, bool active)
    {
        if (RigLayers == null || RigLayers.Count == 0 || RigLayers.Count < layerIndex)
        {
            Debug.LogError($"invalid rig layer or layer index [{layerIndex}]", this);
            return;
        }

        if (layerIndex < 0)
        {
            for (int i = 0; i < _rigLayers.Count; i++)
            {
                _rigLayers[i].active = false;
            }
            return;
        }

        _rigLayers[layerIndex].active = active;
    }

    public RigLayerCustomInfo GetRigLayerCustomInfo(int layerIndex)
    {
        if (RigLayers == null || RigLayers.Count == 0 || RigLayers.Count < layerIndex)
        {
            Debug.LogError($"invalid rig layer or layer index [{layerIndex}]", this);
            return null;
        }

        if (layerIndex < 0) // basic bow case
        {
            return null;
        }

        return _rigLayers[layerIndex].rig.gameObject.GetComponent<RigLayerCustomInfo>();
    }

    public void PlayIdle()
    {
        _animator.SetInteger(TRANSFORM_STATUS, IDLE);
        _animator.SetTrigger("Idle");
    }

    public void PlayRun()
    {
        CheckAndSetLayerFollowingState();
        _animator.SetInteger(TRANSFORM_STATUS, RUNNING);
        _animator.SetTrigger("Run");
    }

    private AnimatorStateInfo _stateBeforeHit;
    public void PlayGetHit()
    {
        _stateBeforeHit = _animator.GetCurrentAnimatorStateInfo(0);
        _animator.SetTrigger("GetHit");
    }

    public void PlayAfterHit()
    {
        if (!_stateBeforeHit.IsName("Run"))
        {
            return;
        }
        PlayRun();
    }

    public void SetStrafe()
    {
        _animator.SetFloat("MovingAimX", _rawInputMovingJoystick.Value.x);
        _animator.SetFloat("MovingAimY", _rawInputMovingJoystick.Value.y);
    }

    public void UpdateReadyAttackState()
    {
        bool readyAttack = _characterState.Value == CharacterState.STATE_READY_ATTACK;
        _animator.SetBool("ReadyAttack", readyAttack);
    }

    public void CheckAndSetLayerFollowingState()
    {
        if (_characterState.Value == CharacterState.STATE_READY_ATTACK)
        {
            SetLayerWeight(3, 1f);
            SetLayerWeight(4, 1f);
            return;
        }

        SetLayerWeight(3, 0f);
        SetLayerWeight(4, 0f);
    }

    public void ActiveBasicBowModel(bool active)
    {
        _basicBowModel.SetActive(active);
    }

    private void SetLayerWeight(int layerIndex, float weight)
    {
        _animator.SetLayerWeight(layerIndex, weight);
    }

    private void PlayerIdleFunc(params object[] args)
    {
        PlayIdle();
    }
}
