using TMPro;
using UnityEngine;

public class WinnerListItemUI : MonoBehaviour
{
    public TextMeshProUGUI winnerUI;
    public TextMeshProUGUI rewardUI;
    public void setWinnerData(string winner, string reward){
        winnerUI.text = winner;
        rewardUI.text = reward;
    }
}
