using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Test.MAS {
    public class NetworkManager : Mirror.NetworkManager {
        #region Değişkenler

        [Header("Multiple Additive Scenes Setup")]
        [HideInInspector] public int gameInstances;

        [Mirror.Scene]
        public string gameScene;

        //bool isAllScenesLoaded;
        //[SerializeField] List<Scene> subScenes = new List<Scene>();
        //int clientIndex;
        //int sceneIndex = 1;

        [SerializeField] int totalNumberOfPlayers;

        [Header("\nRoom Management")]
        public List<Scene> emptyRooms;
        public List<GameObject> sceneManagers;
        public List<string> roomIDs;
        #endregion

        #region Start & Update
        void Update() {
            totalNumberOfPlayers = NetworkServer.connections.Count;
        }
        #endregion

        #region Player Joins & Lefts info kinda stuff
        public override void OnStartServer() {
            base.OnStartServer();
            string log = @"
           _________                                 __________      .__.__       .___             
          /   _____/ ______________  __ ___________  \______   \__ __|__|  |    __| _/             
  ______  \_____  \_/ __ \_  __ \  \/ // __ \_  __ \  |    |  _/  |  \  |  |   / __ |  ______      
 /_____/  /        \  ___/|  | \/\   /\  ___/|  | \/  |    |   \  |  /  |  |__/ /_/ | /_____/      
         /_______  /\___  >__|    \_/  \___  >__|     |______  /____/|__|____/\____ |              
                 \/     \/                 \/                \/                    \/              
                                                                                                   
                                                                                                   
                                                                                                   
                                                                                                   
                                                                                                   
                                                                                                   
___.                                                                                               
\_ |__ ___.__.                                                                                     
 | __ <   |  |                                                                                     
 | \_\ \___  |                                                                                     
 |___  / ____|                                                                                     
     \/\/                                                                                          
__________                     __    _________ .__                                                 
\______   \ ________________ _/  |_  \_   ___ \|__| _____   ____   ____                            
 |    |  _// __ \_  __ \__  \\   __\ /    \  \/|  |/     \_/ __ \ /    \                           
 |    |   \  ___/|  | \// __ \|  |   \     \___|  |  Y Y  \  ___/|   |  \                          
 |______  /\___  >__|  (____  /__|    \______  /__|__|_|  /\___  >___|  /                          
        \/     \/           \/               \/         \/     \/     \/                           
                                                                                                   
                                                                                                   
  ______   ______   ______   ______   ______   ______   ______   ______   ______   ______   ______ 
 /_____/  /_____/  /_____/  /_____/  /_____/  /_____/  /_____/  /_____/  /_____/  /_____/  /_____/ 
                                                                                                   
";
            Debug.Log(log);
        }
        public override void OnServerConnect(NetworkConnection conn) {
            Debug.Log("\nA player has joined: " + conn.address.ToString() + " | Total player count: " + (totalNumberOfPlayers + 1).ToString() + "\n");
        }

        public override void OnServerDisconnect(NetworkConnection conn) {
            base.OnServerDisconnect(conn);
            Debug.Log("\nA player has left the game: " + conn.address.ToString() + " | Total player count: " + (totalNumberOfPlayers - 1).ToString() + "\n");
        }
        #endregion

        #region Move Player to Game Scene
        public void MovePlayerToGameScene(NetworkConnection conn, Scene _scene) {
            SceneManager.MoveGameObjectToScene(conn.identity.gameObject, _scene);
        }
        #endregion

        #region On Server Add Player -UNUSED
        /* public override void OnServerAddPlayer(NetworkConnection conn) {
            base.OnServerAddPlayer(conn);
            //StartCoroutine(OnServerAddPlayerDelayed(conn));
        } */

        /* IEnumerator OnServerAddPlayerDelayed(NetworkConnection conn) {
            while (!isAllScenesLoaded)
                yield return null;

            conn.Send(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.LoadAdditive });

            yield return new WaitForEndOfFrame();

            base.OnServerAddPlayer(conn);

            PlayerDetails playerDetails = conn.identity.GetComponent<PlayerDetails>();
            playerDetails.playerNumber = clientIndex;
            playerDetails.matchIndex = clientIndex % subScenes.Count;

            //Debug.Log("Match Index: " + playerDetails.matchIndex.ToString());
            //Debug.Log("Client Index: " + clientIndex.ToString());
            //Debug.Log("Subscene Count: " + subScenes.Count.ToString());

            clientIndex++;

            if (subScenes.Count > 0)
                MovePlayerToGameScene(conn, subScenes[clientIndex % subScenes.Count]);
        } */
        #endregion

        #region Tüm subScene'leri yükleme -UNUSED
        /* public override void OnStartServer() {
            //StartCoroutine(ServerLoadAllSubScenes());  //Bütün instanceları yüklemesin
        } */

        /* IEnumerator ServerLoadAllSubScenes() {
            for (int i = 0; i < gameInstances; i++) {
                yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });

                Scene newScene = SceneManager.GetSceneAt(i);
                subScenes.Add(newScene);
            }

            isAllScenesLoaded = true;
        } */
        #endregion

        #region Load Single Instance on Server
        public void ServerLoadSingleSubScene(NetworkConnection conn, GameObject _newPlayer, string _matchID) {
            StartCoroutine(ServerLoadSingleSubSceneDelayed(conn, _newPlayer, _matchID));
        }
        IEnumerator ServerLoadSingleSubSceneDelayed(NetworkConnection conn, GameObject _newPlayer, string _matchID) { // GameObject _gameObject // to load single game instances
            Debug.Log("\nCreating a Game Instance... ID: " + _matchID);
            yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });

            Scene[] activeScenes = SceneManager.GetAllScenes();
            Scene newScene = activeScenes[activeScenes.Length - 1]; //? Yeni sahneyi tüm aktif sahneler arasındaki son sahne yaptım.
            //SceneManager.GetSceneAt(sceneIndex);

            ReplaceUIPlayer(conn, _newPlayer);
            MovePlayerToGameScene(conn, newScene);

            GameObject[] sceneManagers = GameObject.FindGameObjectsWithTag("Id");
            foreach (GameObject go in sceneManagers) {
                if (sceneManagers != null || sceneManagers.Length != 0) {

                    if (go.scene == newScene) {
                        //go.name = conn.identity.netId.ToString();
                        //go.GetComponent<SceneDetails>().players.Add(conn.identity.gameObject);
                        go.GetComponent<SceneDetails>().matchID = _matchID; //conn.identity.netId.ToString();
                    }
                }
            }

        }
        #endregion

        #region PlayerJoinGameWithMatchID
        public void JoinGameWithMatchId(NetworkConnection conn, GameObject _newPlayer, string _matchID) {
            Debug.Log("\nPlayer: " + conn.identity.netId + " joining a match: " + _matchID);


            GameObject[] sceneManagers = GameObject.FindGameObjectsWithTag("Id");

            foreach (GameObject go in sceneManagers) {
                bool isRoomFull = go.GetComponent<SceneDetails>().isRoomFull;
                if (go.GetComponent<SceneDetails>().matchID == _matchID && isRoomFull == false) {
                    Scene selectedScene = go.scene;
                    ReplaceUIPlayer(conn, _newPlayer);
                    MovePlayerToGameScene(conn, selectedScene);
                }
            }
        }
        #endregion

        #region On StopServer -UNUSED //! RETURN HERE
        public override void OnStopServer() {
            //NetworkServer.SendToAll(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.UnloadAdditive });
            //StartCoroutine(ServerUnloadSubScenes());
        }

        /* IEnumerator ServerUnloadSubScenes() {
            for (int index = 0; index < subScenes.Count; index++)
                yield return SceneManager.UnloadSceneAsync(subScenes[index]);

            subScenes.Clear();
            isAllScenesLoaded = false;

            yield return Resources.UnloadUnusedAssets();
        } */
        #endregion

        #region OnStopClient
        public override void OnStopClient() {
            // make sure we're not in host mode
            if (mode == NetworkManagerMode.ClientOnly)
                StartCoroutine(ClientUnloadSubScenes());
        }

        IEnumerator ClientUnloadSubScenes() {
            for (int index = 0; index < SceneManager.sceneCount; index++) {
                if (SceneManager.GetSceneAt(index) != SceneManager.GetActiveScene())
                    yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(index));
            }
        }
        #endregion

        #region Switch Player
        public void ReplaceUIPlayer(NetworkConnection conn, GameObject newPrefab) {
            // Cache a reference to the current player object
            GameObject oldPlayer = conn.identity.gameObject;

            // Instantiate the new player object and broadcast to clients
            // Include true for keepAuthority paramater to prevent ownership change
            NetworkServer.ReplacePlayerForConnection(conn, Instantiate(newPrefab), true);

            // Remove the previous player object that's now been replaced
            NetworkServer.Destroy(oldPlayer);
        }

        #endregion

        #region Close Instances OnClientDisconnect
        public override void OnClientDisconnect(NetworkConnection conn) {

            //GameObject[] sceneManagers = GameObject.FindGameObjectsWithTag("Id");
            //foreach (GameObject go in sceneManagers) {
            //    if (go.GetComponent<SceneDetails>().players.Count < go.GetComponent<SceneDetails>().maxPlayerCount) {
            //        //SceneManager.UnloadScene(go.scene);
            //    }
            //}

            base.OnClientDisconnect(conn);
        }
        #endregion

        #region Return empty rooms
        public List<Scene> EmptyRooms() {
            emptyRooms = new List<Scene>();

            GameObject[] sceneManagers = GameObject.FindGameObjectsWithTag("Id");
            if (sceneManagers != null || sceneManagers.Length != 0) {
                foreach (GameObject go in sceneManagers) {
                    if (go.GetComponent<SceneDetails>().isRoomFull == false && !emptyRooms.Contains(go.gameObject.scene)) {
                        emptyRooms.Add(go.gameObject.scene);
                    }
                }
            }
            return emptyRooms;
        }
        #endregion

        #region Find Scenemanagers and return Room Infos;
        public void FindSceneManagers() {
            sceneManagers = new List<GameObject>();

            GameObject[] sceneManagersArray = GameObject.FindGameObjectsWithTag("Id");
            foreach (GameObject go in sceneManagersArray) {
                if (!sceneManagers.Contains(go))
                    sceneManagers.Add(go);
            }

        }

        public List<List<string>> RoomsInfo() {
            Debug.Log("Generating and sending Match data...");
            FindSceneManagers();

            List<string> sceneNames = new List<string>(); //0
            List<string> gameIDs = new List<string>(); //1
            List<string> playerRatios = new List<string>(); //2

            List<List<string>> roomInfos = new List<List<string>>();

            foreach (GameObject sm in sceneManagers) {
                //if (!sceneNames.Contains(sm.scene.name)) {
                //}
                sceneNames.Add(sm.scene.name);

                string newId = sm.GetComponent<SceneDetails>().matchID;
                if (!gameIDs.Contains(newId)) {
                    gameIDs.Add(newId);
                }

                string newRatio = sm.GetComponent<SceneDetails>().players.Count.ToString() + "/" + sm.GetComponent<SceneDetails>().maxPlayerCount.ToString();
                //if (!playerRatios.Contains(newRatio)) {
                //}
                playerRatios.Add(newRatio);

            }

            roomInfos.Add(sceneNames);
            roomInfos.Add(gameIDs);
            roomInfos.Add(playerRatios);

            return roomInfos;
        }
        #endregion

    }
}