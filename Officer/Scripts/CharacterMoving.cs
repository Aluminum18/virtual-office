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

    private void OnEnable ()
    {
        CheckAndSubcribeInput();
    }

    private void CheckAndSubcribeInput()
    {
        if (_joystickDirection == null)
        {
            return;
        }

        if (!IsThisPlayer)
        {
            return;
        }

        _joystickDirection.OnValueChange += UpdateDirection;
    }

    private void OnDisable()
    {
        if (_joystickDirection == null)
        {
            return;
        }

        _joystickDirection.OnValueChange -= UpdateDirection;
    }

    private void Start()
    {
        _chaCon = GetComponent<CharacterController>();
    }

    private void UpdateDirection(Vector3 direction)
    {
        Normalize2Numbers(direction.x, direction.z, out float x, out float z);
        _direction.x = x;
        _direction.z = z;

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

    /// <summary>
    /// find x and y in unit circle so that x/y = a/b (x and y same sign as a and b)
    /// </summary>
    private void Normalize2Numbers(float a, float b, out float x, out float y)
    {
        if (a == 0 && b == 0)
        {
            x = 0; y = 0;
            return;
        }

        if (a == 0)
        {
            x = 0;
            y = 1 * Mathf.Sign(b);
            return;
        }

        float tan = b / a;
        if (tan == 0)
        {
            y = 0;
            x = 1f * Mathf.Sign(a);
            return;
        }

        float tanSqr = Mathf.Pow(tan, 2);

        float xSqr = 1 / (tanSqr + 1);
        float ySqr = tanSqr * xSqr;

        x = Mathf.Sqrt(xSqr) * Mathf.Sign(a);
        y = Mathf.Sqrt(ySqr) * Mathf.Sign(b);
    }
}
