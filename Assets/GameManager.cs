using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    Coroutine Testing;
    void Start()
    {
        Testing = StartCoroutine(Test());
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)){
            Debug.Log("stop");
            
            StopCoroutine(Testing);
            // StopCoroutine(Test());
        }
        if(Input.GetKeyDown(KeyCode.Y)){
            Debug.Log("stop");
            
            Testing = StartCoroutine(Test());
            // StopCoroutine(Test());
        }
    }
    IEnumerator Test(){
        for(int i = 0; true; i++){
            Debug.Log("test " + i);
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
