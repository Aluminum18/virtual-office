using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.Events;

public class LoginMenu : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private FirebaseAuthenticator _authenticator;
    [SerializeField]
    private StringVariable _userId;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _afterLogin;
    [SerializeField]
    private UnityEvent _afterFailLogin;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onSignedIn;
    [SerializeField]
    private GameEvent _onFailedToSignIn;

    [Header("Config")]
    [SerializeField]
    private TMP_InputField _email;
    [SerializeField]
    private TMP_InputField _password;
    [SerializeField]
    private TextMeshProUGUI _errorText;

    public void Login()
    {
        _authenticator.SignIn(_email.text, _password.text);
    }

    public void GuestLogin()
    {
        _authenticator.GuestSignIn();
    }

    public void AutoLogin()
    {
        string email = PlayerPrefs.GetString("email", "");
        if (string.IsNullOrEmpty(email))
        {
            return;
        }

        _authenticator.SignInWithEmail(email);
    }

    private void OnLoginSuccess(params object[] args)
    {
        _afterLogin.Invoke();
    }

    private void OnLoginFail(params object[] args)
    {
        var exception = (System.AggregateException)args[0];
        _errorText.gameObject.SetActive(true);
        _errorText.text = exception.InnerExceptions[0].InnerException.Message;

        _afterFailLogin.Invoke();
    }

    private void OnEnable()
    {
        _errorText.gameObject.SetActive(false);
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        _onSignedIn.Subcribe(OnLoginSuccess);
        _onFailedToSignIn.Subcribe(OnLoginFail);
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        _onSignedIn.Unsubcribe(OnLoginSuccess);
        _onFailedToSignIn.Unsubcribe(OnLoginFail);
    }
}
