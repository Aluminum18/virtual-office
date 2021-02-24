using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotating : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Vector3Variable _joystick;
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private StringVariable _characterState;

    [Header("Runtime Reference")]
    [SerializeField]
    private Vector3Variable _aimSpot;

    [Header("Events in - Runtime Reference")]
    [SerializeField]
    private GameEvent _onAim;

    [Header("Config")]
    [SerializeField]
    [Tooltip("Degree/s")]
    private float _rotateSpeed;
    [Tooltip("Maximum difference of result rotation and target rotation in degree")]
    [SerializeField]
    private float _offsetAngle;

    private bool _isRotating;
    private Vector3 _direction;

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

    public void SetAimSpot(Vector3Variable aimSpot)
    {
        _aimSpot = aimSpot;
    }

    public void SetOnAimEvent(GameEvent onAim)
    {
        _onAim = onAim;
    }

    public void SetJoystickInputDirection(Vector3Variable joystickDirection)
    {
        _joystick = joystickDirection;
    }

    public void SubcribeInput()
    {
        if (_joystick == null || _aimSpot == null)
        {
            return;
        }

        _joystick.OnValueChange += UpdateDirection;
        _aimSpot.OnValueChange += RotateToAimSpot;
        _onAim.Subcribe(ForceRotateToAimSpot);
    }

    public void UnsubcribeInput()
    {
        _joystick.OnValueChange -= UpdateDirection;
        _onAim.Unsubcribe(ForceRotateToAimSpot);
    }

    public void RotateToAimSpot(Vector3 aimSpot)
    {
        if (_characterState.Value.Equals(CharacterStandingState.WALKING))
        {
            return;
        }
        UpdateDirection(aimSpot - transform.position);
    }

    public void ForceRotateToAimSpot(object[] param)
    {
        Vector3 rotateTo = new Vector3(_aimSpot.Value.x - transform.position.x, 0f, _aimSpot.Value.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(rotateTo);
    }

    public void Rotate(Vector3 rotate)
    {
        transform.Rotate(rotate);
    }

    public void Rotate(float x, float y, float z)
    {
        transform.Rotate(x, y, z);
    }

    private void UpdateDirection(Vector3 direction)
    {
        if (direction.Equals(Vector3.zero) || _characterState.Value.Equals(CharacterStandingState.WALKING))
        {
            return;
        }

        Vector3 nomalize = Vector3.Normalize(direction);
        _direction.x = direction.x;
        _direction.z = direction.z;

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
            if (_characterState.Value == CharacterStandingState.WALKING)
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

    private void OnDestroy()
    {
        UnsubcribeInput();
    }

}
