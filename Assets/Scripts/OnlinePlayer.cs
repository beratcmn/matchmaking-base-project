using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class OnlinePlayer : NetworkBehaviour {
    [SerializeField] NetworkMatch networkMatch;
    public string matchID;
    public string netID;

    void Start() {
        networkMatch = GetComponent<NetworkMatch>();
    }
    private void Update() {
        if (!isLocalPlayer) { return; }

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
}
