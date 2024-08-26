using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
// using UnityEngine.Random;

public class Player : MonoBehaviour
{
    public float walkSpeed=10.0f;
    public float panjatSpeed=10.0f;
    private float boost;
    private bool isBoost=false;
    private bool isManjat = false;
    private string playerName;
    public string reward;
    public string PlayerName{
        get{return playerName;}
        set{
            playerName = value;
            setName();
        } 
    }
    private string activity;
    public GameObject tiang;
    Probability proTool;
    Dictionary<string, int> activities;
    public Coroutine activityCourotine;
    public Coroutine boostCourotine;
    // Collider2D collider;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private Animator animator;
    void Awake(){
        proTool = new Probability();
        activities = new Dictionary<string, int>();
        // collider = GetComponent<Collider2D>();
        activities.Add("panjat", 100);
        activities.Add("lempar", 20);
        // collider.
        if (gameManager == null){
            gameManager = FindAnyObjectByType<GameManager>();
        }
        rb = GetComponent<Rigidbody2D>();
        animator = transform.Find("Sprite").GetComponent<Animator>();
        boost = 1.0f;
        StartCoroutine(RandomizeSpeed());
    }
    void OnEnable(){
        gameManager.OnGameStart += Mulai;
        gameManager.OnGameOver += stop;
    }


    void OnDisable(){
        gameManager.OnGameStart -= Mulai;
        gameManager.OnGameOver -= stop;
    }

    private void stop()
    {
        rb.velocity = new Vector2(0,0);
        // rb.bodyType = RigidbodyType2D.Kinematic;
        StopAllCoroutines();
        if(tiang != null){
            Debug.Log("setted");
            reward = tiang.GetComponent<Tiang>().tiangRewardText;
        }
        // throw new NotImplementedException();s
    }

    private void Mulai()
    {
        // throw new NotImplementedException();
        rb.bodyType = RigidbodyType2D.Dynamic;
        boostCourotine = StartCoroutine(Boost());
    }
    IEnumerator Boost(){
        Dictionary<string, int> boostDic = new Dictionary<string, int>();
        boostDic.Add("boost", 5);
        boostDic.Add("not", 100);
        float leadingPlayerDistance = 100;
        while (true)
        {
            if(gameManager.leadingPlayers != null) {
                leadingPlayerDistance = Vector2.Distance(gameManager.leadingPlayers.transform.position, gameManager.leadingPlayers.GetComponent<Player>().tiang.GetComponent<Tiang>().tiangReward.transform.position);
            }
            if(gameObject != gameManager.leadingPlayers){
                boostDic["boost"] = 20;
            }else{
                boostDic["boost"] = 5;
            }
            if(leadingPlayerDistance < 10){
                if(gameObject != gameManager.leadingPlayers){
                    boostDic["boost"] = 100;
                }else{
                    boostDic["boost"] = 5;
                }
            }
            string result = proTool.getProbability(boostDic);
            if(result == "boost"){
                float boostValue = UnityEngine.Random.Range(90.0f, 150.0f);
                this.boost = boostValue;
                isBoost = true;
                Debug.Log("Boost");
                yield return new WaitForSecondsRealtime(2);
                isBoost = false;
                this.boost = 1.0f;
            }else{
                this.boost = 1.0f;
            }
            yield return new WaitForSecondsRealtime(5);
        }
    }
    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        
        if(LayerMask.LayerToName(collision.gameObject.layer) == "Ground"){
            if(activityCourotine!=null){
                StopCoroutine(activityCourotine);
            }
            StartCoroutine(OnGroundDesision());
        }
        if(collision.gameObject == tiang){
            if(tiang.GetComponent<Tiang>().winner == null){
                StopCoroutine(activityCourotine);
                activityCourotine = StartCoroutine(Panjat()); 
            }
        }
        // Debug.Log(collision.gameObject);
        if(tiang != null){
            if(collision.gameObject == tiang.GetComponent<Tiang>().tiangReward.transform.parent.gameObject){
                Debug.Log(activityCourotine);
                StopCoroutine(activityCourotine);
                if(tiang.GetComponent<Tiang>().winner == null){
                    activityCourotine = StartCoroutine(Menang());
                }
            }
        }
    }
    void Start(){
    }
    private void setName(){
        gameObject.name = playerName;
        transform.Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>().text = playerName;
    }
    IEnumerator OnGroundDesision(){
        animator.SetBool("isOnGround", true);
        if(gameManager.tiangsActive.Count != 0){
            int nomorTiang = UnityEngine.Random.Range(0,gameManager.tiangsActive.Count);
            tiang = gameManager.tiangsActive[nomorTiang];
            activity = "panjat";
            // Debug.Log(name + " " + activity);
            if(activity == "panjat") activityCourotine = StartCoroutine(WalkToTiang());
        }
        yield return new WaitForFixedUpdate();
    }
    IEnumerator WalkToTiang(){
        isManjat = false;
        animator.SetBool("isWalk", true);
        Vector3 destination = new Vector2(tiang.transform.position.x, transform.position.y);
        Vector3 direction = (destination-transform.position).normalized;
        walkSpeed = UnityEngine.Random.Range(75.0f, 225.0f);
        if(direction.x > 0){
            transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = true;
        }else{
            transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = false;
        }
        while (true)
        {
            direction = (destination-transform.position).normalized;
            rb.velocity = direction * (walkSpeed+boost) *Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator Lempar(){
        yield return new WaitForFixedUpdate();
    }
    IEnumerator Panjat(){
        isManjat = true;
        animator.SetBool("isPanjat", true);
        transform.position = new Vector2(tiang.transform.position.x,transform.position.y);
        Vector2 direction = Vector2.up;
        rb.bodyType = RigidbodyType2D.Kinematic;
        panjatSpeed = UnityEngine.Random.Range(40.0f, 180.0f);
        while (true)
        {
            rb.velocity = direction*(panjatSpeed+boost)*Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator Menang(){
        isManjat = false;
        tiang.GetComponent<Tiang>().SetWinner(gameObject);
        rb.velocity =new Vector2(0,0);
        animator.SetBool("isMenang", true);
        StopCoroutine(boostCourotine);
        // gameManager.players.
        var item = gameManager.players.First(kvp => kvp.Value == gameObject);
        gameManager.players.Remove(item.Key);
        transform.position = tiang.GetComponent<Tiang>().tiangReward.transform.position;
        gameManager.tiangsActive.Remove(this.tiang);
        foreach (GameObject _player in gameManager.players.Values.ToList<GameObject>())
        {
            Player _playerComponent = _player.GetComponent<Player>();
            if(_playerComponent.tiang == this.tiang){
                _playerComponent.jatuhKan();
            }
        }
        yield return null;
    }
    public void jatuhKan(){
        if(isManjat){
            // Debug.Log(gameObject.name + " Error");
            StopCoroutine(activityCourotine);
            activityCourotine = StartCoroutine(Jatuh());
        }
    }
    IEnumerator Jatuh(){
        // StopCoroutine(activityCourotine);
        animator.SetBool("isWalk", false);
        animator.SetBool("isJatuh", true);
        isManjat = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForFixedUpdate();
    }
    IEnumerator RandomizeSpeed(){
        while (true)
        {
            walkSpeed = UnityEngine.Random.Range(75.0f, 125.0f);
            panjatSpeed = UnityEngine.Random.Range(40.0f, 80.0f);
            float waitTime= UnityEngine.Random.Range(0.5f, 1.5f);
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }
    public void kalah(){
        animator.SetBool("isKalah", true);
    }
}
