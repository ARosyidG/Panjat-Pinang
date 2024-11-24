using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    private GameObject _registerFormGameObject;
    private Dictionary<String,TMP_InputField> _registerForm;
    private TMP_InputField username;
    private TMP_InputField password;
    private TMP_InputField repassword;
    [SerializeField] private Button registerButton;
    [SerializeField] private RequestToAPI requestToAPI;
    [SerializeField] private Warning warning;
    private event Action<string> OnError;
    public Dictionary<String,TMP_InputField> GetRegisterForm()
    {
        return _registerForm;
    }
    public void SetRegisterForm(GameObject value)
    {
        _registerFormGameObject = value;
        username = _registerFormGameObject.transform.Find("Username").GetComponent<TMP_InputField>();
        password = _registerFormGameObject.transform.Find("Password").GetComponent<TMP_InputField>();
        repassword = _registerFormGameObject.transform.Find("RePassword").GetComponent<TMP_InputField>();
        _registerForm = new Dictionary<string, TMP_InputField>(){
            {"username",username},
            {"password",password},
            {"repassword",repassword}
        };

    }
    void Start()
    {
        SetRegisterForm(transform.Find("RegisterForm").gameObject);
        requestToAPI = FindAnyObjectByType<RequestToAPI>();
        registerButton.onClick.AddListener(RequestRegister);
    }
    void OnEnable(){
        OnError += warningHendler;
    }
    void OnDisable(){
        OnError -= warningHendler;
    }
    private void RequestRegister(){
        // verify input field
        foreach (TMP_InputField value in _registerForm.Values){
            if (value.text == "" || value.text == null){
                OnError.Invoke("! Fill The Form");
                return;
            }
        }
        if(_registerForm["password"].text != _registerForm["repassword"].text){
            OnError.Invoke("!!! RePassword Tidak Sesuai");
            return ;
        }
        requestToAPI.RegisterUser(_registerForm, OnError);
    }
    private void warningHendler(string message){
        warning.gameObject.SetActive(true);
        warning.Message = message;
    }
}
