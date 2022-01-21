using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

namespace Test.MAS {
    public class PlayerController : NetworkBehaviour {
        [Header("Player Controller")]
        public float speed = 5;

        [Header("Parameters")]
        [SerializeField] bool isLocal;
        [SyncVar] public string matchID;

        //public string sceneDestination;

        [Header("UI Controller")]
        [SerializeField] Canvas playerUI;
        [SerializeField] Text debugMatchIdText;

        void Start() {
            if (isLocalPlayer) {
                //StartCoroutine(MoveToGameScene());
                playerUI.gameObject.SetActive(true);
                this.gameObject.GetComponent<Renderer>().material.color = Color.red;
            } else {
                //StartCoroutine(MoveToGameScene());
                this.gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
            StartCoroutine(MoveToGameScene());
        }

        void Update() {
            isLocal = isLocalPlayer;

            if (!isLocalPlayer)
                return;

            debugMatchIdText.text = "Match ID: " + matchID.ToString();
            Camera.main.transform.position = new Vector3(this.gameObject.transform.position.x, Camera.main.transform.position.y, this.gameObject.transform.position.z - 10);
        }

        IEnumerator MoveToGameScene() {
            yield return new WaitForSeconds(1);
            SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName("Game"));
        }

        void FixedUpdate() {
            if (!isLocalPlayer)
                return;

            if (Input.GetKey(KeyCode.D))
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            if (Input.GetKey(KeyCode.A))
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            if (Input.GetKey(KeyCode.W))
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            if (Input.GetKey(KeyCode.S))
                transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        }

    }
}