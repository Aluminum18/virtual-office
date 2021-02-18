using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimController : MonoBehaviour
{
    // Animation State key
    private const string TRANSFORM_STATUS = "TransformStatus";

    private const string WALKING_STATE = "WalkingState";
    // Transform Status values
    private const int STAND = 1;
    private const int MOVE = 2;

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
            for (int i = 0; i < RigLayers.Count; i++)
            {
                RigLayers[i].active = false;
            }
            return;
        }

        RigLayers[layerIndex].active = active;
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

        return RigLayers[layerIndex].rig.gameObject.GetComponent<RigLayerCustomInfo>();
    }

    public void PlayIdle()
    {
        _animator.SetInteger(TRANSFORM_STATUS, STAND);
        _animator.SetTrigger("Idle");
    }

    public void PlayRun()
    {
        CheckAndSetLayerFollowingState();
        _animator.SetInteger(TRANSFORM_STATUS, MOVE);
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

    public void PlayCastArrNade()
    {
        _animator.SetTrigger("ArrNadeCast");
    }

    public void PlayDraw()
    {
        _animator.SetTrigger("Draw");
    }

    public void PlayAim()
    {
        _animator.SetTrigger("Aim");
    }

    public void PlayShoot()
    {
        _animator.SetTrigger("Shoot");
    }

    public void SetStrafe()
    {
        _animator.SetFloat("MovingAimX", _rawInputMovingJoystick.Value.x);
        _animator.SetFloat("MovingAimY", _rawInputMovingJoystick.Value.y);
    }

    public void UpdateReadyAttackState()
    {
        bool walking = _characterState.Value == CharacterStandingState.WALKING;
        _animator.SetBool(WALKING_STATE, walking);
    }

    public void CheckAndSetLayerFollowingState()
    {
        if (_characterState.Value == CharacterStandingState.WALKING)
        {
            SetLayerWeight(1, 1f);
            //SetLayerWeight(4, 1f);
            return;
        }

        SetLayerWeight(1, 0f);
        //SetLayerWeight(4, 0f);
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
