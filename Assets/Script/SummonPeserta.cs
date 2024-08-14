using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SummonPeserta : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private GameObject playerPrefab;
    // Start is called before the first frame update
    [SerializeField] private GameManager gameManager;
    // int lineCount = 0;
    void Start()
    {
        input.onValueChanged.AddListener(test);
        
    }
    private void test(string arg0)
    {
        List<string> names = input.text.Split('\n').ToList();
        names.Remove("");
        foreach (string name in names)
        {
            if(!gameManager.players.ContainsKey(name) && name != "")summon(name);
        }
        if(names.Count != gameManager.players.Count)removeUnwatedPlayer(names);
    }
    void removeUnwatedPlayer(List<string> names){
        string[] keys = gameManager.players.Keys.ToArray();
        foreach (string key in keys)
        {
            if(!names.Contains(key)){
                Destroy(gameManager.players[key]);
                gameManager.players.Remove(key);
            }
        }
    }
    void summon(string playerName){
        GameObject player = Instantiate(
            playerPrefab,
            new Vector3(
                transform.position.x+(UnityEngine.Random.Range(-2.0f,2.0f)),
                transform.position.y+(UnityEngine.Random.Range(0.0f,0.2f)),
                transform.position.z),
                new Quaternion()
            );
        player.GetComponent<Player>().PlayerName = playerName;
        gameManager.players.Add(playerName,player);
    }
}
