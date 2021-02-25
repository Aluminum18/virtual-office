using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using Firebase.Firestore;

public class FirebaseAuthenticator : MonoBehaviour
{
    [SerializeField]
    private GameEvent _onFailedToCreateUser;
    [SerializeField]
    private GameEvent _onUserAccountCreated;
    [SerializeField]
    private GameEvent _onUserSignedIn;
    [SerializeField]
    private GameEvent _onFailedToSignIn;

    public void CreateNewUser(string email, string password)
    {
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(
            createUserTask =>
            {
                if (createUserTask.IsCanceled || createUserTask.IsFaulted)
                {
                    _onFailedToCreateUser.Raise(createUserTask.Exception);
                    return;
                }

                Debug.Log("New user created!");
                _onUserAccountCreated.Raise(createUserTask.Result);
            });
    }

    public void SignIn(string email, string password)
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(
            signInTask =>
            {
                if (signInTask.IsCanceled || signInTask.IsFaulted)
                {
                    _onFailedToSignIn.Raise(signInTask.Exception);
                    return;
                }

                _onUserSignedIn.Raise(signInTask.Result);
            });
    }

    public void GuestSignIn()
    {
        _onUserSignedIn.Raise();
    }

    public void SignInWithEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return;
        }

        Query query = FirebaseFirestore.DefaultInstance.Collection(FireStoreCollection.USER_ACCOUNT).WhereEqualTo("email", email);
        query.GetSnapshotAsync().ContinueWithOnMainThread(
            getUserTask =>
            {
                var userSnapshot = getUserTask.Result;
                if (userSnapshot.Count == 0)
                {
                    Debug.Log("Skip auto login, email is not registered");
                    return;
                }

                if (userSnapshot.Count > 1)
                {
                    Debug.LogError("Error! 1 email for different accounts");
                    return;
                }

                foreach (var user in userSnapshot)
                {
                    var userInfo = user.ConvertTo<FireStoreUserInfo>();
                    string key = TextUtils.xorString(userInfo.dateCreated.Ticks.ToString(), userInfo.userId);
                    string password = TextUtils.xorString(userInfo.password, key);
                    SignIn(email, password);
                }
            });
    }
}
