using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.Events;

public class FirebaseInitializer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onFirebaseInitialized;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(
            initTask =>
            {
                if (initTask.Result == DependencyStatus.Available)
                {
                    Debug.Log("Firebase init successfully!");

                    InitRealTimeDB();

                    Invoke("RaiseEvent", 0.5f);
                    return;
                }

                Debug.LogError("Fail to init Firebase");
            });
    }

    private void InitRealTimeDB()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://fir-test-d1e63.firebaseio.com/");
    }

    private void RaiseEvent()
    {
        _onFirebaseInitialized?.Invoke();
    }

    /// <summary>
    /// https://github.com/firebase/quickstart-unity/issues/845
    /// </summary>
    private void OnApplicationQuit()
    {
        FirebaseFirestore.DefaultInstance.App.Dispose();
    }
}
