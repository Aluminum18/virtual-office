using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Firebase.Unity.Editor;
using UnityEngine.Events;

public class RealTimeDBInitializer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onRealTimeDBInitialized;

    public void Init()
    {
#if UNITY_EDITOR
        //Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://fir-test-d1e63.firebaseio.com/");
#endif
        _onRealTimeDBInitialized?.Invoke();
    }
}
