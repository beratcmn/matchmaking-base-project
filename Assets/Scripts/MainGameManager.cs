using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MainGameManager : NetworkBehaviour {

    [SerializeField] List<Player> playerList = new List<Player>();
    [SerializeField] GameObject[] spawnPoints;
    bool isAllPlayersPlaced;

    public void AddPlayer(Player _player) {
        playerList.Add(_player);
    }

    void Start() {
        //Player.localPlayer.ReplacePlayer();
    }

    void Update() {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");


        //if (isAllPlayersReady && !isAllPlayersPlaced) {
        //    for (int i = 0; i < playerList.Count; i++) {
        //        playerList[i].gameObject.transform.position = spawnPoints[i].transform.position;
        //    }
        //
        //foreach (Player _player in playerList) {
        //    _player.gameObject.transform.position = spawnPoints[0].transform.position;
        //}
        //
        //    isAllPlayersPlaced = true;
        //}

        /* if (isAllPlayersReady && isAllPlayersPlaced)
            foreach (Player _player in playerList) { _player.ActivatePlayerMesh(); } */ // Active the 3d player when everything is setted up

    }
    /*  public void SwitchPlayers() {
         foreach (Player _player in playerList) {
             _player.SwitchPlayer();
         }
     } */

    public void PlacePlayers() {
        foreach (Player _player in playerList) {
            Debug.Log("Player Name: " + _player.name);
        }
    }
}
