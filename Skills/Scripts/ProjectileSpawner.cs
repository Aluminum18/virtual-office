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
            projectile.transform.rotation = Quaternion.LookRotation(target - projectile.transform.position);
            moving.HeadForward(_projectileSpeed);
        }
        else
        {
            moving.HeadTo(target, _projectileSpeed);
        }

        _onProjectileSpawn.Invoke();
        return projectile;
    }
}

public enum MovingPath
{
    Straight,
    Curve
}