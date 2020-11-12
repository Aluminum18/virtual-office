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
    private Queue<GameObject> _usingOjects = new Queue<GameObject>();

    public GameObject Spawn()
    {
        GameObject go;
        if (_pool.Count == 0)
        {
            if (_limitInstances == -1 || _usingOjects.Count < _limitInstances)
            {
                go = Instantiate(_pooledObject);
            }
            else
            {
                go = _usingOjects.Dequeue();
            }
        }
        else
        {
            go = _pool.Pop();
        }

        go.transform.SetPositionAndRotation(_spawnPos.position, _spawnPos.rotation);
        _usingOjects.Enqueue(go);

        return go;
    }

    public void Return(GameObject go)
    {

    }
}
