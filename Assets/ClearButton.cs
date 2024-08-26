using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearButton : MonoBehaviour
{
    Button button;
    [SerializeField]TMP_InputField peserta;
    void Awake(){
        button = GetComponent<Button>();
        button.onClick.AddListener(Clear);
    }
    void Clear(){
        peserta.text = "";
    }
}
