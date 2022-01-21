using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Mirror;
using UnityEngine;

[System.Serializable] // network uyumlu olsun diye böyle yaptık
public class Match {
    public string matchID;
    public List<Player> players = new List<Player>();

    public Match(string _matchID, Player _player) {
        this.matchID = _matchID;
        players.Add(_player);
    }

    public Match() { } // blank constructor

}

public class Matchmaker : NetworkBehaviour {

    public static Matchmaker instance;
    public SyncList<Match> matches = new SyncList<Match>();
    public SyncList<String> matchIDs = new SyncList<String>();
    [SerializeField] GameObject gameManagerPrefab;


    void Start() {
        instance = this;
        Debug.Log("\nMatchmaker initialized!\n");
    }


    public bool HostGame(string _matchID, Player _player) // burada host olup olmadığını kontrol ediyoruz
    {
        if (!matchIDs.Contains(_matchID)) {
            matchIDs.Add(_matchID);
            Match match = new Match(_matchID, _player);
            matches.Add(match);
            Debug.Log("\nMatch generated! ID: " + _matchID);
            return true;
        } else {
            Debug.Log("MatchID already exists!");
            return false;
        }
    }


    public bool JoinGame(string _matchID, Player _player) // burada host olup olmadığını kontrol ediyoruz
    {
        if (matchIDs.Contains(_matchID)) {

            for (int i = 0; i < matches.Count; i++) {
                if (matches[i].matchID == _matchID) {
                    matches[i].players.Add(_player);
                    break;
                }
            }

            Debug.Log("\nJoined the match! ID: " + _matchID);
            return true;
        } else {
            Debug.Log("MatchID does not exists!");
            return false;
        }
    }


    public void BeginGame(string _matchID) {
        GameObject newGameManager = Instantiate(gameManagerPrefab);
        NetworkServer.Spawn(newGameManager);
        newGameManager.GetComponent<NetworkMatch>().matchId = _matchID.ToGuid();
        MainGameManager _gameManager = newGameManager.GetComponent<MainGameManager>();

        for (int i = 0; i < matches.Count; i++) {
            if (matches[i].matchID == _matchID) {
                foreach (var player in matches[i].players) {
                    Player _player = player.GetComponent<Player>();
                    _gameManager.AddPlayer(_player); //Burada match içindeki herbir player'a bakıyoruz ve onlardaki StartGame fonksiyonunu çağırıyoruz. Bu şekilde her player kendi gamescene'ini çağırıyor
                    _player.inGame = true; //Burada player'in oyunda olduğunu söylüyoruz
                    _player.StartGame();
                    _player.MovePlayer();
                }
                break;
            }
        }

        //_gameManager.PlacePlayers();
    }


    public static string GetRandomMatchID() {
        string _id = string.Empty;

        for (int i = 0; i < 5; i++) {
            int random = UnityEngine.Random.Range(0, 36);
            if (random < 26) {
                _id += (char)(random + 65); //sadece büyük harfleri istiyoruz diye 65 kaydırıyoruz.
            } else {
                _id += (random - 26).ToString();
            }
        }

        Debug.Log($"Your random match id: {_id}");
        return _id;
    }


}
public static class MatchExtensions {
    public static Guid ToGuid(this string id) {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);

        return new Guid(hashBytes);
    }
}