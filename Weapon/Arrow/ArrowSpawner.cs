using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField]
    private Vector3Variable _aimSpot;
    [SerializeField]
    private ObjectPool _arrowPool;
    [SerializeField]
    private float _arrowSpeed;

    public void FireArrow()
    {
        GameObject arrow = _arrowPool.Spawn();
        var arrowMoving = arrow.GetComponent<ArrowMoving>();
        arrowMoving.HeadTo(_aimSpot.Value);
        arrowMoving.MoveForward(_arrowSpeed);
    }
}
