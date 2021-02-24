using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class CharacterMoving : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Vector3Variable _joystickDirection;
    [SerializeField]
    private StringVariable _userId;

    [Header("Events out")]
    [SerializeField]
    private UnityEvent _onStartMove;
    [SerializeField]
    private UnityEvent _onChangeDirection;
    [SerializeField]
    private UnityEvent _onStop;

    [Header("Config")]
    [Tooltip("m/s")]
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _moveFactor;

    private bool _isMoving = false;

    private Vector3 _direction;
    private CharacterController _chaCon;

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

    public void SetMoveFactor(float factor)
    {
        _moveFactor = factor;
    }

    public void SetInputJoystickDirection(Vector3Variable direction)
    {
        _joystickDirection = direction;
        CheckAndSubcribeInput();
    }

    public void CheckAndSubcribeInput()
    {
        if (_joystickDirection == null)
        {
            return;
        }

        _joystickDirection.OnValueChange += UpdateDirection;
    }

    public void UnsubcribeInput()
    {
        if (_joystickDirection == null)
        {
            return;
        }

        _joystickDirection.OnValueChange -= UpdateDirection;
    }

    private void OnDestroy()
    {
        UnsubcribeInput();
    }

    private void Start()
    {
        _chaCon = GetComponent<CharacterController>();
    }

    private void UpdateDirection(Vector3 direction)
    {
        _direction.x = direction.x;
        _direction.z = direction.z;

        _onChangeDirection.Invoke();

        if (_direction.Equals(Vector3.zero))
        {
            _onStop.Invoke();
            return;
        }

        StartMove();
    }

    private void StartMove()
    {
        if (_isMoving)
        {
            return;
        }
        _onStartMove.Invoke();
        StartCoroutine(IE_Move());
    }  

    private IEnumerator IE_Move()
    {
        _isMoving = true;

        while (!_direction.Equals(Vector3.zero))
        {
            _chaCon.SimpleMove(_direction * _moveSpeed * _moveFactor);

            yield return null;
        }

        _isMoving = false;
    }
}
