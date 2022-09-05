using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TheNemesisTest.Runtime.UI;
using UnityEngine;

namespace TheNemesisTest.Runtime.NetworkSystems {
    [RequireComponent(typeof(PhotonView))]
    public class TNTLobby : MonoBehaviourPunCallbacks {
        #region Private Fields
        private PhotonView _photonView;
        #endregion

        #region Public Fields
        public System.Action OnAllPlayersJoined;
        #endregion

        #region Properties
        public static TNTLobby Instance { get; private set; }
        #endregion

        #region Behaviour Callbacks
        private void Awake () {
            if(Instance != null)
                Destroy(gameObject);

            Instance = this;

            _photonView = GetComponent<PhotonView>();
        }

        IEnumerator Start () {
            Connect();
            while(!PhotonNetwork.IsConnected) {
                Debug.Log("Connecting to master client...");
                yield return null;
            }
            if(PhotonNetwork.IsMasterClient) {
                TNTLobbyUI.Instance.ToggleMainMenuPanel();
                Debug.Log("I AM MASTER CLIENT");
            }
        }
        #endregion

        #region Photon Callbacks
        public override void OnConnectedToMaster () {
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
            Debug.Log("Connected to master");
            TNTLobbyUI.Instance.ToggleMainMenuPanel();
        }

        //Triggered on a room's creation or joining event
        public override void OnJoinedRoom () {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2) {
                OnAllPlayersJoined?.Invoke();
                if(PhotonNetwork.IsMasterClient)
                    TeamChoosingManager.Instance.Setup();
                else {
                    TeamChoosingManager.Instance.SwapTexts();
                }
            }
        }

        public override void OnJoinRoomFailed (short returnCode, string message) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
        }

        //Triggered automatically whenever a user enters a specific room in the Lobby
        public override void OnPlayerEnteredRoom (Photon.Realtime.Player newPlayer) {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2) {
                OnAllPlayersJoined?.Invoke();
                if(PhotonNetwork.IsMasterClient)
                    TeamChoosingManager.Instance.Setup();
                else {
                    TeamChoosingManager.Instance.SwapTexts();
                }
            }
        }

        public override void OnJoinRandomFailed (short returnCode, string message) {
            CreateRoom();
        }

        public override void OnDisconnected (DisconnectCause cause) {
            Debug.LogErrorFormat("Disconnected with disconnetion cause: {0}", cause);
        }
        #endregion

        #region Public Methods
        public void Connect () {
            PhotonNetwork.ConnectUsingSettings();

            //if(PhotonNetwork.IsConnectedAndReady)
            //    PhotonNetwork.NickName = "TNT_Guest" + "_" + Random.Range(0, 100) + (char)Random.Range(65, 91);
        }

        public void JoinRandomRoom () {
            PhotonNetwork.JoinRandomRoom();
        }

        public void StopLookingForRoom () {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Stop looking for a room");
        }

        public void CreateRoom () {
            string roomName = "TNT_Room_" + Random.Range(1, 100);

            RoomOptions options = new RoomOptions() {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 2
            };

            PhotonNetwork.CreateRoom(roomName, options);
            Debug.LogFormat("Room with name {0} created. Waiting for someone to join...", roomName);
        }

        public void ExitGame () {
            if(PhotonNetwork.IsConnected) {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.SendAllOutgoingCommands();
                PhotonNetwork.Disconnect();
            }
            Application.Quit();
        }

        public override void OnPlayerLeftRoom (Photon.Realtime.Player otherPlayer) {
            Debug.LogErrorFormat("Player {0} left the game, going back to main menu", otherPlayer.NickName);
            StopLookingForRoom();
            TNTLobbyUI.Instance.ToggleMainMenuPanel();
        }

        private void OnApplicationQuit () {
            if(PhotonNetwork.IsConnected) {
                PhotonNetwork.Disconnect();
            }
        }
        #endregion

    }
}
