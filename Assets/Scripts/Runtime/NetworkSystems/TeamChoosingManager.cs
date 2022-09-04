using Photon.Pun;
using TheNemesisTest.Runtime.Data;
using TheNemesisTest.Runtime.UI;
using UnityEngine;

namespace TheNemesisTest.Runtime.NetworkSystems {
    [RequireComponent(typeof(PhotonView))]
    public class TeamChoosingManager : MonoBehaviour {
        #region Public Variables

        [SerializeField] private TeamDatabaseSO teamsDatabase;
        #endregion

        #region Private Variables
        private PhotonView _pv;
        private static TeamChoosingManager instance;
        private int playerOneIndex;
        private int playerTwoIndex;
        private bool isPlayerOneReady;
        private bool isPlayerTwoReady;
        #endregion

        #region Properties
        public static TeamChoosingManager Instance => instance;
        #endregion

        void Awake () {
            _pv = GetComponent<PhotonView>();

            if(instance == null) {

                instance = this;
            }
        }

        public void Setup () {
            if(PhotonNetwork.IsMasterClient) {
                playerOneIndex = 0;
                playerTwoIndex = 1;
            }
            else {
                playerOneIndex = 1;
                playerTwoIndex = 0;
            }
            _pv.RPC(nameof(SetTeam), RpcTarget.All, 1, playerOneIndex);
            _pv.RPC(nameof(SetTeam), RpcTarget.All, 2, playerTwoIndex);
        }

        public void SwapTexts() => TeamChoosingUI.Instance.SwapTexts();

        [PunRPC]
        private void SetTeam (int caller, int teamIndex) {
            if(caller == 1) {
                playerOneIndex = teamIndex;
            }
            else {
                playerTwoIndex = teamIndex;
            }
            TeamChoosingUI.Instance.SetTeamNameTexts(teamsDatabase.teams[playerOneIndex], teamsDatabase.teams[playerTwoIndex]);
        }

        //Called by switch on UI
        public void GoLeft () {
            if(PhotonNetwork.IsMasterClient) {
                if(playerOneIndex == 0) {
                    playerOneIndex = teamsDatabase.teams.Count - 1;
                }
                else {
                    playerOneIndex--;
                }
                _pv.RPC(nameof(SetTeam), RpcTarget.All, 1, playerOneIndex);
            }
            else {
                if(playerTwoIndex == 0) {
                    playerTwoIndex = teamsDatabase.teams.Count - 1;
                }
                else {
                    playerTwoIndex--;
                }
                _pv.RPC(nameof(SetTeam), RpcTarget.All, 2, playerTwoIndex);
            }
        }

        //Called by switch on UI
        public void GoRight () {
            if(PhotonNetwork.IsMasterClient) {
                if(playerOneIndex == teamsDatabase.teams.Count - 1) {
                    playerOneIndex = 0;
                }
                else {
                    playerOneIndex++;
                }
                _pv.RPC(nameof(SetTeam), RpcTarget.All, 1, playerOneIndex);
            }
            else {
                if(playerTwoIndex == teamsDatabase.teams.Count - 1) {
                    playerTwoIndex = 0;
                }
                else {
                    playerTwoIndex++;
                }
                _pv.RPC(nameof(SetTeam), RpcTarget.All, 2, playerTwoIndex);
            }
        }

        [PunRPC]
        private void SetPlayerReady(int caller) {
            if(caller == 1) {
                isPlayerOneReady = true;
                if(!PhotonNetwork.IsMasterClient)
                    TeamChoosingUI.Instance.SetOpponentReadyness();
            }
            else {
                isPlayerTwoReady = true;
                if(PhotonNetwork.IsMasterClient)
                    TeamChoosingUI.Instance.SetOpponentReadyness();
            }

            if(isPlayerOneReady && isPlayerTwoReady) {
                GameManager.Instance.PlayerOneTeamIndex = playerOneIndex;
                GameManager.Instance.PlayerTwoTeamIndex = playerTwoIndex;
                PhotonNetwork.LoadLevel(1);
            }
        }

        //Called by button on UI
        public void SetReady () {
            if(playerOneIndex == playerTwoIndex) {
                Debug.LogError("Cannot play with the same team");
                return;
            }

            if(PhotonNetwork.IsMasterClient) {
                if(isPlayerOneReady)
                    return;
                else {
                    _pv.RPC(nameof(SetPlayerReady), RpcTarget.All,1);
                }
            }
            else {
                if(isPlayerTwoReady)
                    return;
                else {
                    _pv.RPC(nameof(SetPlayerReady), RpcTarget.All, 2);
                }
            }

            TeamChoosingUI.Instance.SetupPersonalReadyness();
        }
    }
}
