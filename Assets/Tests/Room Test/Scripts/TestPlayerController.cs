using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TestPlayerController : NetworkBehaviour {
    [Header("Movement")]
    [SerializeField] float speed;
    void Start() {

    }

    void Update() {
        if (!isLocalPlayer)
            return;

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
