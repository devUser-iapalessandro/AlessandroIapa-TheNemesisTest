using TheNemesisTest.Runtime.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheNemesisTest.Runtime.UI {

    public class TeamChoosingUI : MonoBehaviour {
        #region Public Variables
        [SerializeField] private TextMeshProUGUI homeTeamName;
        [SerializeField] private TextMeshProUGUI oppositeTeamName;
        [SerializeField] private Button leftSwitch;
        [SerializeField] private Button rightSwitch;
        [SerializeField] private TextMeshProUGUI personalReadyButtonText;
        [SerializeField] private TextMeshProUGUI opponentReadyText;
        #endregion

        #region Private Variables
        private static TeamChoosingUI instance;
        private Color readyColor = Color.green;
        #endregion

        #region Properties
        public static TeamChoosingUI Instance => instance;
        #endregion

        #region Behaviour Callbacks
        void Awake () {
            if(instance == null) {

                instance = this;
            }
        }

        void Start () {
            personalReadyButtonText.color = Color.white;
            opponentReadyText.color = Color.white;
            leftSwitch.interactable = true;
            rightSwitch.interactable = true;
            readyColor = Color.green;
        }
        #endregion

        #region Public Methods
        public void SetTeamNameTexts (TeamDataSO homeData, TeamDataSO opposerData) {
            homeTeamName.text = homeData.teamName;
            oppositeTeamName.text = opposerData.teamName;
        }

        public void SwapTexts () {
            Debug.LogError("SWAP");
            var temp = homeTeamName;
            homeTeamName = oppositeTeamName;
            oppositeTeamName = temp;
        }

        public void SetupPersonalReadyness () {
            personalReadyButtonText.color = readyColor;
            leftSwitch.interactable = false;
            rightSwitch.interactable = false;
        }

        public void SetOpponentReadyness () {
            opponentReadyText.color = readyColor;
        }
        #endregion

    }
}
