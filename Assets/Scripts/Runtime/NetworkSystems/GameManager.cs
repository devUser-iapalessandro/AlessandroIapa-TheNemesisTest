using Photon.Pun;
using TheNemesisTest.Runtime.Arena;
using TheNemesisTest.Runtime.Data;
using TheNemesisTest.Runtime.Player;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace TheNemesisTest.Runtime.NetworkSystems {
    [RequireComponent(typeof(PhotonView))]
    public class GameManager : MonoBehaviour {
        #region Public Variables
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private GameObject goalPrefab;
        [SerializeField] private int pointsToWin = 3;

        [SerializeField] private TeamDatabaseSO teamDatabase;

        public System.Action<int, int> OnPointsChanged;
        public System.Action OnWin;
        public System.Action OnLose;
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
            _pv = GetComponent<PhotonView>();
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            playerOnePoints = 0;
            playerTwoPoints = 0;

            SceneManager.sceneLoaded += SetupGame;
        }

        private void OnGUI () {
            if(SceneManager.GetActiveScene().buildIndex == 1)
                GUILayout.Label(string.Format("Player One -> {0} - Player Two -> {1}", playerOneInstance, playerTwoInstance));
        }
        #endregion

        #region Public Methods
        public void InstantiatePlayers () {
            if(PhotonNetwork.IsMasterClient) {
                Debug.Log("INSTA MASTER");
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
                object[] data = { 2 };
                playerOneGoalInstance = PhotonNetwork.Instantiate(goalPrefab.name, arena.GetRandomPositionInsideEdges(), Quaternion.identity, data: data);

            }
            else {
                if(playerTwoGoalInstance != null)
                    return;
                object[] data = { 1 };
                playerTwoGoalInstance = PhotonNetwork.Instantiate(goalPrefab.name, arena.GetRandomPositionInsideEdges(), Quaternion.identity, data: data);

            }
        }

        public void AddPoint (int goalIndex) {
            if(goalIndex == 1) {
                playerTwoPoints++;
            }
            else if(goalIndex == 2) {
                playerOnePoints++;
            }

            _pv.RPC(nameof(SendCurrentPoints), RpcTarget.All, playerOnePoints, playerTwoPoints);

            _pv.RPC(nameof(ResetArena), RpcTarget.All);

            _pv.RPC(nameof(CheckWinningCondition), RpcTarget.All);
        }
        #endregion

        #region Private Methods
        [PunRPC]

        private void CheckWinningCondition () {
            if(playerOnePoints == pointsToWin) {
                _pv.RPC(nameof(SetWinner), RpcTarget.All, 1);
            }
            else if(playerTwoPoints == pointsToWin) {
                _pv.RPC(nameof(SetWinner), RpcTarget.All, 2);
            }
            else {
                SetupGame(SceneManager.GetActiveScene());
            }
        }

        private void SetupGame (Scene scene, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
            Debug.LogError($"Scene Index is {scene.buildIndex}");
            if(scene.buildIndex == 1) {
                Debug.LogError("SETUP GAME");
                InstantiatePlayers();
            }
        }

        [PunRPC]
        private void ResetArena () {

            if(PhotonNetwork.IsMasterClient) {
                PhotonNetwork.Destroy(playerOneInstance.gameObject);
                PhotonNetwork.Destroy(ballInstance);
                PhotonNetwork.Destroy(playerOneGoalInstance);
            }
            else {
                PhotonNetwork.Destroy(playerTwoInstance.gameObject);
                PhotonNetwork.Destroy(playerTwoGoalInstance);
            }
            playerOneInstance = null;
            playerTwoInstance = null;
            playerOneGoalInstance = null;
            playerTwoGoalInstance = null;
            ballInstance = null;
        }
        #endregion

        #region RPCs
        [PunRPC]
        private void SendCurrentPoints (int p1Points, int p2Points) {
            playerOnePoints = p1Points;
            playerTwoPoints = p2Points;

            OnPointsChanged?.Invoke(playerOnePoints, playerTwoPoints);
        }

        [PunRPC]
        private void SetWinner (int playerIndex) {
            if(playerIndex == 1 && PhotonNetwork.IsMasterClient || playerIndex == 2 && !PhotonNetwork.IsMasterClient)
                OnWin?.Invoke();
            else {
                OnLose?.Invoke();
            }
        }
        #endregion
    }
}
