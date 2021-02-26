using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Runtime Config")]
    [SerializeField]
    private string _owner;
    [SerializeField]
    private int _team;
    [SerializeField]
    private int _projectileDamage;

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
    public int ProjectileDamage
    {
        get
        {
            return _projectileDamage;
        }
        set
        {
            _projectileDamage = value;
        }
    }

    public GameObject SpawnProjectile(Vector3 target)
    {
        var projectile = _projectilePool.Spawn();
        var contactBehavior = projectile.GetComponent<ProjectileContactBehavior>();
        contactBehavior.Owner = _owner;
        contactBehavior.Team = _team;
        contactBehavior.Damage = _projectileDamage;

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