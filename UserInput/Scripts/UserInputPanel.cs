using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserInputPanel : MonoBehaviour, IPointerDownHandler
{
    [Header("Config")]
    [SerializeField]
    private Camera _mainCam;


    [Header("Events out")]
    [SerializeField]
    private GameEvent _onTouchedWorld;

    private Vector3 _touchScreenPoint = Vector3.zero;

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchScreenPoint.x = eventData.position.x;
        _touchScreenPoint.y = eventData.position.y;
        _touchScreenPoint.z = _mainCam.nearClipPlane;

        CheckTouchWorld();
    }

    private void CheckTouchWorld()
    {
        var ray = _mainCam.ScreenPointToRay(_touchScreenPoint);
        // 6 = Plane
        Physics.Raycast(ray, out var hit, 100f);

        if (hit.Equals(null))
        {
            return;
        }

        _onTouchedWorld.Raise(hit.point);
    }
}
