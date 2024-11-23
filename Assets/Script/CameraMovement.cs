using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public Transform target = null;
    public float smoothTime = 1.0f;
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
        yield return new WaitForSecondsRealtime(1);
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
                Vector3 targetPosition = new Vector3(
                    target.transform.position.x,
                    Mathf.Clamp(target.transform.position.y,-1.22000003f,Mathf.Infinity),
                    -10.0f
                );
                // Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }
}