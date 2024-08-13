using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestToAPI : MonoBehaviour
{
    private String token;
    void Update(){
        if(Input.GetKeyDown(KeyCode.U)){
            StartCoroutine(GetUser(token));
        }
    }
    IEnumerator GetUser(String token){
        string uri = "http://localhost:8099/17an/api/user/current";
        using(UnityWebRequest request = UnityWebRequest.Get(uri)){
            request.SetRequestHeader("X-API-TOKEN", "test");
            yield return request.SendWebRequest();
            WebResponse<User> Data = JsonUtility.FromJson<WebResponse<User>>(request.downloadHandler.text);
            // Debug.Log(request.downloadHandler.text);
            Debug.Log(Data.data.username);
        }
    }
}
