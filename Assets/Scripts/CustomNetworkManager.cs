using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager {

    [SerializeField] int totalNumberOfPlayer = 0;

    public override void OnServerConnect(NetworkConnection conn) {
        totalNumberOfPlayer++;
        Debug.Log("\nA player has joined: " + conn.address.ToString() + " | Total player count: " + totalNumberOfPlayer.ToString() + "\n");
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn); // Burada built in bir şekilde dc olanları destroyluyor
        totalNumberOfPlayer--;
        Debug.Log("\nA player has left the game: " + conn.address.ToString() + " | Total player count: " + totalNumberOfPlayer.ToString() + "\n");
    }

    public void ReplacePlayer(NetworkConnection conn, GameObject _newPlayerObj, Player _player) {
        Debug.Log($"Test message!");
        // Cache a reference to the current player object
        GameObject oldPlayer = conn.identity.gameObject;

        // Instantiate the new player object and broadcast to clients
        // Include true for keepAuthority paramater to prevent ownership change
        GameObject newPlayer = Instantiate(_newPlayerObj);
        newPlayer.GetComponent<NetworkMatch>().matchId = _player.matchID.ToGuid();
        newPlayer.GetComponent<OnlinePlayer>().matchID = _player.matchID;
        newPlayer.GetComponent<OnlinePlayer>().netID = _player.matchID.ToGuid().ToString();
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer, true);

        // Remove the previous player object that's now been replaced
        NetworkServer.Destroy(oldPlayer);
    }
}



