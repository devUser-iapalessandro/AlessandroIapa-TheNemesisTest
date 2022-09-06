using System.Collections;
using TheNemesisTest.Runtime.NetworkSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheNemesisTest.Runtime.UI {
    public class HUD : MonoBehaviour {
        [SerializeField] private GameObject goalScreenContainer;
        [SerializeField] private GameObject endGameContainer;
        [SerializeField] private GameObject countdownContainer;
        [SerializeField] private TextMeshProUGUI hudText;
        [SerializeField] private TextMeshProUGUI goalScreenText;
        [SerializeField] private TextMeshProUGUI hasWonOrLostText;
        [SerializeField] private TextMeshProUGUI countdownText;

        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quickRestartButton;

        private WaitForSeconds setPointsTimeInterval;
        private WaitForSeconds goalScreenShowTime;

        #region Behaviour Callbacks
        void Awake () {
            setPointsTimeInterval = new WaitForSeconds(.33f);
            goalScreenShowTime = new WaitForSeconds(3f);
            goalScreenContainer.SetActive(false);
            countdownContainer.SetActive(false);
            hudText.gameObject.SetActive(true);
            endGameContainer.SetActive(false);
            hudText.text = "Home 0 - 0 Visitors";
            hasWonOrLostText.text = string.Empty;
            goalScreenText.text = string.Empty;
            countdownText.text = string.Empty;
        }

        IEnumerator Start () {
            ToggleCountdown();
            while(GameManager.Instance == null)
                yield return null;

            GameManager.Instance.OnTeamHasScored += SetGoalScreenMessage;
            GameManager.Instance.OnRoundHasStarted += ToggleCountdown;
            GameManager.Instance.OnGameEnded += ToggleEndGamePanel;
            GameManager.Instance.OnPointsChanged += SetPoints;
            mainMenuButton.onClick.AddListener(GameManager.Instance.GoBackToMainMenu);
            quickRestartButton.onClick.AddListener(GameManager.Instance.QuickRestart);
        }

        void OnDestroy () {
            GameManager.Instance.OnTeamHasScored -= SetGoalScreenMessage;
            GameManager.Instance.OnRoundHasStarted -= ToggleCountdown;
            GameManager.Instance.OnGameEnded -= ToggleEndGamePanel;
            GameManager.Instance.OnPointsChanged -= SetPoints;
            mainMenuButton.onClick.RemoveListener(GameManager.Instance.GoBackToMainMenu);
            quickRestartButton.onClick.RemoveListener(GameManager.Instance.QuickRestart);
        }
        #endregion

        #region Public Methods
        public void SetPoints (int playerOnePoints, int playerTwoPoints) {
            StopCoroutine(nameof(SetPointsCO));
            StartCoroutine(SetPointsCO(playerOnePoints, playerTwoPoints));
        }

        public void SetGoalScreenMessage (string teamName) {
            StopCoroutine(nameof(SetGoalScreenMessageCO));
            StartCoroutine(SetGoalScreenMessageCO(teamName));
        }

        public void ToggleEndGamePanel (bool hasWon) {
            StopAllCoroutines();
            goalScreenContainer.SetActive(false);
            endGameContainer.SetActive(true);
            hasWonOrLostText.text = hasWon ? "Congratulations!\nYou won!" : "Unlucky!\nYou lost!";
        }

        public void ToggleCountdown () {
            StopCoroutine(nameof(CountdownCO));
            StartCoroutine(nameof(CountdownCO));
        }
        #endregion

        #region Private Methods
        private IEnumerator SetPointsCO (int playerOnePoints, int playerTwoPoints) {
            hudText.text = string.Format("Home {0} - {1} Visitors", playerOnePoints, playerTwoPoints);
            for(int i = 0; i < 3; i++) {
                hudText.gameObject.SetActive(false);
                yield return setPointsTimeInterval;
                hudText.gameObject.SetActive(true);
                yield return setPointsTimeInterval;
            }
            hudText.gameObject.SetActive(true);
        }

        private IEnumerator SetGoalScreenMessageCO (string teamName) {
            goalScreenText.text = $"GOAL ! ! !\n{teamName}\nscored!";
            goalScreenContainer.SetActive(true);
            yield return goalScreenShowTime;
            goalScreenContainer.SetActive(false);
        }

        private IEnumerator CountdownCO () {
            countdownContainer.SetActive(true);
            for(int i = 0; i < 3; i++) {
                countdownText.text = $"{3 - i}!";
                yield return new WaitForSeconds(1f);
            }
            countdownContainer.SetActive(false);
        }
        #endregion
    }
}