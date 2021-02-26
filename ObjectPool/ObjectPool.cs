using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _pooledObject;
    [SerializeField]
    private Transform _spawnPos;
    [Tooltip("-1 means infinity")]
    [SerializeField]
    private int _limitInstances = -1;

    private Stack<GameObject> _pool = new Stack<GameObject>();
    private HashSet<GameObject> _poolChecker = new HashSet<GameObject>();

    private Queue<GameObject> _usingOjects = new Queue<GameObject>();

    public GameObject PooledObject
    {
        get
        {
            return _pooledObject;
        }
    }

    public GameObject Spawn()
    {
        GameObject go;
        if (_pool.Count == 0)
        {
            if (_limitInstances == -1 || _usingOjects.Count < _limitInstances)
            {
                go = Instantiate(_pooledObject);
                go.AddComponent<ObjectInPool>().SetPool(this);
            }
            else
            {
                go = _usingOjects.Dequeue();
            }
        }
        else
        {
            go = _pool.Pop();
            _poolChecker.Remove(go);
        }

        go.transform.SetPositionAndRotation(_spawnPos.position, _spawnPos.rotation);
        _usingOjects.Enqueue(go);

        go.SetActive(true);

        return go;
    }

    public void In_SpawnObject()
    {
        Spawn();
    }

    public void ReturnToPool(GameObject go)
    {
        go.SetActive(false);

        if (_poolChecker.Contains(go))
        {
            return;
        }

        _pool.Push(go);
        _poolChecker.Add(go);
    }
}
