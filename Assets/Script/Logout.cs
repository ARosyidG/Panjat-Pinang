using System;
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

    private void toMainMenu(bool isLogoutSuccess)
    {   
        if (!isLogoutSuccess){
            button.interactable=true;
            return;
        }

        LoginInformation.LoggedUser = null;
        isLogoutSucces -= toMainMenu;
        Debug.Log(LoginInformation.LoggedUser);
        SceneManager.LoadScene("Menu");
    }

    private void OnLogoutButtonClick()
    {
        requestToAPI.Logout(isLogoutSucces);
        button.interactable=false;
    }
}
