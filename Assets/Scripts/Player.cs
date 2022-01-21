using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
public class Player : NetworkBehaviour {

    public static Player localPlayer; //LocalPlayer'a her yerden erişmek için yaptık. isLocalPlayer'a sadece NetworkBehaviour'lar erişebiliyor çünkü.
    [SyncVar] public string matchID;
    [SerializeField] NetworkMatch networkMatch;
    //[SerializeField] Match currentMatch; //TODO work on this

    [Header("Playable Player")]
    [SerializeField] GameObject playerMeshObj;

    [Header("General")]
    [SyncVar] public bool inGame = false;

    void Start() {
        if (isLocalPlayer) {
            localPlayer = this;
        } else {
            LobbyUI.instance.SpawnUIPlayer();
        }
        networkMatch = GetComponent<NetworkMatch>();
    }

    void Update() {
        /* if (matchID != null && Matchmaker.instance.matches != null) {
            if (Matchmaker.instance.matches.Count != 0) {
                for (int i = 0; i < Matchmaker.instance.matches.Count; i++) {
                    if (matchID == Matchmaker.instance.matches[i].matchID) {
                        localPlayer.currentMatch = Matchmaker.instance.matches[i];
                        break;
                    }
                }
            }
        } */

        if (!isLocalPlayer | !inGame) { return; }

        float speed = 10f;

        if (Input.GetKey(KeyCode.W))
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        if (Input.GetKey(KeyCode.S))
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        if (Input.GetKey(KeyCode.D))
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        if (Input.GetKey(KeyCode.A))
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
    }

    /*
    Host game
    */

    public void HostGame() {
        string _matchID = Matchmaker.GetRandomMatchID();
        CmdHostGame(_matchID);
    }


    [Command]
    void CmdHostGame(string _matchID) {
        matchID = _matchID;
        if (Matchmaker.instance.HostGame(_matchID, this)) {
            Debug.Log("\nGame hosted successfully!" + " \nMatchID: " + matchID + "\n");
            networkMatch.matchId = _matchID.ToGuid();
            TargetHostGame(true, _matchID);
        } else {
            Debug.Log($"Game could not be hosted!");
            TargetHostGame(false, _matchID);
        }
    }

    [TargetRpc]
    void TargetHostGame(bool success, string _matchID) {
        matchID = _matchID;
        LobbyUI.instance.HostSuccess(success);
    }


    /*
    Join game
    */

    public void JoinGame(string _inputMatchID) {
        CmdJoinGame(_inputMatchID);
    }


    [Command]
    void CmdJoinGame(string _matchID) {
        matchID = _matchID;
        if (Matchmaker.instance.JoinGame(_matchID, this)) {
            Debug.Log("\nJoined the game successfully!" + " \nMatchID: " + matchID + "\n");
            networkMatch.matchId = _matchID.ToGuid();
            TargetJoinGame(true, _matchID);
        } else {
            Debug.Log($"Could not be joined!");
            TargetJoinGame(false, _matchID);
        }
    }

    [TargetRpc]
    void TargetJoinGame(bool success, string _matchID) {
        matchID = _matchID;
        LobbyUI.instance.JoinSuccess(success);
    }



    /*
    Begin game
    */

    public void BeginGame() {
        CmdBeginGame();
    }


    [Command]
    void CmdBeginGame() {
        Matchmaker.instance.BeginGame(matchID);
        Debug.Log($"Game beginning! | MatchID: {matchID}");
    }

    public void StartGame() {
        TargetBeginGame();
    }

    [TargetRpc]
    void TargetBeginGame() {
        Debug.Log($"Game beginning! | MatchID = {matchID}");
        LobbyUI.instance.HideUI();
        //TODO Additively load the game scene!
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    public void MovePlayer() {
        Invoke("TargetMovePlayer", 1);
    }

    [TargetRpc]
    void TargetMovePlayer() {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByBuildIndex(2));
        Debug.Log($"Scene Name: " + this.gameObject.scene.name.ToString());
        Debug.Log($"Is it a local player?: " + isLocalPlayer.ToString());
    }


}

