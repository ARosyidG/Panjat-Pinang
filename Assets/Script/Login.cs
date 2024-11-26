using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private GameObject _loginFormGameObject;
    private Dictionary<String,TMP_InputField> _loginForm;
    private TMP_InputField username;
    private TMP_InputField password;
    [SerializeField] private Button loginButton;
    [SerializeField] private RequestToAPI requestToAPI;
    [SerializeField] private Warning warning;
    private event Action<string> OnError;
    public Action isLogginSuccess;
    public Dictionary<String,TMP_InputField> GetloginForm()
    {
        return _loginForm;
    }
    public void SetloginForm(GameObject value)
    {
        _loginFormGameObject = value;
        username = _loginFormGameObject.transform.Find("Username").GetComponent<TMP_InputField>();
        password = _loginFormGameObject.transform.Find("Password").GetComponent<TMP_InputField>();
        _loginForm = new Dictionary<string, TMP_InputField>(){
            {"username",username},
            {"password",password},
        };

    }
    void Start()
    {
        SetloginForm(transform.Find("LoginForm").gameObject);
        requestToAPI = FindAnyObjectByType<RequestToAPI>();
        loginButton.onClick.AddListener(Requestlogin);
    }
    void OnEnable(){
        OnError += warningHendler;
    }
    void OnDisable(){
        OnError -= warningHendler;
    }
    private void Requestlogin(){
        // verify input field
        foreach (TMP_InputField value in _loginForm.Values){
            if (value.text == "" || value.text == null){
                OnError.Invoke("! Fill The Form");
                return;
            }
        }
        requestToAPI.Login(_loginForm, OnError,isLogginSuccess);
    }
    private void warningHendler(string message){
        warning.gameObject.SetActive(true);
        warning.Message = message;
    }
}
