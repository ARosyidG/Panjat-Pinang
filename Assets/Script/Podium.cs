using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Podium : MonoBehaviour
{
    [SerializeField]
    GameObject winnerPodium;
    [SerializeField] GameObject winnerPrefabs;
    public void setPodium(List<GameObject> winners){
        foreach (GameObject winner in winners)
        {
            // winnerPodium.SetActive(true);
            winnerCanvas _winner = Instantiate(winnerPrefabs,winnerPodium.transform).GetComponent<winnerCanvas>();
            _winner.setText(winner.GetComponent<Player>().PlayerName,winner.GetComponent<Player>().reward);
        }
    }
}
