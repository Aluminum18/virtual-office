using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RegisterMenu : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private FirebaseAuthenticator _authenticator;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _closeAfterCreated;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onUserCreated;
    [SerializeField]
    private GameEvent _onFailedToCreateUser;
    
    [Header("Config")]
    [SerializeField]
    private TMP_InputField _email;
    [SerializeField]
    private TMP_InputField _password;
    [SerializeField]
    private TMP_InputField _passwordConfirm;
    [SerializeField]
    private TextMeshProUGUI _errorText;

    public void Register()
    {
        if (!_password.text.Equals(_passwordConfirm.text))
        {
            _errorText.text = "passwords does not match!";
            return;
        }

        _authenticator.CreateNewUser(_email.text, _password.text);
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        _onFailedToCreateUser.Subcribe(SetErrorText);
        _onUserCreated.Subcribe(CloseAfterCreatedUser);
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        _onFailedToCreateUser.Unsubcribe(SetErrorText);
        _onUserCreated.Unsubcribe(CloseAfterCreatedUser);
    }

    private void SetErrorText(params object[] agrs)
    {
        var exception = (System.AggregateException)agrs[0];
        _errorText.text = exception.InnerExceptions[0].InnerException.Message;
    }

    private void CloseAfterCreatedUser(params object[] agrs)
    {
        _closeAfterCreated?.Invoke();
    }
}
