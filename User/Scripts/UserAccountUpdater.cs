using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.Events;

public class UserAccountUpdater : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private ThisPlayerAccountInfo _userAccount;
    [SerializeField]
    private StringVariable _thisUserId;
    [SerializeField]
    private StringVariable _thisUserDisplayName;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onUserCreated;
    [SerializeField]
    private GameEvent _onUserSignedIn;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onUserAccountInfoReadyInSO;

    [Header("Config")]
    [SerializeField]
    private TMP_InputField _password;
    [SerializeField]
    private TMP_InputField _fullName;
    [SerializeField]
    private TMP_InputField _displayName;

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

    private void UpdateAccountInfoToSO(params object[] args)
    {
        if (args.Length == 0)
        {
            CreateGuestInfo();
            return;
        }
        var userAuthInfo = (FirebaseUser)args[0];
        var userRef = FirebaseFirestore.DefaultInstance.Collection(FireStoreCollection.USER_ACCOUNT).Document(userAuthInfo.UserId);

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(
            getAccountInfoTask =>
            {
                if (getAccountInfoTask.IsFaulted)
                {
                    Debug.LogError(getAccountInfoTask.Exception.ToString());
                    return;
                }

                FireStoreUserInfo accountData = getAccountInfoTask.Result.ConvertTo<FireStoreUserInfo>();

                _userAccount.Id = accountData.userId;
                _userAccount.FullName = accountData.fullName;
                _userAccount.DisplayName = accountData.displayName;
                _userAccount.Email = accountData.email;

                _onUserAccountInfoReadyInSO.Invoke();
            });

    }

    private void CreateGuestInfo()
    {
        _userAccount.DisplayName = SystemInfo.deviceName;
        _userAccount.Id = SystemInfo.deviceUniqueIdentifier;
        _thisUserId.Value = _userAccount.Id;
        _onUserAccountInfoReadyInSO.Invoke();
    }

    private void OnEnable()
    {
        _onUserCreated.Subcribe(AddNewUserInfo);
        _onUserSignedIn.Subcribe(UpdateAccountInfoToSO);
    }

    private void OnDisable()
    {
        _onUserCreated.Unsubcribe(AddNewUserInfo);
        _onUserSignedIn.Unsubcribe(UpdateAccountInfoToSO);
    }
}
