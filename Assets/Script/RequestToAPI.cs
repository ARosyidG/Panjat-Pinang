using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestToAPI : MonoBehaviour
{
    // private String token = "test";
    // private User loggedUser;
    void Awake(){
        User u = new User();
        LoginInformation.loggedUser = u;
    }
    void Update(){
        
        if(Input.GetKeyDown(KeyCode.U)){
            StartCoroutine(GetUser(LoginInformation.loggedUser.token));
        }
        if(Input.GetKeyDown(KeyCode.I)){
            // StartCoroutine(AddScore(LoginInformation.loggedUser));
        }
        if(Input.GetKeyDown(KeyCode.O)){
            StartCoroutine(GetScores(LoginInformation.loggedUser));
        }
    }
    IEnumerator GetUser(String token){
        string uri = "http://localhost:8099/17an/api/user/current";
        using(UnityWebRequest request = UnityWebRequest.Get(uri)){
            request.SetRequestHeader("X-API-TOKEN", token);
            yield return request.SendWebRequest();
            WebResponse<User> Data = JsonUtility.FromJson<WebResponse<User>>(request.downloadHandler.text);
            // Debug.Log(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            // loggedUser = Data.data;
            LoginInformation.loggedUser = Data.data;

        }
    }
    IEnumerator AddScore(User user, ScoreData scoreData){
        string jsonData = JsonUtility.ToJson(scoreData);
        string uri = "http://localhost:8099/17an/api/score";
        UnityWebRequest www = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("X-API-TOKEN", user.token);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
    }
    IEnumerator GetScores(User user){
        string uri = "http://localhost:8099/17an/api/score";
        using(UnityWebRequest request = UnityWebRequest.Get(uri)){
            request.SetRequestHeader("X-API-TOKEN", user.token);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            // WebResponse<User> Data = JsonUtility.FromJson<WebResponse<User>>(request.downloadHandler.text);
            // Debug.Log(request.downloadHandler.text);
            WebResponse<ScoreData> Data = JsonUtility.FromJson<WebResponse<ScoreData>>(request.downloadHandler.text);
            // Debug.Log(Data.data.scores.Count);
            LoginInformation.loggedUser.scores = Data.data.scores;
            foreach (Score item in LoginInformation.loggedUser.scores)
            {
                Debug.Log(item.reward);   
            }
            // loggedUser = Data.data;
        }
    }
    IEnumerator Login(String username, String Password){
        string uri = "http://localhost:8099/Test";
        using(UnityWebRequest request = UnityWebRequest.Get(uri)){
            // request.SetRequestHeader("X-API-TOKEN", "test");
            yield return request.SendWebRequest();
            // WebResponse<User> Data = JsonUtility.FromJson<WebResponse<User>>(request.downloadHandler.text);
            // Debug.Log(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            // loggedUser = Data.data;
        }
    }

}
