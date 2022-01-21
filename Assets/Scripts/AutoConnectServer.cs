using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AutoConnectServer : MonoBehaviour {
    [SerializeField] NetworkManager networkManager;

    void Start() {
        if (!Application.isBatchMode) {
            Debug.Log("=== Client connecting... ===");
            Join();
        } else {
            Debug.Log("\n=== Server starting... ===\n");
            networkManager.StartServer();
        }
    }
    public void Join() {
        networkManager.networkAddress = "84.54.12.196";
        networkManager.StartClient();
        //networkManager.StartHost();
    }

}
