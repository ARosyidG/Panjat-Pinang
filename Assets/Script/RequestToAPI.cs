using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public class RequestToAPI : MonoBehaviour
{
    public void Login(Dictionary<string, TMP_InputField> loginForm, Action<string> OnError, Action isLogginSuccess)
    {
        StartCoroutine(LoginRequest(
            loginForm["username"].text,
            loginForm["password"].text,
            OnError,
            isLogginSuccess
        ));
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
    public void RegisterUser(Dictionary<string, TMP_InputField> registerForm, Action<string> OnError, Action isLogginSuccess)
    {
        RegisterUserRequest registerUserRequest = new RegisterUserRequest()
        {
            username = registerForm["username"].text,
            password = registerForm["password"].text
        };
        StartCoroutine(RegisterUserRequest(registerUserRequest, OnError, isLogginSuccess));
    }
    public void Logout(Action<bool> isSucces){
        if (LoginInformation.isLoggedAsGuest) return;
        StartCoroutine(LogoutRequest(LoginInformation.LoggedUser.token,isSucces));
    }
    IEnumerator RegisterUserRequest(RegisterUserRequest registerUserRequest, Action<string> OnError, Action isLogginSuccess)
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
        StartCoroutine(LoginRequest(registerUserRequest.username, registerUserRequest.password, OnError, isLogginSuccess));
    }
    IEnumerator UpadeteLoggedUser(String token, Action isLogginSucces)
    {
        string uri = "http://localhost:8099/17an/api/user/current";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-API-TOKEN", token);
            yield return request.SendWebRequest();
            WebResponse<User> Data = JsonConvert.DeserializeObject<WebResponse<User>>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            if (Data.error != null) yield break;
            LoginInformation.LoggedUser = Data.data;
            LoginInformation.LoggedUser.token = token;
            isLogginSucces.Invoke();
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
    IEnumerator LoginRequest(String username, String Password, Action<string> OnError, Action isLogginSuccess)
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
        StartCoroutine(UpadeteLoggedUser(Data.data.token, isLogginSuccess));
    }
    IEnumerator LogoutRequest(string token, Action<bool> isSucces){
        string uri = "http://localhost:8099/17an/api/auth";
        using (UnityWebRequest request = UnityWebRequest.Delete(uri))
        {
            request.SetRequestHeader("X-API-TOKEN", token);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            WebResponse<String> Data = JsonConvert.DeserializeObject<WebResponse<String>>(request.downloadHandler.text);
            Debug.Log(Data.data);
            if (Data.error != null){
                yield break;
            }
            isSucces.Invoke(true);
        }
    }
}
