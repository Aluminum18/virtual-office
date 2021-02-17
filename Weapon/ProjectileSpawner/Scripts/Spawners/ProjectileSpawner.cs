using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private ObjectPool _projectilePool;
    [SerializeField]
    protected float _projectileSpeed;
    [SerializeField]
    protected float _dropAngleSpeed;
    [SerializeField]
    protected MovingPath _movingPath;

    [SerializeField]
    protected UnityEvent _onProjectileSpawn;

    public GameObject SpawnProjectile(Vector3 target)
    {
        var projectile = _projectilePool.Spawn();
        var moving = projectile.GetComponent<ArrowMoving>();

        if (moving == null)
        {
            return projectile;
        }

        if (_movingPath.Equals(MovingPath.Straight))
        {
            moving.MoveForward(target, _projectileSpeed);
        }
        else if (_movingPath.Equals(MovingPath.DropCurve))
        {
            moving.CurveDropMove(target, _projectileSpeed, _dropAngleSpeed);
        }
        else
        {
            moving.CurveMoveToTarget(target, _projectileSpeed);
        }

        _onProjectileSpawn.Invoke();
        return projectile;
    }
}

public enum MovingPath
{
    Straight,
    Curve,
    DropCurve
}