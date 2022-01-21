using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {

    public static LobbyUI instance;

    [Header("Host & Join")]
    [SerializeField] List<Selectable> uiSelectables = new List<Selectable>();
    [SerializeField] InputField joinMatchInput;
    [SerializeField] Canvas lobbyCanvas;

    [Header("Lobby")]
    [SerializeField] Transform UIPlayerParent;
    [SerializeField] GameObject UIPlayerPrefab;
    [SerializeField] Text matchIDText;
    [SerializeField] Button beginGameButton;
    public GameObject wholeUI;

    void Start() {
        instance = this;
    }

    public void Join() {
        if (joinMatchInput.text != "") {
            Debug.Log("Joining the lobby...");
            joinMatchInput.interactable = false;

            uiSelectables.ForEach(x => x.interactable = false);

            Player.localPlayer.JoinGame(joinMatchInput.text.ToUpper());
        }
    }

    public void CreateRoom() {
        Debug.Log("Creating a lobby...");
        joinMatchInput.interactable = false;

        uiSelectables.ForEach(x => x.interactable = false);

        Player.localPlayer.HostGame();
    }

    public void JoinSuccess(bool success) {
        if (success) {
            lobbyCanvas.enabled = true;
            SpawnUIPlayer();
            matchIDText.text = Player.localPlayer.matchID;
        } else {
            joinMatchInput.interactable = true;

            uiSelectables.ForEach(x => x.interactable = true);
        }

    }
    public void HostSuccess(bool success) {
        if (success) {
            lobbyCanvas.enabled = true;
            SpawnUIPlayer();
            matchIDText.text = Player.localPlayer.matchID;
            beginGameButton.gameObject.SetActive(true);
        } else {
            joinMatchInput.interactable = true;

            uiSelectables.ForEach(x => x.interactable = true);
        }
    }

    public void SpawnUIPlayer() {
        GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
    }

    public void BeginGame() {
        Player.localPlayer.BeginGame();
        //Player.localPlayer.ReplacePlayer();
    }

    public void HideUI() {
        wholeUI.SetActive(false);
    }

}
