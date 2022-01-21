using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MAS {
    public class UIRoomItem : MonoBehaviour {
        public GameObject player;
        void Start() {

        }

        void Update() {
            foreach (GameObject pl in GameObject.FindGameObjectsWithTag("UIPlayer")) {
                if (pl.GetComponent<UIPlayer>().customIsLocal) {
                    player = pl;
                    break;
                }
            }


        }

        public void JoinMatch(GameObject _frame) {
            player.GetComponent<UIPlayer>().JoinGameWithMatchID(_frame);
        }
    }
}