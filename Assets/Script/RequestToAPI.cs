using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public class RequestToAPI : MonoBehaviour
{
    void Awake()
    {
        User u = new User();
        LoginInformation.LoggedUser = u;

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
    //         StartCoroutine(GetScoresRequest(LoginInformation.loggedUser));
    //     }
    // }
    public void Login()
    {

    }
    public void AddScore(List<GameObject> winners)
    {
        if (LoginInformation.isLoggedAsGuest) return;
        ScoreData scores = new ScoreData();
        foreach (GameObject winner in winners)
        {
            scores.scores.Add(new Score()
            {
                winner = winner.GetComponent<Player>().PlayerName,
                reward = winner.GetComponent<Player>().reward,
            });
        }
        StartCoroutine(AddScoreRequest(LoginInformation.LoggedUser, scores));
        GetScores();
    }
    public void GetScores()
    {
        if (!LoginInformation.isLoggedAsGuest) return;
        StartCoroutine(GetScoresRequest(LoginInformation.LoggedUser));
    }
    public void RegisterUser(Dictionary<string, TMP_InputField> registerForm, Action<string> OnError)
    {
        RegisterUserRequest registerUserRequest = new RegisterUserRequest()
        {
            username = registerForm["username"].text,
            password = registerForm["password"].text
        };
        StartCoroutine(RegisterUserRequest(registerUserRequest, OnError));
    }
    IEnumerator RegisterUserRequest(RegisterUserRequest registerUserRequest, Action<string> OnError)
    {
        string jsonData = JsonUtility.ToJson(registerUserRequest);
        string uri = "http://localhost:8099/17an/api/user";
        UnityWebRequest www = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.SetRequestHeader("Content-Type", "application/json");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        WebResponse<string> Data = JsonConvert.DeserializeObject<WebResponse<string>>(www.downloadHandler.text);
        if (Data.error != null || Data.error == "")   
        {
            OnError.Invoke(Data.error);
            Debug.LogError(www.downloadHandler.text);
            yield break;
        }
        StartCoroutine(LoginRequest(registerUserRequest.username, registerUserRequest.password, OnError));
    }
    IEnumerator UpadeteLoggedUser(String token)
    {
        string uri = "http://localhost:8099/17an/api/user/current";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-API-TOKEN", token);
            yield return request.SendWebRequest();
            WebResponse<User> Data = JsonUtility.FromJson<WebResponse<User>>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            LoginInformation.LoggedUser = Data.data;
            LoginInformation.LoggedUser.token = token;
            Debug.Log(LoginInformation.LoggedUser.token);
        }
    }
    IEnumerator AddScoreRequest(User user, ScoreData scoreData)
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
    IEnumerator GetScoresRequest(User user)
    {
        Debug.Log("getting score...");
        string uri = "http://localhost:8099/17an/api/score";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-API-TOKEN", user.token);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            WebResponse<ScoreData> Data = JsonUtility.FromJson<WebResponse<ScoreData>>(request.downloadHandler.text);
            LoginInformation.LoggedUser.scores = Data.data.scores;
            foreach (Score item in LoginInformation.LoggedUser.scores)
            {
                Debug.Log($"winner: {item.winner}, reward: {item.reward}");
            }

        }
    }
    IEnumerator LoginRequest(String username, String Password, Action<string> OnError)
    {
        string uri = "http://localhost:8099/17an/api/auth";
        LoginUserRequest loginUserRequest = new LoginUserRequest()
        {
            username = username,
            password = Password
        };
        string jsonData = JsonUtility.ToJson(loginUserRequest);
        UnityWebRequest www = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.SetRequestHeader("Content-Type", "application/json");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
         WebResponse<LoginResponse> Data = JsonConvert.DeserializeObject<WebResponse<LoginResponse>>(www.downloadHandler.text);
        if (Data.error != null || Data.error == "")
        {
            OnError.Invoke(Data.error);
            Debug.LogError(Data.error);
            yield break;
        }
        Debug.Log(www.downloadHandler.text);
        StartCoroutine(UpadeteLoggedUser(Data.data.token));
    }

}
