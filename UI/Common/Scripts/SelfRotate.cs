using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SelfRotate : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotateSpeed;

    private void Update()
    {
        transform.Rotate(_rotateSpeed * Time.deltaTime);
    }
}
