using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthUIHandler : MonoBehaviour
{
    [SerializeField]private GameObject AuthUI;
    [SerializeField]private GameObject MenuUI;
    [SerializeField]private GameObject LoginUI;
    [SerializeField]private GameObject RegisterUI;
    [SerializeField]private Button LoginButton;
    [SerializeField]private Button RegisterButton;
    [SerializeField]private Button LoginAsGuestButton;

    void Awake(){
        MenuUI = AuthUI.transform.Find("Menu").gameObject;
        LoginUI = AuthUI.transform.Find("Login").gameObject;
        RegisterUI = AuthUI.transform.Find("Register").gameObject;
        LoginButton = MenuUI.transform.Find("Login").GetComponent<Button>();
        LoginAsGuestButton = MenuUI.transform.Find("Login As Guest").GetComponent<Button>();
        RegisterButton = MenuUI.transform.Find("Register").GetComponent<Button>();
    }
    void Start(){
        LoginButton.onClick.AddListener(OnLoginButtonClicked);
        RegisterButton.onClick.AddListener(OnRegisterButtonClicked);
        LoginAsGuestButton.onClick.AddListener(OnLoginAsGuestButtonClicked);
    }
    void OnEnable(){
        LoginUI.GetComponent<Login>().isLogginSuccess += toMainGame;
    }
    private void OnLoginAsGuestButtonClicked()
    {
        AuthUI.SetActive(false);
        LoginInformation.LoggedUser = null;
        toMainGame();
    }

    private void OnRegisterButtonClicked()
    {
        MenuUI.SetActive(false);
        RegisterUI.SetActive(true);
    }

    private void OnLoginButtonClicked()
    {
        MenuUI.SetActive(false);
        LoginUI.SetActive(true);
    }
    public void BackToMenu(){
        AuthUI.SetActive(true);
        MenuUI.SetActive(true);
        RegisterUI.SetActive(false);
        LoginUI.SetActive(false);
        LoginInformation.LoggedUser = null;
    }
    public void toMainGame()
    {
        LoginUI.GetComponent<Login>().isLogginSuccess -= toMainGame;
        SceneManager.LoadScene("MainGame");
    }
}
