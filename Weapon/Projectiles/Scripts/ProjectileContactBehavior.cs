using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileContactBehavior : MonoBehaviour
{
    [Header("Runtime config")]
    [SerializeField]
    private string _owner;
    [SerializeField]
    private int _team;
    [SerializeField]
    private int _damage;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onContactMapObject;
    [SerializeField]
    private UnityEvent _onContactPlayer;

    private Rigidbody _rb;

    public string Owner
    {
        get
        {
            return _owner;
        }
        set
        {
            _owner = value;
        }
    }
    public int Team
    {
        get
        {
            return _team;
        }
        set
        {
            _team = Mathf.Clamp(value, 1, 2);
        }
    }
    public int Damage
    {
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        InteractWithCollider(other);
    }

    private void InteractWithCollider(Collider collider)
    {
        int colliderLayer = collider.gameObject.layer;

        switch (colliderLayer)
        {
            case 14: // MapBound
                {
                    HandleContactMapObject();
                    break;
                }
            case 15: // Terrain
                {
                    HandleContactMapObject();
                    break;
                }
            case 16: // Player
                {
                    HandleContactPlayer(collider);
                    break;
                }
        }
    }

    private void HandleContactMapObject()
    {
        _onContactMapObject.Invoke();
    }

    private void HandleContactPlayer(Collider collider)
    {
        var characterAtt = collider.GetComponent<CharacterAttribute>();

        if (characterAtt == null)
        {
            Debug.LogError($"Player does not contain Attribute conponent!", collider.gameObject);
            return;
        }

        characterAtt.InMapInfo.Hp.Value -= _damage;
        _onContactPlayer.Invoke();

        if (characterAtt.InMapInfo.Hp.Value > 0)
        {
            return;
        }

        var characterAction = collider.GetComponent<CharacterAction>();
        if (characterAction == null)
        {
            return;
        }

        characterAction.ActiveDefeatedForceShot(transform.position, transform.rotation, _rb.velocity);

    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
