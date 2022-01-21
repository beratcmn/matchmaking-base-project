using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RaceManager : MonoBehaviour {
    [SerializeField] GameObject testobj;
    void Start() {
        GameObject _test = Instantiate(testobj);
        NetworkServer.Spawn(_test);
    }

    // Update is called once per frame
    void Update() {

    }
}
