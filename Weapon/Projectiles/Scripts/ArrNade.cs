using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using Photon.Pun;

public class ArrNade : MonoBehaviour
{
    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onExplode;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onRequestActivateSkill;

    [Header("Config")]
    [SerializeField]
    private ObjectPool _arrrowPool;
    [SerializeField]
    private int _arrowSpeed;
    [SerializeField]
    private int _arrowNumber;
    [SerializeField]
    private float _maxAngle;
    [SerializeField]
    private float _layersOffsetAngle;

    [Header("Inspec")]
    [SerializeField]
    private int _layerNumber;

    public bool IsExploded { get; set; }

    private ProjectileContactBehavior _contactBehavior;

    public void RequestExplodeOnCollision()
    {
        if (!PhotonNetwork.IsMasterClient || IsExploded)
        {
            return;
        }
        _onRequestActivateSkill.Raise(SkillId.ArrNade, SkillState.Second);
    }

    public void Explose()
    {
        if (IsExploded)
        {
            return;
        }

        IsExploded = true;
        _onExplode.Invoke();

        Stack<GameObject> arrows = new Stack<GameObject>();

        for (int i = 0; i < _arrowNumber; i++)
        {
            arrows.Push(_arrrowPool.Spawn());
        }

        _layerNumber = (int)(_maxAngle / _layersOffsetAngle);

        var firstArrow = arrows.Pop();
        firstArrow.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        firstArrow.GetComponent<ArrowMoving>()?.MoveForward(_arrowSpeed);

        for (int i = 1; i < _layerNumber + 1; i++)
        {
            int arrowNum = 2 * i + 2;
            float xAngle = 90f + i * _layersOffsetAngle;
            float yOffset = 360f / arrowNum;
            for (int j = 1; j < arrowNum + 1; j++)
            {
                if (arrows.Count == 0)
                {
                    return;
                }

                var arrow = arrows.Pop();

                arrow.transform.rotation = Quaternion.Euler(xAngle, j * yOffset, 0f);

                Observable.Timer(System.TimeSpan.FromSeconds(0.1 * j)).Subscribe(_ =>
                {
                    arrow.GetComponent<ArrowMoving>().MoveForward(_arrowSpeed);
                });
            }
        }
    }

    private void Awake()
    {
        Observable.TimerFrame(1).Subscribe(_ =>
        {
            _contactBehavior = GetComponent<ProjectileContactBehavior>();

            _contactBehavior = GetComponent<ProjectileContactBehavior>();
            var splitArrowContactBhv = _arrrowPool.PooledObject.GetComponent<ProjectileContactBehavior>();
            splitArrowContactBhv.Damage = _contactBehavior.Damage;
            splitArrowContactBhv.Team = _contactBehavior.Team;
            splitArrowContactBhv.Owner = _contactBehavior.Owner;

            _contactBehavior.Damage = 0;
        });   
    }
}

