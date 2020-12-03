using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Text;
using System;

public class UserInfoFetcher : MonoSingleton<UserInfoFetcher>
{
    private StringBuilder _sb = new StringBuilder();

    public void GetUserInfo(string userId, Action<FireStoreUserInfo> successCallback, Action failCallback)
    {
        _sb.Clear();
        _sb.Append(FireStoreCollection.USER_ACCOUNT).Append("/").Append(userId);
        var userRef = FirebaseFirestore.DefaultInstance.Document(_sb.ToString());
        userRef.GetSnapshotAsync().ContinueWithOnMainThread(
            getUserInfoTask =>
            {
                if (getUserInfoTask.IsFaulted)
                {
                    Debug.LogError($"Fail to get user info with id [{userId}]");
                    failCallback?.Invoke();
                    return;
                }

                var userData = getUserInfoTask.Result;

                successCallback?.Invoke(userData.ConvertTo<FireStoreUserInfo>());
            });
    }
}
