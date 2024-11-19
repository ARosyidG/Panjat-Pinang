using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestToAPI : MonoBehaviour
{
    void Awake()
    {
        User u = new User();
        LoginInformation.loggedUser = u;

    }
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         // StartCoroutine(AddScore(LoginInformation.loggedUser, new ScoreData
    //         // {
    //         //     scores = new List<Score>(){
    //         //     new Score(){
    //         //         winner = "A",
    //         //         reward = "A"
    //         //     },
    //         //     new Score(){
    //         //         winner = "B",
    //         //         reward = "B"
    //         //     }
    //         // },
    //         // }));
    //         StartCoroutine(GetScores(LoginInformation.loggedUser));
    //     }
    // }
    public void Login()
    {

    }
    public void AddScore(List<GameObject> winners)
    {
            ScoreData scores = new ScoreData();
            foreach (GameObject winner in winners)
            {
                scores.scores.Add(new Score(){
                    winner = winner.GetComponent<Player>().PlayerName,
                    reward = winner.GetComponent<Player>().reward,
                });
            }
        StartCoroutine(AddScore(LoginInformation.loggedUser, scores));
    }
    public void GetScores()
    {

    }
    public void GetUser()
    {

    }
    public void RegisterUser()
    {

    }
    IEnumerator GetUser(String token)
    {
        string uri = "http://localhost:8099/17an/api/user/current";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-API-TOKEN", token);
            yield return request.SendWebRequest();
            WebResponse<User> Data = JsonUtility.FromJson<WebResponse<User>>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            LoginInformation.loggedUser = Data.data;
        }
    }
    IEnumerator AddScore(User user, ScoreData scoreData)
    {
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
    IEnumerator GetScores(User user)
    {
        Debug.Log("getting score...");
        string uri = "http://localhost:8099/17an/api/score";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-API-TOKEN", user.token);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            WebResponse<ScoreData> Data = JsonUtility.FromJson<WebResponse<ScoreData>>(request.downloadHandler.text);
            LoginInformation.loggedUser.scores = Data.data.scores;
            foreach (Score item in LoginInformation.loggedUser.scores)
            {
                Debug.Log($"winner: {item.winner}, reward: {item.reward}");
            }

        }
    }
    IEnumerator Login(String username, String Password)
    {
        string uri = "http://localhost:8099/Test";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            Debug.Log(request.downloadHandler.text);
        }
    }

}
