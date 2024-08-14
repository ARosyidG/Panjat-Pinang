using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MulaiButton : MonoBehaviour
{
    private GameManager gameManager;
    private Button startButton;
    [SerializeField] Canvas canvas;
    void Awake(){
        if (gameManager == null){
            gameManager = FindAnyObjectByType<GameManager>();
        }
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(Mulai);
    }

    private void Mulai()
    {
        gameManager.OnGameStart?.Invoke();
        canvas.enabled = false;   
    }
}
