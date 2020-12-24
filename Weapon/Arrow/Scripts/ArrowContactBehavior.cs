using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowContactBehavior : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onContactMapObject;
    [SerializeField]
    private UnityEvent _onContactPlayer;

    private Rigidbody _rb;

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

        characterAtt.InMapInfo.Hp.Value -= 20f;

        var characterAction = collider.GetComponent<CharacterAction>();
        if (characterAction == null)
        {
            return;
        }

        characterAction.ActiveDefeatedForceShot(transform.position - transform.forward, transform.rotation, _rb.velocity);

        _onContactPlayer.Invoke();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
