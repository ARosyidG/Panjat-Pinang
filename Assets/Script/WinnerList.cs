using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerList : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject listPrefabs; 
    Canvas canvas;
    [SerializeField] private Transform WinnerListContainer;
    public Action<List<Score>> isRefreshedSuccsed;
    private RequestToAPI requestToAPI;
    [SerializeField] private Button RefreshButton;
    List<GameObject> listOfWInnerListItems;
    void Awake(){
        canvas = GetComponent<Canvas>();
        requestToAPI = FindAnyObjectByType<RequestToAPI>();
        listOfWInnerListItems = new List<GameObject>();
    }
    void Start(){
        closeButton.onClick.AddListener(OnCloseButton);
        RefreshButton.onClick.AddListener(Refresh);
    }

    private void OnCloseButton()
    {
        canvas.enabled = false;
    }
    void OnEnable(){
        isRefreshedSuccsed += OnRefreshedSuccess;
    }
    void OnDisable(){
        isRefreshedSuccsed -= OnRefreshedSuccess;
    }
    public void Refresh(){
        foreach (var item in listOfWInnerListItems)
        {
            Destroy(item);
        }
        listOfWInnerListItems.Clear();
        requestToAPI.GetScores(isRefreshedSuccsed);

    }
    private void OnRefreshedSuccess(List<Score> winners){
        foreach (var item in winners)
        {
            GameObject winnerListItem = Instantiate(listPrefabs,WinnerListContainer);
            WinnerListItemUI winnerListItemComponent = winnerListItem.GetComponent<WinnerListItemUI>();
            winnerListItemComponent.setWinnerData(item.winner, item.reward);
            listOfWInnerListItems.Add(winnerListItem);
        }
    }
}
