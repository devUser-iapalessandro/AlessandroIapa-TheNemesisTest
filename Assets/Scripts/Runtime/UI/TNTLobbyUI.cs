using System.Collections;
using TheNemesisTest.Runtime.NetworkSystems;
using UnityEngine;

namespace TheNemesisTest.Runtime.UI {
    public class TNTLobbyUI : MonoBehaviour {

        #region Public Variables
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject teamChoosingPanel;
        [SerializeField] private GameObject searchingPanel;
        [SerializeField] private GameObject loadingPanel;
        #endregion

        #region Properties
        private static TNTLobbyUI instance;
        public static TNTLobbyUI Instance => instance;
        #endregion

        #region Behaviour Callbacks
        private void Awake () {
            if(instance == null) {
                instance = this;
            }
        }

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
            searchingPanel.SetActive(false);
        }

        //TODO automate
        public void ToggleTeamChoosingPanel () {
            mainMenuPanel.SetActive(false);
            teamChoosingPanel.SetActive(true);
            searchingPanel.SetActive(false);
        }

        //TODO automate
        public void ToggleSearchingPanel () {
            mainMenuPanel.SetActive(false);
            teamChoosingPanel.SetActive(false);
            searchingPanel.SetActive(true);
        }
        #endregion
    }
}
