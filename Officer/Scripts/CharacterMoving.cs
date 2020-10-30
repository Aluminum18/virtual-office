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

    [Header("Events out")]
    [SerializeField]
    private UnityEvent _onStartMove;
    [SerializeField]
    private UnityEvent _onStop;

    [Header("Config")]
    [Tooltip("m/s")]
    [SerializeField]
    private float _moveSpeed;

    private bool _isMoving = false;

    private Vector3 _direction;
    private CharacterController _chaCon;

    private void Awake()
    {
        _joystickDirection.OnValueChange += UpdateDirection;
    }

    private void OnDisable()
    {
        _joystickDirection.OnValueChange -= UpdateDirection;
    }

    private void Start()
    {
        _chaCon = GetComponent<CharacterController>();
    }

    private void UpdateDirection(Vector3 direction)
    {
        Vector3 normalizedDir = Vector3.Normalize(direction);
        _direction.x = normalizedDir.x;
        _direction.z = normalizedDir.y;
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
            Debug.Log("direction" + _direction);
            _chaCon.SimpleMove(_direction * _moveSpeed);
            yield return null;
        }

        _onStop.Invoke();
        _isMoving = false;
    }
}
