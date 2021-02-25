using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Text;

public class RealTimeDBAccessor : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Editor only")]
    [TextArea(1, 10)]
    public string response;
    public string key;
#endif

    private DatabaseReference _dbRef;
    private DatabaseReference DbRootRef
    {
        get
        {
            if (_dbRef == null)
            {
                _dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            }
            return _dbRef;
        }
    }

    public DatabaseReference GetDataRef(string key)
    {
        return FirebaseDatabase.DefaultInstance.GetReference(key);
    }

    public void GetData(string key, Action<string> callback)
    {
        GetSnapShot(key,
            snapShot =>
            {
                callback.Invoke(snapShot.GetRawJsonValue());
            });
    }

    public void GetData(string key, Action<DataSnapshot> callback)
    {
        GetSnapShot(key,
            snapShot =>
            {
                callback.Invoke(snapShot);
            });
    }

    public void UpdateData(string key, object value)
    {
        double currentTime = TimeUtils.GetCurrentTimeInMiliSec();
        //DbRootRef.Child(key).SetRawJsonValueAsync(JsonUtility.ToJson(value)).ContinueWithOnMainThread(
        //    saveDataTask =>
        //    {
        //        Debug.Log($"Data with key [{key}] updated!");
        //        double responseTime = TimeUtils.GetCurrentTimeInMiliSec() - currentTime;
        //        Debug.Log($"Response in {responseTime}");
        //    });

        DbRootRef.Child(key).SetValueAsync(value).ContinueWithOnMainThread(
            saveDataTask =>
            {
                Debug.Log($"Data with key [{key}] updated!");
                double responseTime = TimeUtils.GetCurrentTimeInMiliSec() - currentTime;
                Debug.Log($"Response in {responseTime}");
            });
    }

    /// <summary>
    /// full child key (eg. use 'parent/child' instead of 'child')
    /// </summary>
    public void UpdateAChildWithCustomData(string childKey, object newNalue)
    {
        var childNewValue = DataConverter.ToDictionary(newNalue);
        UpdateAChild(childKey, childNewValue);
    }

    /// <summary>
    /// full child key (eg. use 'parent/child' instead of 'child')
    /// </summary>
    public void UpdateAChild(string childKey, object newNalue)
    {
        Dictionary<string, object> childUpdate = new Dictionary<string, object>
        {
            [childKey] = newNalue
        };

        double currentTime = TimeUtils.GetCurrentTimeInMiliSec();
        DbRootRef.UpdateChildrenAsync(childUpdate).ContinueWithOnMainThread(
            updateTask =>
            {
                if (updateTask.IsFaulted)
                {
                    Debug.LogError($"Failed to update child with key [{childKey}]");
                    return;
                }
                Debug.Log($"Child with key [{childKey}] updated!");

                double responseTime = TimeUtils.GetCurrentTimeInMiliSec() - currentTime;
                Debug.Log($"Response in {responseTime}");
            });
    }

    /// <summary>
    /// full child key (eg. use 'parent/child' instead of 'child')
    /// </summary>
    public void RemoveAChild(string childKey)
    {
        var childRef = DbRootRef.Child(childKey);
        childRef.RemoveValueAsync();
    }

    public void ListenChildChange(string parent, EventHandler<ChildChangedEventArgs> changedHandler)
    {
        var dataRef = DbRootRef.Child(parent);
        dataRef.ChildChanged += changedHandler;
    }

    public void RemoveChildChangeListener(string parent, EventHandler<ChildChangedEventArgs> changedHandler)
    {
        var dataRef = DbRootRef.Child(parent);
        dataRef.ChildChanged -= changedHandler;
    }

    private void GetSnapShot(string key, Action<DataSnapshot> callback)
    {
        FirebaseDatabase.DefaultInstance.GetReference(key).GetValueAsync().ContinueWithOnMainThread(
            getDataTask =>
            {
                if (getDataTask.IsFaulted)
                {
                    Debug.LogError($"Failed to get data with key [{key}]");
                    return;
                }

                var snapShot = getDataTask.Result;

                if (!snapShot.Exists)
                {
                    Debug.LogError($"Data with key [{key}] does not exist!");
                    return;
                }

                callback?.Invoke(snapShot);
            });
    }
}
