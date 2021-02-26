using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class MulshotContainer : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private ObjectPool _pool;
    [SerializeField]
    private float _angle;
    [SerializeField]
    private float _delay;
    [SerializeField]
    private float _splitShotSpeed;
    [SerializeField]
    private UnityEvent _onSplitShotSpawn;

    private ProjectileContactBehavior _contactBehavior;

    public void ActiveMulshot()
    {
        for (int i = 0; i < 3; i++)
        {
            var splitArrow = _pool.Spawn();
            splitArrow.transform.Rotate(0f, (i - 1) * _angle, 0f);

            Observable.Timer(System.TimeSpan.FromSeconds(i * _delay)).Subscribe(_ =>
            {
                splitArrow.GetComponent<ArrowMoving>()?.MoveForward(_splitShotSpeed);
                _onSplitShotSpawn.Invoke();
            });
        }
    }

    private void Awake()
    {
        // delay 1 frame for Damage, Team and Owner data ready
        Observable.TimerFrame(1).Subscribe(_ =>
        {
            _contactBehavior = GetComponent<ProjectileContactBehavior>();
            var splitArrowContactBhv = _pool.PooledObject.GetComponent<ProjectileContactBehavior>();
            splitArrowContactBhv.Damage = _contactBehavior.Damage;
            splitArrowContactBhv.Team = _contactBehavior.Team;
            splitArrowContactBhv.Owner = _contactBehavior.Owner;

            _contactBehavior.Damage = 0;
        });
    }

    private void OnEnable()
    {
        // container rotation will aim to aimspot,
        //delay 2 frame to make sure split shots always rotate to aimspot at the beginning
        Observable.TimerFrame(2).Subscribe(_ =>
        {
            ActiveMulshot();
        });
        gameObject.SetActive(false);
    }
}
