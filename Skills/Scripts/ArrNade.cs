using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ArrNade : MonoBehaviour
{
    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onExplode;

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

    public void Explose()
    {
        _onExplode.Invoke();

        Stack<GameObject> arrows = new Stack<GameObject>();

        for (int i = 0; i < _arrowNumber; i++)
        {
            arrows.Push(_arrrowPool.Spawn());
        }

        _layerNumber = (int)(_maxAngle / _layersOffsetAngle);

        arrows.Pop().transform.rotation = Quaternion.Euler(90f, 0f, 0f);

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
                StartCoroutine(MoveMiniArrow(arrow, j * 0.1f));
            }
        }

        gameObject.SetActive(false);
    }

    private IEnumerator MoveMiniArrow(GameObject arrow, float delay)
    {
        yield return new WaitForSeconds(delay);
        arrow.GetComponent<ArrowMoving>().HeadForward(_arrowSpeed);
    }
}

[CustomEditor(typeof(ArrNade))]
public class ArrNadeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myTarget = (ArrNade)target;
        if (GUILayout.Button("Explode"))
        {
            if (Application.isPlaying)
            {
                myTarget.Explose();
            }
        }
    }
}

