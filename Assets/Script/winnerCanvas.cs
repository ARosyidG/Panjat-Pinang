using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class winnerCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _reward;
    public void setText(string name, string reward){
        _name.text = name;
        _reward.text = reward;
    }

}
