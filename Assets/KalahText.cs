using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KalahText : MonoBehaviour
{
    // Start is called before the first frame update
    string[] text = {
        "Ngecit",
        "F*** U",
        "Curang",
        "Game *****"
    };
    void OnEnable(){
        int randomNumber = UnityEngine.Random.Range(0,text.Length);
        transform.Find("Text").GetComponent<TextMeshProUGUI>().text = text[randomNumber];
    }
}
