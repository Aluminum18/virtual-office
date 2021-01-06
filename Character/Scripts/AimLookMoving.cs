using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AimLookMoving : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Vector3Variable _joystickDirection;

    [Header("Config")]
    [SerializeField]
    private float _moveSpeed;

    private bool _isMoving = false;

    private Vector3 _direction;

    private void Awake()
    {
        _joystickDirection.OnValueChange += UpdateDirection;
    }

    private void OnDisable()
    {
        _joystickDirection.OnValueChange -= UpdateDirection;
    }

    private void UpdateDirection(Vector3 direction)
    {
        Vector3 normalizedDir = Vector3.Normalize(direction);
        _direction.x = normalizedDir.x;
        _direction.y = normalizedDir.y;
        StartMove();
    }

    private void StartMove()
    {
        if (_isMoving)
        {
            return;
        }

        StartCoroutine(IE_Move());
    }

    private IEnumerator IE_Move()
    {
        _isMoving = true;

        while (!_direction.Equals(Vector3.zero))
        {
            transform.position += _direction * _moveSpeed * Time.deltaTime;
            Debug.Log("direction" + _direction);
            yield return null;
        }

        _isMoving = false;
    }
}
