using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

namespace Test.MAS {
    public class UIPlayer : NetworkBehaviour {
        [SerializeField] GameObject player;
        public bool customIsLocal;
        [SyncVar] public List<Scene> emptyRooms;
        [SyncVar] public List<List<string>> roomInfos;
        [Header("UI Controller")]
        [SerializeField] Canvas wholeUI;
        [SerializeField] GameObject roomItem;
        [SerializeField] GameObject roomListParent;
        public string matchID;

        #region Start And Update 
        void Start() {
            if (isLocalPlayer) {
                wholeUI.gameObject.SetActive(true);
            }
        }

        void Update() {
            customIsLocal = isLocalPlayer;

            if (!isLocalPlayer)
                return;

        }
        #endregion

        #region Generate Random MatchID
        public string GenerateMatchID() {
            return Random.Range(100_000, 999_999).ToString();
        }
        #endregion

        #region Join Random Room
        public void JoinRandomRoom() {
            Debug.Log("Looking for empty rooms...");
            CmdFindEmptyRoomsAndJoin();
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }
        #endregion

        #region Create New Room
        public void CreateNewRoom() {
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
            CmdInstanceOpenRequest(GenerateMatchID());
        }
        #endregion

        #region UI Functions

        #region Refresh Room
        public void RefreshRooms() {
            #region Currently not used
            //CmdFindSceneManagers();


            /*
            //? Clear the current room list on UI
            if (roomListParent.transform.childCount > 0) {
                foreach (GameObject currentRoomItem in roomListParent.transform) {
                    GameObject.Destroy(currentRoomItem);
                }
            }


            for (int i = 0; i < sceneManagers.Count; i++) {
                GameObject newRoomItem = Instantiate(roomItem);
                newRoomItem.transform.parent = roomListParent.transform;
                GameObject newRoomItemFrame = newRoomItem.transform.GetChild(0).gameObject;
                newRoomItemFrame.transform.GetChild(1).gameObject.GetComponent<Text>().text = sceneManagers[i].GetComponent<SceneDetails>().matchID;
                newRoomItemFrame.transform.GetChild(2).gameObject.GetComponent<Text>().text = sceneManagers[i].GetComponent<SceneDetails>().players.Count.ToString() + "/" + sceneManagers[i].GetComponent<SceneDetails>().maxPlayerCount;
            }

            */

            //CmdPlayerPairs();
            //CmdGetRoomIDs();

            //if (roomListParent.transform.childCount > 0) {
            //    foreach (Transform currentRoomItem in roomListParent.transform) {
            //        GameObject.Destroy(currentRoomItem.gameObject);
            //    }
            //}


            //for (int i = 0; i < roomIDs.Count; i++) {
            //    GameObject newRoomItem = Instantiate(roomItem);
            //    newRoomItem.transform.parent = roomListParent.transform;
            //    GameObject newRoomItemFrame = newRoomItem.transform.GetChild(0).gameObject;
            //    newRoomItemFrame.transform.GetChild(1).gameObject.GetComponent<Text>().text = roomIDs[i];
            //    newRoomItemFrame.transform.GetChild(2).gameObject.GetComponent<Text>().text = playerPairs[i];
            //}
            #endregion

            StartCoroutine(RefreshRoomsDelayed());
        }

        IEnumerator RefreshRoomsDelayed() {
            CmdFindRooms();

            yield return new WaitForSeconds(2); //her şeyin serverdan gelmesini bekliyoruz

            //Reset room ui
            if (roomListParent.transform.childCount > 0) {
                foreach (Transform currentRoomItem in roomListParent.transform) {
                    GameObject.Destroy(currentRoomItem.gameObject);
                }
            }



            for (int i = 0; i < roomInfos[1].Count; i++) {
                string sceneName = roomInfos[0][i];
                string gameID = roomInfos[1][i];
                string playerRatio = roomInfos[2][i];

                GameObject newRoomItem = Instantiate(roomItem);
                newRoomItem.transform.parent = roomListParent.transform;
                GameObject newRoomItemFrame = newRoomItem.transform.GetChild(0).gameObject;

                newRoomItemFrame.transform.GetChild(0).gameObject.GetComponent<Text>().text = sceneName;
                newRoomItemFrame.transform.GetChild(1).gameObject.GetComponent<Text>().text = gameID;
                newRoomItemFrame.transform.GetChild(2).gameObject.GetComponent<Text>().text = playerRatio;
            }

        }
        #endregion

        #region Join Game with MatchID
        public void JoinGameWithMatchID(GameObject frame) {
            string sceneName = frame.transform.GetChild(0).gameObject.GetComponent<Text>().text;
            matchID = frame.transform.GetChild(1).gameObject.GetComponent<Text>().text;
            Debug.Log("Scene name: " + sceneName);
            Debug.Log("Match ID: " + matchID);
            CmdJoinRoomWithMatchID(matchID);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        #endregion

        #endregion

        #region Commands
        [Command]
        void CmdInstanceOpenRequest(string _matchID) {
            NetworkManager.singleton.GetComponent<NetworkManager>().ServerLoadSingleSubScene(connectionToClient, player, _matchID);
        }

        [Command]
        void CmdFindEmptyRoomsAndJoin() {
            emptyRooms = NetworkManager.singleton.GetComponent<NetworkManager>().EmptyRooms();

            if (emptyRooms.Count > 0) {
                NetworkManager.singleton.GetComponent<NetworkManager>().ReplaceUIPlayer(connectionToClient, player);
                NetworkManager.singleton.GetComponent<NetworkManager>().MovePlayerToGameScene(connectionToClient, emptyRooms[0]);
            }
        }

        [Command]
        void CmdFindRooms() {
            roomInfos = NetworkManager.singleton.GetComponent<NetworkManager>().RoomsInfo();
        }

        [Command]
        void CmdJoinRoomWithMatchID(string _matchId) {
            NetworkManager.singleton.GetComponent<NetworkManager>().JoinGameWithMatchId(connectionToClient, player, _matchId);
        }
        #endregion

    }

}
