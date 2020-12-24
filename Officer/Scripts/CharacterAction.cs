using System.Collections;
using System.Collections.Generic;
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
    private UnityEvent _onPrepareProjectile;
    [SerializeField]
    private UnityEvent _onCancelAttack;
    [SerializeField]
    private UnityEvent _onAttackProjectileSpawn;
    [SerializeField]
    private UnityEvent _onStateChanged;
    [SerializeField]
    private UnityEvent _onTookDamage;
    [SerializeField]
    private UnityEvent _onHealed;
    [SerializeField]
    private UnityEvent _onDefeated;

    [Header("Config")]
    [SerializeField]
    private float _baseAttackTime;
    [SerializeField]
    private float _attackRate;
    [SerializeField]
    private float _attackFactor;
    [SerializeField]
    private GameObject _forceShot;

    [SerializeField]
    private float _timeToNextAttack = 0;


    private bool _cancelAttack = false;

    private bool _isAttacking = false;

    private bool _projectileReady = false;
    private bool _lateAttack = false;

    #region Action could be network awared
    public bool CancelAttack
    {
        get
        {
            return _cancelAttack;
        }
        set
        {
            if (_cancelAttack == value)
            {
                return;
            }

            _cancelAttack = value;

            if (value)
            {
                _projectileReady = false;
                _isAttacking = false;
                _lateAttack = false;
                _timeToNextAttack = 0;
                _onCancelAttack.Invoke();
            }
        }
    }

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

    public void PrepareProjectile(params object[] args)
    {
        if (_isAttacking || _projectileReady)
        {
            return;
        }

        if (_timeToNextAttack > 0)
        {
            return;
        }

        _cancelAttack = false;
        _projectileReady = false;
        CountingUntilProjectile();
        _onPrepareProjectile?.Invoke();
    }

    public void Shoot(params object[] args)
    {
        if (_projectileReady)
        {
            SpawnProjectile();
            return;
        }

        _lateAttack = true;
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
    #endregion
    public void CheckCancelAttackOnMove()
    {
        if (_characterState.Value == CharacterState.STATE_READY_ATTACK)
        {
            return;
        }
        CancelAttack = true;
    }

    public void ActiveDefeatedForceShot(Vector3 contactPoint, Quaternion contactRotation, Vector3 direction)
    {
        _forceShot.SetActive(true);
        _forceShot.transform.SetPositionAndRotation(contactPoint, contactRotation);

        StartCoroutine(IE_ActiveDefeatedForceShot(contactPoint, contactRotation, direction));
    }
    private IEnumerator IE_ActiveDefeatedForceShot(Vector3 contactPoint, Quaternion contactRotation, Vector3 direction)
    {
        _forceShot.transform.SetPositionAndRotation(contactPoint, contactRotation);
        float time = 0.3f;
        while (time > 0)
        {
            _forceShot.transform.Translate(direction * Time.deltaTime);
            yield return null;
            time -= Time.deltaTime;
        }
    }

    private void SpawnProjectile()
    {
        _projectileReady = false;
        _timeToNextAttack = _attackRate;
        _onAttackProjectileSpawn.Invoke();
        StartCoroutine(IE_Reload());
    }

    private void CountingUntilProjectile()
    {
        StartCoroutine(IE_AtkToProjectile());
    }

    private IEnumerator IE_AtkToProjectile()
    {
        _isAttacking = true;

        float timeToProjectile = _baseAttackTime * _attackFactor;
        
        while (timeToProjectile > 0)
        {
            if (_cancelAttack)
            {
                yield break;
            }

            timeToProjectile -= Time.deltaTime;
            yield return null;
        }

        _projectileReady = true;

        if (_lateAttack)
        {
            SpawnProjectile();
            _lateAttack = false;
        }
        _isAttacking = false;
    }

    private IEnumerator IE_Reload()
    {
        while (_timeToNextAttack > 0)
        {
            _timeToNextAttack -= Time.deltaTime;
            yield return null;
        }

        ContinuePrepareProjectile();
    }

    private void ContinuePrepareProjectile()
    {
        if (_characterState.Value == CharacterState.STATE_READY_ATTACK)
        {
            PrepareProjectile();
        }
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
        _onCancelAim?.Subcribe(CancelAttackFunc);

        _onShoot?.Subcribe(PrepareProjectile);
        _onShoot?.Subcribe(Shoot);
    }

    private void UnsubcribeInput()
    {
        _onAim?.Unsubcribe(ChangeToAttackState);
        _onAim?.Unsubcribe(PrepareProjectile);

        _onCancelAim?.Unsubcribe(ChangeToIdleState);
        _onCancelAim?.Unsubcribe(CancelAttackFunc);

        _onShoot?.Unsubcribe(PrepareProjectile);
        _onShoot?.Unsubcribe(Shoot);
    }    

    public void SubcribeInMapInfo()
    {
        _characterHp.OnValueChange += TakeDamageOrHeal;
    }

    private void UnsubcribeInMapInfo()
    {
        _characterHp.OnValueChange -= TakeDamageOrHeal;
    }

    /// <summary>
    /// Bridge for subcribe event
    /// </summary>
    private void CancelAttackFunc(params object[] args)
    {
        CancelAttack = true;
    }

    private void OnEnable()
    {
        _onEnable.Invoke();
        _forceShot.SetActive(false);
    }

    private void Start()
    {
        ChangeToIdleState();
    }

    private void OnDestroy()
    {
        UnsubcribeInput();
        UnsubcribeInMapInfo();
    }
}
