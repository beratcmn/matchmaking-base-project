using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace Test.MAS {
    public class SceneDetails : MonoBehaviour {
        public string matchID;
        public int maxPlayerCount;
        public bool isRoomFull = false;
        public List<Transform> startPositions;
        public List<GameObject> players;
        public bool isAllPlayersSet = true;

        void Start() {
            StartCoroutine(CheckRoomToDestroy(10));
        }


        void Update() {
            isRoomFull = (players.Count == maxPlayerCount);

            #region Find Players
            GameObject[] allActivePlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject _player in allActivePlayers) {
                if (_player.scene == this.gameObject.scene && !players.Contains(_player)) {
                    players.Add(_player);
                }
            }

            players.RemoveAll(GameObject => GameObject == null);
            #endregion

            #region Set Player MatchIDs
            foreach (GameObject _player in players) {
                if (_player.GetComponent<PlayerController>().matchID == "" || _player.GetComponent<PlayerController>().matchID == null)
                    _player.GetComponent<PlayerController>().matchID = matchID;
            }
            #endregion

            #region Set start positions
            if (players.Count == startPositions.Count) {
                SetPlayers();
            }
            #endregion
        }

        #region Destroy empty rooms
        IEnumerator CheckRoomToDestroy(int _time) {
            yield return new WaitForSeconds(_time);
            if (players.Count == 0) {
                SceneManager.UnloadSceneAsync(this.gameObject.scene);
            }

            //? Coroutine çalışırken scene unload oluyor ve hata atıyor console'a o yüzden try ile denedim
            try {
                StartCoroutine(CheckRoomToDestroy(5));
            } catch (System.Exception) {

            }
        }
        #endregion

        [Client]
        public void SetPlayers() {
            if (!isAllPlayersSet) {
                for (int i = 0; i < players.Count; i++) {
                    //players[i].transform.position = startPositions[i].position;
                    int j = Random.Range(0, startPositions.Count);
                    players[j].transform.position = startPositions[j].position;
                    startPositions.Remove(startPositions[j]);
                }
                isAllPlayersSet = true;
            }
        }

    }
}