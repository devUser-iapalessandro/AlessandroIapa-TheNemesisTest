using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace TheNemesisTest.Runtime.NetworkSystems {
    [RequireComponent(typeof(PhotonView))]
    public class TNTLobby : MonoBehaviourPunCallbacks {
        #region Private Fields
        private PhotonView _photonView;
        #endregion

        #region Public Fields
        public System.Action<List<RoomInfo>> OnRoomsListChanged;
        public System.Action OnRoomJoined;
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
        #endregion

        #region Photon Callbacks
        public override void OnConnectedToMaster () {
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
            Debug.Log("Connected to master");

        }

        //Triggered automatically whenever a user enters a Lobby in the Master Server
        public override void OnJoinedLobby () {
            Debug.Log("Connected to Lobby");
        }

        //Triggered on a room's creation or joining event
        public override void OnJoinedRoom () {
            OnRoomJoined?.Invoke();
        }

        public override void OnJoinRoomFailed (short returnCode, string message) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
        }

        //Triggered automatically whenever a user enters a specific room in the Lobby
        public override void OnPlayerEnteredRoom (Photon.Realtime.Player newPlayer) {
            Debug.Log($"Player {newPlayer.NickName} entered the room");
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
                Debug.Log("GAME STARTING!");

            //PhotonNetwork.LoadLevel(1);
        }

        //Triggered by Photon Network
        public override void OnRoomListUpdate (List<RoomInfo> roomList) {
            OnRoomsListChanged?.Invoke(roomList);
        }

        public override void OnCreatedRoom () {

        }

        public override void OnCreateRoomFailed (short returnCode, string message) {

        }

        public override void OnJoinRandomFailed (short returnCode, string message) {
            Debug.LogError("Could not find any match, creating a new room");
            CreateRoom();
        }
        #endregion

        #region Public Methods
        public void Connect () {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = "TNT_Guest" + "_" + Random.Range(0, 100) + (char)Random.Range(65, 91);
        }

        public void JoinRandomRoom () {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("Looking for a random room");
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

        public void JoinRoom (RoomInfo infos) {
            PhotonNetwork.JoinRoom(infos.Name);
            Debug.Log("Room Joined");
        }

        public void ExitGame () {
            if(PhotonNetwork.IsConnected)
                PhotonNetwork.Disconnect();
            Application.Quit();
        }
        #endregion

    }
}
