using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class MulshotContainer : MonoBehaviour
{
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

    private void OnEnable()
    {
        ActiveMulshot();
        gameObject.SetActive(false);
    }
}
