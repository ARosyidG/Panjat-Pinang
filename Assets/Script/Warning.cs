using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Warning : MonoBehaviour
{
    private string _message;
    public string Message
    {
        get { return _message; }
        set
        {
            _message = value;
            warningTextUI.text = _message;
        }
    }
    [SerializeField] private TextMeshProUGUI warningTextUI;
    void Awake(){
        warningTextUI = GetComponentInChildren<TextMeshProUGUI>();
    }
}
