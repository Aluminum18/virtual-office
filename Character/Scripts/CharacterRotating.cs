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

    public void SetJoystickInputDirection(Vector3Variable joystickDirection)
    {
        _joystick = joystickDirection;
    }

    public void CheckAndSubcribeInput()
    {
        _joystick.OnValueChange += UpdateDirection;
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

    private void OnDestroy()
    {
        _joystick.OnValueChange -= UpdateDirection;
    }

}
