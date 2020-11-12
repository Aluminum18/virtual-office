using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotating : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Vector3Variable _joystick;
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

    private void OnEnable()
    {
        _joystick.OnValueChange += UpdateDirection;
    }

    private void OnDisable()
    {
        _joystick.OnValueChange -= UpdateDirection;
    }

}
