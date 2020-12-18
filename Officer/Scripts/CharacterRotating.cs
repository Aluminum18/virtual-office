using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotating : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Vector3Variable _joystick;
    [SerializeField]
    private Vector3Variable _aimSpot;
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private StringVariable _characterState;

    [Header("Config")]
    [SerializeField]
    [Tooltip("Degree/s")]
    private float _rotateSpeed;
    [SerializeField]
    private float _offsetAngle = 5;

    private bool _isRotating;
    private Vector3 _direction;

    private bool _isAiming = false;
    private Vector3 _aimingDirection = Vector3.zero;

    private bool IsThisPlayer
    {
        get
        {
            var att = GetComponent<CharacterAttribute>();
            if (att == null)
            {
                return true;
            }

            if (att.AssignedUserId == _userId.Value)
            {
                return true;
            }

            return false;
        }
    }

    public void SetCharacterState(StringVariable inputCharacterState)
    {
        _characterState = inputCharacterState;
    }

    public void SetJoystickInputDirection(Vector3Variable joystickDirection)
    {
        _joystick = joystickDirection;
        CheckAndSubcribeInput();
    }

    private void UpdateDirection(Vector3 direction)
    {
        if (direction.Equals(Vector3.zero))
        {
            return;
        }

        Vector3 nomalize = Vector3.Normalize(direction);
        _direction.x = nomalize.x;
        _direction.z = nomalize.z;

        StartRotate();
    }

    private void StartRotate()
    {
        if (_isRotating)
        {
            return;
        }

        StartCoroutine(IE_Rotate());
    }

    private IEnumerator IE_Rotate()
    {
        _isRotating = true;
        Quaternion rotateTo = Quaternion.LookRotation(_direction);
        while (Quaternion.Angle(transform.rotation, rotateTo) > _offsetAngle)
        {
            if (_characterState.Value == CharacterState.STATE_READY_ATTACK)
            {
                _isRotating = false;
                yield break;
            }

            if (!_direction.Equals(Vector3.zero))
            {
                rotateTo = Quaternion.LookRotation(_direction);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, _rotateSpeed * Time.deltaTime);
            yield return null;
        }

        _isRotating = false;
    }

    private void ToAimSpot(Vector3 aimSpot)
    {
        if (_characterState.Value == CharacterState.STATE_IDLE)
        {
            return;
        }

        if (_isAiming)
        {
            return;
        }
        //StartCoroutine(IE_ToAimSpot());
    }

    private IEnumerator IE_ToAimSpot()
    {
        _isAiming = true;

        // Rotate around Y only
        _aimingDirection.x = _aimSpot.Value.x - transform.position.x;
        _aimingDirection.z = _aimSpot.Value.z - transform.position.z;
        Quaternion rotateTo = Quaternion.LookRotation(_aimingDirection);

        while(Quaternion.Angle(transform.rotation, rotateTo) > _offsetAngle)
        {
            if (_characterState.Value == CharacterState.STATE_IDLE)
            {
                _isRotating = false;
                yield break;
            }

            _aimingDirection.x = _aimSpot.Value.x - transform.position.x;
            _aimingDirection.z = _aimSpot.Value.z - transform.position.z;
            rotateTo = Quaternion.LookRotation(_aimingDirection);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, _rotateSpeed * Time.deltaTime);
            yield return null;
        }

        _isAiming = false;
    }

    private void Update()
    {
        if (_characterState.Value == CharacterState.STATE_IDLE)
        {
            return;
        }

        // Rotate around Y only
        _aimingDirection.x = _aimSpot.Value.x - transform.position.x;
        _aimingDirection.z = _aimSpot.Value.z - transform.position.z;
        Quaternion rotateTo = Quaternion.LookRotation(_aimingDirection);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, _rotateSpeed * Time.deltaTime);

    }


    public void CheckAndSubcribeInput()
    {
        if (_joystick == null
            || _aimSpot == null
            )
        {
            return;
        }

        _joystick.OnValueChange += UpdateDirection;
        _aimSpot.OnValueChange += ToAimSpot;
    }

    private void OnDestroy()
    {
        _joystick.OnValueChange -= UpdateDirection;
        _aimSpot.OnValueChange -= ToAimSpot;
    }

}
