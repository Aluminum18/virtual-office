using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAction : MonoBehaviour
{
    [Header("Reference - assigned in runtime")]
    [SerializeField]
    private StringVariable _characterState;
    [SerializeField]
    private FloatVariable _characterHp;

    [Header("Events in (user input)")]
    [SerializeField]
    private GameEvent _onAim;
    [SerializeField]
    private GameEvent _onCancelAim;
    [SerializeField]
    private GameEvent _onShoot;

    [Header("Events out")]
    [SerializeField]
    private UnityEvent _onEnable;
    [SerializeField]
    private UnityEvent _onHealed;
    [SerializeField]
    private UnityEvent _onTookDamage;
    [SerializeField]
    private UnityEvent _onDefeated;
    [SerializeField]
    private UnityEvent _onStateChanged;

    [Header("Config")]
    [SerializeField]
    private GameObject _forceShot;

    [Header("Runtime Config")]
    [SerializeField]
    private BaseWeapon _weapon;


    #region Network awared actions
    public void SetInput(InputValueHolder inputHolder)
    {
        _characterState = inputHolder.CharacterState;
        _onShoot = inputHolder.OnShoot;
        _onAim = inputHolder.OnAim;
        _onCancelAim = inputHolder.OnCancelAim;

        SubcribeInput();
    }

    public void SetInMapInfo(PlayerInMapInfo info)
    {
        _characterHp = info.Hp;

        //temp
        _characterHp.Value = 100f;

        SubcribeInMapInfo();
    }

    public void SetWeapon(BaseWeapon weapon)
    {
        if (_weapon != null)
        {
            _weapon._onReloaded.RemoveListener(CheckStateAndContinuePrepareAttack);
        }

        _weapon = weapon;
        _weapon._onReloaded.AddListener(CheckStateAndContinuePrepareAttack);
    }

    public void ChangeToAttackState(params object[] args)
    {
        _characterState.Value = "attack";
        _onStateChanged.Invoke();
    }

    public void ChangeToIdleState(params object[] args)
    {
        _characterState.Value = "idle";
        _onStateChanged.Invoke();
    }

    public void PrepareProjectile(params object[] args)
    {
        _weapon.PrepareProjectile();
    }

    public void CancelAttack(params object[] args)
    {
        _weapon.CancelAttack();
    }

    public void CheckCancelAttackOnMove()
    {
        if (_characterState.Value == CharacterState.STATE_READY_ATTACK)
        {
            return;
        }
        CancelAttack();
    }

    public void Attack(object[] args)
    {
        _weapon.StartAttack();
    }

    private void CheckStateAndContinuePrepareAttack()
    {
        if (_characterState.Value.Equals(CharacterState.STATE_READY_ATTACK))
        {
            PrepareProjectile();
        }
    }
    #endregion
    public void ActiveDefeatedForceShot(Vector3 contactPoint, Quaternion contactRotation, Vector3 direction)
    {
        _forceShot.SetActive(true);
        _forceShot.transform.SetPositionAndRotation(contactPoint, contactRotation);
        MainThreadDispatcher.StartUpdateMicroCoroutine(IE_ActiveDefeatedForceShot(contactPoint, contactRotation, direction));
    }

    private IEnumerator IE_ActiveDefeatedForceShot(Vector3 contactPoint, Quaternion contactRotation, Vector3 direction)
    {
        float time = 1f;
        var rb = _forceShot.GetComponent<Rigidbody>();
        rb.velocity = _forceShot.transform.forward * 20f;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector3.zero;
    }

    private void TakeDamageOrHeal(float newHp)
    {
        if (_characterHp.Value <= 0f)
        {
            _onDefeated.Invoke();
            return;
        }

        float change = _characterHp.LastChange;
        if (change < 0f)
        {
            _onTookDamage.Invoke();
        }
        else if (change > 0f)
        {
            _onHealed.Invoke();
        }      
    }

    public void SubcribeInput()
    {
        _onAim?.Subcribe(ChangeToAttackState);
        _onAim?.Subcribe(PrepareProjectile);

        _onCancelAim?.Subcribe(ChangeToIdleState);
        _onCancelAim?.Subcribe(CancelAttack);

        _onShoot?.Subcribe(Attack);
    }

    private void UnsubcribeInput()
    {
        _onAim?.Unsubcribe(ChangeToAttackState);
        _onAim?.Unsubcribe(PrepareProjectile);

        _onCancelAim?.Unsubcribe(ChangeToIdleState);
        _onCancelAim?.Unsubcribe(CancelAttack);

        _onShoot?.Unsubcribe(Attack);
    }    

    public void SubcribeInMapInfo()
    {
        _characterHp.OnValueChange += TakeDamageOrHeal;
    }

    private void UnsubcribeInMapInfo()
    {
        _characterHp.OnValueChange -= TakeDamageOrHeal;
    }

    private void OnEnable()
    {
        _onEnable.Invoke();
        _forceShot.SetActive(false);

#if UNITY_EDITOR
        if (_weapon == null)
        {
            return;
        }
        _weapon._onReloaded.AddListener(CheckStateAndContinuePrepareAttack);
#endif
    }

    private void Start()
    {
        ChangeToIdleState();
    }

    private void OnDestroy()
    {
        UnsubcribeInput();
        UnsubcribeInMapInfo();
        
        if (_weapon == null)
        {
            return;
        }
        _weapon._onReloaded.RemoveListener(CheckStateAndContinuePrepareAttack);
    }
}
