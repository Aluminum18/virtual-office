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

        _onContactPlayer.Invoke();
    }
}
