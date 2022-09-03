using System.Collections;
using TheNemesisTest.Runtime.NetworkSystems;
using UnityEngine;

namespace TheNemesisTest.Runtime.UI {
    public class TNTLobbyUI : MonoBehaviour {

        #region Public Variables
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject teamChoosingPanel;
        [SerializeField] private GameObject loadingPanel;
        #endregion

        #region Behaviour Callbacks
        IEnumerator Start () {
            while(TNTLobby.Instance == null) {
                Debug.Log("Waiting for lobby to be concrete");
                yield return null;
            }
            TNTLobby.Instance.OnAllPlayersJoined += ToggleTeamChoosingPanel;
            ToggleMainMenuPanel();
        }
        #endregion

        #region Public Methods
        //TODO automate
        public void ToggleMainMenuPanel () {
            mainMenuPanel.SetActive(true);
            teamChoosingPanel.SetActive(false);
            loadingPanel.SetActive(false);
        }

        //TODO automate
        public void ToggleTeamChoosingPanel () {
            mainMenuPanel.SetActive(false);
            teamChoosingPanel.SetActive(true);
            loadingPanel.SetActive(false);
        }

        //TODO automate
        public void ToggleLoadingPanel () {
            mainMenuPanel.SetActive(false);
            teamChoosingPanel.SetActive(false);
            loadingPanel.SetActive(true);
        }
        #endregion
    }
}
