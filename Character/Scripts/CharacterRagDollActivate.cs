using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRagDollActivate : MonoBehaviour
{
    [SerializeField]
    private List<Collider> _colliders;
    [SerializeField]
    private List<Rigidbody> _rbs;
    [SerializeField]
    private CharacterController _chaCon;
    [SerializeField]
    private Animator _animator;

    public void ActiveRagDoll(bool active)
    {
        ActiveColliderAndRb(active);
        _chaCon.enabled = !active;
        _animator.enabled = !active;
    }

    private void ActiveColliderAndRb(bool active)
    {
        for (int i = 0; i < _colliders.Count; i++)
        {
            _colliders[i].enabled = active;
        }

        for (int i = 0; i < _rbs.Count; i++)
        {
            _rbs[i].detectCollisions = active;
            _rbs[i].isKinematic = !active;
        }
    }
}
