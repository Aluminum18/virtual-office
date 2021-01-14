using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInPool : MonoBehaviour
{
    private ObjectPool _pool;

    public void SetPool(ObjectPool pool)
    {
        _pool = pool;
    }

    private void OnDisable()
    {
        _pool.ReturnToPool(gameObject);
    }
}
