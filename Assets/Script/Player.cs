using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string playerName;
    public string PlayerName{
        get{return playerName;}
        set{
            playerName = value;
            setName();
        } 
    }
    private string activity;
    private GameObject tiang;
    Probability proTool;
    Dictionary<string, int> activities;
    public Coroutine activityCourotine;
    // Collider2D collider;
    void Awake(){
        proTool = new Probability();
        activities = new Dictionary<string, int>();
        // collider = GetComponent<Collider2D>();
        activities.Add("panjat", 100);
        activities.Add("lempar", 20);
        // collider.
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(LayerMask.LayerToName(collision.gameObject.layer) == "Ground"){
            StartCoroutine(OnGroundDesision());
        }
        if(collision.gameObject == tiang){
            StopCoroutine(activityCourotine);
            activityCourotine = StartCoroutine(Panjat());
        }
    }
    void Start(){
    }
    void Update(){
    }
    private void setName(){
        gameObject.name = playerName;
        transform.Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>().text = playerName;
    }
    IEnumerator OnGroundDesision(){
        activity = proTool.getProbability(activities);
        // Debug.Log(activity);
        if(activity == "panjat") activityCourotine = StartCoroutine(WalkToTiang());
        yield return null;
    }
    IEnumerator WalkToTiang(){
        yield return null;
    }
    IEnumerator Lempar(){
        yield return null;
    }
    IEnumerator Panjat(){
        yield return null;
    }
}
