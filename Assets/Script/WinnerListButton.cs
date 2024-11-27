using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WinnerListButton : MonoBehaviour
{
    [SerializeField] private Canvas WinnerListCanvas;
    private Button button;
    void Awake(){
        if(LoginInformation.isLoggedAsGuest){
            gameObject.SetActive(false);
        }
    }
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    void OnButtonClick(){
        WinnerListCanvas.enabled = true;
        WinnerListCanvas.GetComponent<WinnerList>().Refresh();
    }
}
