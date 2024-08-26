using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public delegate void ActivateTiang(int nomorTiang, int tinggiTiang);
    public ActivateTiang OnTiangActivate;
    public delegate void gameStart();
    public gameStart OnGameStart;
    public delegate void gameOver();
    public gameStart OnGameOver;
    public Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    private List<GameObject> listOfPlayers;
    [SerializeField]
    private GameObject[] tiangs;
    public List<GameObject> tiangsActive;
    public GameObject leadingPlayers;
    Coroutine getLeadingPlayerCotoutine;
    public bool isGameOver = false;
    public List<GameObject> winner;
    [SerializeField] Podium podiumArea;
    bool isStarted = false;
    void Start(){
        foreach (GameObject tiang in tiangs)
        {
            tiang.GetComponent<Tiang>().OnEnableEvent += tiangEnable;
            tiang.GetComponent<Tiang>().OnDisableEvent += tiangDisable;
        }
        OnGameStart += mulai;
    }

    private void mulai()
    {
        isStarted = true;
        listOfPlayers = players.Values.ToList<GameObject>();
        getLeadingPlayerCotoutine = StartCoroutine(getLeadingPlayer());

        // throw new NotImplementedException();
    }
    void Update(){
        if(tiangsActive.Count == 0 && !isGameOver){
            Debug.Log("GameOver");
            isGameOver = true;
            OnGameOver?.Invoke();
            foreach (GameObject _player in listOfPlayers)
            {
                if(!winner.Contains(_player)){
                    _player.GetComponent<Player>().kalah();
                }
            }
            // podiumArea.transform.parent.gameObject.SetActive(true);
            // StopAllCoroutines();
            podiumArea.GetComponent<Canvas>().enabled = true;
            podiumArea.setPodium(winner);
            isStarted = false;
        }
    }
    private void tiangDisable(GameObject tiang)
    {
        tiangsActive.Remove(tiang);
    }

    private void tiangEnable(GameObject tiang)
    {
        tiangsActive.Add(tiang);
    }
    public IEnumerator getLeadingPlayer(){
        List<GameObject> _playersGameObject = null;
        List<Player> _players = null;
        int prevPlayersCount = 0;
        while (true)
        {;
            if(prevPlayersCount != players.Count){
                _playersGameObject = players.Values.ToList<GameObject>();
                _players  = new List<Player>();
                foreach (GameObject _playerGameObject in _playersGameObject)
                {
                    _players.Add(_playerGameObject.GetComponent<Player>());
                }
                prevPlayersCount = players.Count;
            }
            Player _leadingPlayer = null;
            // Debug.Log(_players.Count);
            foreach (Player _player in _players)
            {
                if (_player.tiang != null){
                    if(_leadingPlayer == null){
                        _leadingPlayer = _player;
                    }else{
                        float leadingPlayerDistance = Vector2.Distance(_leadingPlayer.transform.position, _leadingPlayer.tiang.GetComponent<Tiang>().tiangReward.transform.position);
                        float playerDistance = Vector2.Distance(_player.transform.position, _player.tiang.GetComponent<Tiang>().tiangReward.transform.position);
                        if(leadingPlayerDistance > playerDistance){
                            _leadingPlayer = _player;
                            // yield return new WaitForSecondsRealtime(1);
                        }
                        // Debug.Log(leadingPlayerDistance);
                    }
                }
                
            }
            if(_leadingPlayer!=null){
                leadingPlayers = _leadingPlayer.gameObject;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
