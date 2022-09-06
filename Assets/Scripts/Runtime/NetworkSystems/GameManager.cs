using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TheNemesisTest.Runtime.Arena;
using TheNemesisTest.Runtime.Data;
using TheNemesisTest.Runtime.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheNemesisTest.Runtime.NetworkSystems {
    [RequireComponent(typeof(PhotonView))]
    public class GameManager : MonoBehaviourPunCallbacks {
        #region Public Variables
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private GameObject goalPrefab;
        [SerializeField] private GameObject quickRestartPrefab;
        [SerializeField] private int pointsToWin = 3;

        [SerializeField] private TeamDatabaseSO teamDatabase;

        public System.Action<int, int> OnPointsChanged;
        public System.Action<string> OnTeamHasScored;
        public System.Action OnRoundHasStarted;
        public System.Action<bool> OnGameEnded;
        #endregion

        #region Private Variables
        private static GameManager instance;
        private PhotonView _pv;
        private TNTArena arena;
        private static TNTPlayer playerOneInstance;
        private static TNTPlayer playerTwoInstance;
        private static GameObject ballInstance;
        private static GameObject playerOneGoalInstance;
        private static GameObject playerTwoGoalInstance;
        private int playerOneTeamIndex;
        private int playerTwoTeamIndex;
        private int playerOnePoints;
        private int playerTwoPoints;
        #endregion

        #region Properties
        public static GameManager Instance => instance;

        public TeamDatabaseSO TeamDatabase => teamDatabase;

        public int PlayerOneTeamIndex { get => playerOneTeamIndex; set => playerOneTeamIndex = value; }
        public int PlayerTwoTeamIndex { get => playerTwoTeamIndex; set => playerTwoTeamIndex = value; }
        public TNTArena Arena { get => arena; set => arena = value; }
        #endregion

        #region Behaviour Callbacks
        void Awake () {
            StopAllCoroutines();
            _pv = GetComponent<PhotonView>();

            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            playerOnePoints = 0;
            playerTwoPoints = 0;

            SceneManager.sceneLoaded += SetupGame;
        }

        void OnDestroy () {
            SceneManager.sceneLoaded -= SetupGame;
        }
        #endregion

        #region Public Methods
        public void InstantiatePlayers () {
            if(PhotonNetwork.IsMasterClient) {
                if(playerOneInstance != null)
                    return;
                object[] data = { playerOneTeamIndex };
                playerOneInstance = PhotonNetwork.Instantiate(playerPrefab.name, arena.PlayerOneSpawnPoint.position, Quaternion.identity, data: data).GetComponent<TNTPlayer>();
            }
            else {
                if(playerTwoInstance != null)
                    return;
                object[] data = { playerTwoTeamIndex };
                playerTwoInstance = PhotonNetwork.Instantiate(playerPrefab.name, arena.PlayerTwoSpawnPoint.position, Quaternion.identity, data: data).GetComponent<TNTPlayer>();
            }
            InstantiateGoals();
            InstantiateBall();
        }

        public void InstantiateBall () {
            if(PhotonNetwork.IsMasterClient && ballInstance == null) {
                ballInstance = PhotonNetwork.Instantiate(ballPrefab.name, arena.BallSpawnPoint.position, Quaternion.identity);
            }
        }

        public void InstantiateGoals () {
            if(PhotonNetwork.IsMasterClient) {
                if(playerOneGoalInstance != null)
                    return;
                object[] data = { 1, playerOneTeamIndex };
                playerOneGoalInstance = PhotonNetwork.Instantiate(goalPrefab.name, arena.GetRandomPositionInsideEdges(), Quaternion.identity, data: data);

            }
            else {
                if(playerTwoGoalInstance != null)
                    return;
                object[] data = { 2, playerTwoTeamIndex };
                playerTwoGoalInstance = PhotonNetwork.Instantiate(goalPrefab.name, arena.GetRandomPositionInsideEdges(), Quaternion.identity, data: data);
            }
        }

        public void AddPoint (int goalIndex) {
            int p1OldValue = playerOnePoints;
            int p2OldValue = playerOnePoints;

            if(goalIndex == 1) {
                playerTwoPoints++;
            }
            else if(goalIndex == 2) {
                playerOnePoints++;
            }

            _pv.RPC(nameof(SendCurrentPoints), RpcTarget.All, playerOnePoints, playerTwoPoints, p1OldValue, p2OldValue);

            _pv.RPC(nameof(ResetArena), RpcTarget.All);

            _pv.RPC(nameof(CheckWinningCondition), RpcTarget.All);
        }

        public void GoBackToMainMenu () {
            playerOneInstance = null;
            playerTwoInstance = null;
            playerOneGoalInstance = null;
            playerTwoGoalInstance = null;
            ballInstance = null;
            PhotonNetwork.LeaveRoom(false);
        }

        public void QuickRestart () {
            Instantiate(quickRestartPrefab);
            GoBackToMainMenu();
        }
        #endregion

        #region Private Methods
        private void SetupGame (Scene scene, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
            if(scene.buildIndex == 1) {
                OnRoundHasStarted?.Invoke();
                if(this != null)
                    StartCoroutine(nameof(LoadRound));
            }
        }

        private IEnumerator LoadRound () {
            yield return new WaitForSeconds(3.25f);
            InstantiatePlayers();
        }

        private IEnumerator ScoreScreenCO () {
            yield return new WaitForSeconds(3f);
            SetupGame(SceneManager.GetActiveScene());
        }
        #endregion

        #region RPCs
        [PunRPC]
        private void CheckWinningCondition () {
            if(playerOnePoints == pointsToWin) {
                _pv.RPC(nameof(SetWinner), RpcTarget.All, 1);
            }
            else if(playerTwoPoints == pointsToWin) {
                _pv.RPC(nameof(SetWinner), RpcTarget.All, 2);
            }
            else {
                StartCoroutine(nameof(ScoreScreenCO));
            }
        }

        [PunRPC]
        private void ResetArena () {
            if(PhotonNetwork.IsMasterClient) {
                if(playerOneInstance != null)
                    PhotonNetwork.Destroy(playerOneInstance.gameObject);
                if(ballInstance != null)
                    PhotonNetwork.Destroy(ballInstance);
                if(playerOneGoalInstance != null)
                    PhotonNetwork.Destroy(playerOneGoalInstance);
            }
            else {
                if(playerTwoInstance != null)
                    PhotonNetwork.Destroy(playerTwoInstance.gameObject);
                if(playerTwoGoalInstance != null)
                    PhotonNetwork.Destroy(playerTwoGoalInstance);
            }

            playerOneInstance = null;
            playerTwoInstance = null;
            playerOneGoalInstance = null;
            playerTwoGoalInstance = null;
            ballInstance = null;
        }

        [PunRPC]
        private void SendCurrentPoints (int p1Points, int p2Points, int p1OldValue, int p2OldValue) {
            playerOnePoints = p1Points;
            playerTwoPoints = p2Points;

            OnTeamHasScored?.Invoke(p1OldValue != playerOnePoints ? teamDatabase.teams[playerOneTeamIndex].teamName : teamDatabase.teams[playerTwoTeamIndex].teamName);
            OnPointsChanged?.Invoke(p1Points, p2Points);
        }

        [PunRPC]
        private void SetWinner (int playerIndex) {
            if(playerIndex == 1 && PhotonNetwork.IsMasterClient || playerIndex == 2 && !PhotonNetwork.IsMasterClient)
                OnGameEnded?.Invoke(true);
            else {
                OnGameEnded?.Invoke(false);
            }
        }
        #endregion

        #region Pun Callbacks
        public override void OnDisconnected (DisconnectCause cause) {
            base.OnDisconnected(cause);
            Debug.LogError("OnDisconnected");
        }

        public override void OnLeftRoom () {
            base.OnLeftRoom();
            Debug.LogError("OnLeftRoom");
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerLeftRoom (Photon.Realtime.Player otherPlayer) {
            if(SceneManager.GetActiveScene().buildIndex == 0)
                return;
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.LogError("OnPlayerLeftRoom");
            OnGameEnded?.Invoke(true);
        }

        public override void OnLeftLobby () {
            base.OnLeftLobby();
            Debug.LogError("OnLeftLobby");
        }
        #endregion
    }
}
