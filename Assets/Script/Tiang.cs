using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tiang : MonoBehaviour
{
    public delegate void OnEnableHandler(GameObject tiang);
    public OnEnableHandler OnEnableEvent;
    public OnEnableHandler OnDisableEvent;
    [SerializeField] GameObject tiangPrefab;
    [SerializeField] GameObject tiangRewardPrefabs;
    public GameObject tiangReward;
    public string tiangRewardText;
    // Update is called once per frame
    public GameObject winner = null;
    GameManager gameManager;
    void Awake(){
        if (gameManager == null){
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }
    void Start(){
    }
    void Update()
    {
        
    }
    public void SetWinner(GameObject _winner){
        if (winner == null)
        {
            winner = _winner;
            gameManager.winner.Add(winner);
        }
    }
    void OnEnable(){
        OnEnableEvent?.Invoke(gameObject);
        gameManager.OnGameStart += mulai;
    }
    void OnDisable(){
        OnDisableEvent?.Invoke(gameObject);
        gameManager.OnGameStart -= mulai;
    }
    
    private void mulai()
    {
        // throw new NotImplementedException();
        tiangReward.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = tiangRewardText;
    }
    public void updateReward(string _tiangReward){
        tiangRewardText = _tiangReward;
    }

    public void updateTinggiTiang(int tinggiTiang){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
            // Debug.Log(child);
        }
        for (int i = 0; i < tinggiTiang; i++)
        {
            GameObject t = null;
            if(i == tinggiTiang-1){
                t = Instantiate(tiangRewardPrefabs,transform);
                tiangReward = t.transform.Find("Canvas").gameObject;
            }else{
                t = Instantiate(tiangPrefab,transform);
            }
            t.transform.localPosition = new Vector3(0,i,0); 
        }
    }
}
