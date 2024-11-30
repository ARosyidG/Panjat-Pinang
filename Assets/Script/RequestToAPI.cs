using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public class RequestToAPI : MonoBehaviour
{
    Action<List<Score>> test;
    void Update(){
        if(!LoginInformation.isLoggedAsGuest && Input.GetKeyDown(KeyCode.Space)){
            GetScores(test);
        }
    }
    public void Login(Dictionary<string, TMP_InputField> loginForm, Action<string> OnError, Action<bool> isLogginSuccess)
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
    }
    public void GetScores(Action<List<Score>> isSuccess)
    {
        if (LoginInformation.isLoggedAsGuest) return;
        Debug.Log("Getting Score...");
        StartCoroutine(GetScoresRequest(LoginInformation.LoggedUser, isSuccess));
    }
    public void RegisterUser(Dictionary<string, TMP_InputField> registerForm, Action<string> OnError, Action<bool> isLogginSuccess)
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
    IEnumerator RegisterUserRequest(RegisterUserRequest registerUserRequest, Action<string> OnError, Action<bool> isLogginSuccess)
    {
        string jsonData = JsonUtility.ToJson(registerUserRequest);
        string uri = "https://wrong-orsola-ganausi-032007d3.koyeb.app/17an/api/user";
        UnityWebRequest www = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.SetRequestHeader("Content-Type", "application/json");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        WebResponse<string> Data = JsonConvert.DeserializeObject<WebResponse<string>>(www.downloadHandler.text);
        Debug.Log(Data);
        if (Data.error != null || Data.error == "")   
        {
            isLogginSuccess.Invoke(false);
            OnError.Invoke(Data.error);
            Debug.LogError(www.downloadHandler.text);
            yield break;
        }
        StartCoroutine(LoginRequest(registerUserRequest.username, registerUserRequest.password, OnError, isLogginSuccess));
    }
    IEnumerator UpadeteLoggedUser(String token, Action<bool> isLogginSucces)
    {
        string uri = "https://wrong-orsola-ganausi-032007d3.koyeb.app/17an/api/user/current";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-API-TOKEN", token);
            yield return request.SendWebRequest();
            WebResponse<User> Data = JsonConvert.DeserializeObject<WebResponse<User>>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            if (Data.error != null){
                isLogginSucces.Invoke(false);
                yield break;
            } 
            LoginInformation.LoggedUser = Data.data;
            LoginInformation.LoggedUser.token = token;
            isLogginSucces.Invoke(true);
            Debug.Log(LoginInformation.LoggedUser.token);
        }
    }
    IEnumerator AddScoreRequest(User user, ScoreData scoreData)
    {
        string jsonData = JsonUtility.ToJson(scoreData);
        string uri = "https://wrong-orsola-ganausi-032007d3.koyeb.app/17an/api/score";
        UnityWebRequest www = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("X-API-TOKEN", user.token);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
    }
    IEnumerator GetScoresRequest(User user, Action<List<Score>> isSuccess)
    {
        Debug.Log("getting score...");
        string uri = "https://wrong-orsola-ganausi-032007d3.koyeb.app/17an/api/score";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-API-TOKEN", user.token);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            WebResponse<ScoreData> Data = JsonConvert.DeserializeObject<WebResponse<ScoreData>>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            if (Data.error != null){
                Debug.LogError(Data.error);
                yield break;
            }
            isSuccess.Invoke(Data.data.scores);
        }
    }
    IEnumerator LoginRequest(String username, String Password, Action<string> OnError, Action<bool> isLogginSuccess)
    {
        string uri = "https://wrong-orsola-ganausi-032007d3.koyeb.app/17an/api/auth";
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
            isLogginSuccess.Invoke(false);
            OnError.Invoke(Data.error);
            Debug.LogError(Data.error);
            yield break;
        }
        Debug.Log(www.downloadHandler.text);
        StartCoroutine(UpadeteLoggedUser(Data.data.token, isLogginSuccess));
    }
    IEnumerator LogoutRequest(string token, Action<bool> isSucces){
        string uri = "https://wrong-orsola-ganausi-032007d3.koyeb.app/17an/api/auth";
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
