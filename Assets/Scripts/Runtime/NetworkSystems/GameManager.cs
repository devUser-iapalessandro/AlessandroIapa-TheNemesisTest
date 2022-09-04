using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TheNemesisTest.Runtime.Arena;
using TheNemesisTest.Runtime.Data;
using TheNemesisTest.Runtime.Player;
using UnityEngine;

namespace TheNemesisTest.Runtime.NetworkSystems {
    [RequireComponent(typeof(PhotonView))]
    public class GameManager : MonoBehaviour {
        #region Public Variables
        [SerializeField] private GameObject playerPrefab;

        [SerializeField] private int playerOneTeamIndex;
        [SerializeField] private int playerTwoTeamIndex;

        [SerializeField] private TeamDatabaseSO teamDatabase;
        #endregion

        #region Private Variables
        private static GameManager instance;
        private PhotonView _pv;
        private static TNTPlayer playerOne;
        private static TNTPlayer playerTwo;
        private int playerOnePoints;
        private int playerTwoPoints;
        #endregion

        #region Properties
        public static GameManager Instance => instance;

        public TeamDatabaseSO TeamDatabase => teamDatabase;
        #endregion

        void Awake () {
            if(instance == null) {
                instance = this;
            }
            _pv = GetComponent<PhotonView>();
            playerOnePoints = 0;
            playerTwoPoints = 0;
        }

        private void OnGUI () {
            if(GUILayout.Button("Spawn Players"))
                InstantiatePlayers();
        }

        public void InstantiatePlayers () {
            if(PhotonNetwork.IsMasterClient) {
                var p1 = PhotonNetwork.Instantiate(playerPrefab.name, TNTArena.Instance.PlayerOneSpawnPoint.position, Quaternion.identity).GetComponent<TNTPlayer>();
                var p2 = PhotonNetwork.Instantiate(playerPrefab.name, TNTArena.Instance.PlayerTwoSpawnPoint.position, Quaternion.identity).GetComponent<TNTPlayer>();
                playerOne = p1;
                playerTwo = p2;
            }

            _pv.RPC(nameof(SetupPlayers), RpcTarget.All);
        }

        [PunRPC]
        private void SetupPlayers () {
            playerOne.Setup(playerOneTeamIndex);
            playerTwo.Setup(playerTwoTeamIndex);
        }
    }
}
