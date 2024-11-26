using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToMenu : MonoBehaviour
{
    private AuthUIHandler authUIHandler;
    [SerializeField] private Button button;
    void Awake(){
        authUIHandler = FindAnyObjectByType<AuthUIHandler>();
        button = GetComponent<Button>();
    }
    void Start(){
        button.onClick.AddListener(BackToMenuHandler);
    }
    private void BackToMenuHandler(){
        authUIHandler.BackToMenu();
        transform.parent.gameObject.SetActive(false);
    }
}
