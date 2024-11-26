using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Logout : MonoBehaviour
{
    private Button button;
    [SerializeField] private RequestToAPI requestToAPI; 
    Action<bool> isLogoutSucces;
    void Awake(){
        button = GetComponent<Button>();
    }
    void Start (){
        button.onClick.AddListener(OnLogoutButtonClick); 
    }
    void OnEnable(){
        isLogoutSucces += toMainMenu; 
    }

    private void toMainMenu(bool obj)
    {
        LoginInformation.LoggedUser = null;
        isLogoutSucces -= toMainMenu;
        Debug.Log(LoginInformation.LoggedUser);
        SceneManager.LoadScene("Menu");
    }

    private void OnLogoutButtonClick()
    {
        requestToAPI.Logout(isLogoutSucces);
    }
}
