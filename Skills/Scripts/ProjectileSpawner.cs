using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    private ObjectPool _projectilePool;
    [SerializeField]
    protected float _projectileSpeed;
    [SerializeField]
    protected UnityEvent _onProjectileSpawn;

    public GameObject SpawnProjectile(Vector3 target, float speed, MovingPath path)
    {
        var projectile = _projectilePool.Spawn();
        var moving = projectile.GetComponent<ArrowMoving>();

        if (moving == null)
        {
            return projectile;
        }

        if (path.Equals(MovingPath.Straight))
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