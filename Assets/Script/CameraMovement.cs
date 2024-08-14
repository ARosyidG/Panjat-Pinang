using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameManager gameManager;
    public Transform target = null;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 podium = new Vector3(0.0399999991f,-1.22000003f,-10.0f);
    // private Transform podiumTransform = new Transform();
    void Awake(){
        if (gameManager == null){
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }
    void OnEnable(){
        gameManager.OnGameOver+=goToPodium;
    }

    private void goToPodium()
    {
        // throw new NotImplementedException();
        StartCoroutine(goToPodiumCoroutine());
    }
    IEnumerator goToPodiumCoroutine(){
        while (Vector3.Distance(transform.position, podium) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, podium, ref velocity, smoothTime);
            yield return new WaitForFixedUpdate();
        }
        transform.position = podium;
    }

    void FixedUpdate()
    {
        if(!gameManager.isGameOver){
            if(gameManager.leadingPlayers != null){
                target = gameManager.leadingPlayers.transform;
            }
            if (target!=null){
                Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }
}