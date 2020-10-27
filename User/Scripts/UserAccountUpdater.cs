using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;
using System;

public class UserAccountUpdater : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _password;
    [SerializeField]
    private TMP_InputField _fullName;
    [SerializeField]
    private TMP_InputField _displayName;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onUserCreated;

    StringBuilder _sb = new StringBuilder();

    private void AddNewUserInfo(params object[] args)
    {
        _sb.Clear();
        FirebaseUser user = (FirebaseUser)args[0];
        DateTime dateCreated = DateTime.Now;
        string key = TextUtils.xorString(dateCreated.Ticks.ToString(), user.UserId);
        string encodedPassword = TextUtils.xorString(_password.text, key);

        FireStoreUserInfo userData = new FireStoreUserInfo
        {
            userId = user.UserId,
            email = user.Email,
            fullName = _fullName.text,
            displayName = _displayName.text,
            password = encodedPassword,
            dateCreated = dateCreated
        };

        var userRef = FirebaseFirestore.DefaultInstance.Collection(FireStoreCollection.USER_ACCOUNT).Document(userData.userId);
        userRef.SetAsync(userData, SetOptions.MergeAll).ContinueWithOnMainThread(
            updateTask =>
            {
                if (updateTask.IsFaulted)
                {
                    Debug.LogError(updateTask.Exception.ToString());
                    return;
                }
                Debug.Log($"Updated user info with id [{userData.userId}]");
            });
    }

    private void OnEnable()
    {
        _onUserCreated.Subcribe(AddNewUserInfo);
    }

    private void OnDisable()
    {
        _onUserCreated.Unsubcribe(AddNewUserInfo);
    }
}
