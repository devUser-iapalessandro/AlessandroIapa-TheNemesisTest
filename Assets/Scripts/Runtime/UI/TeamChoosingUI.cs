using TheNemesisTest.Runtime.Data;
using TMPro;
using UnityEngine;

namespace TheNemesisTest.Runtime.UI {

    public class TeamChoosingUI : MonoBehaviour {
        #region Public Variables
        [SerializeField] private TextMeshProUGUI _homeTeamName;
        [SerializeField] private TextMeshProUGUI _oppositeTeamName;
        #endregion

        #region Private Variables
        private static TeamChoosingUI instance;
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
        #endregion

        #region Public Methods
        public void SetTeamNameTexts (TeamDataSO homeData, TeamDataSO opposerData) {
            _homeTeamName.text = homeData.teamName;
            _oppositeTeamName.text = opposerData.teamName;
        }

        public void SwapTexts () {
            Debug.LogError("SWAP");
            var temp = _homeTeamName;
            _homeTeamName = _oppositeTeamName;
            _oppositeTeamName = temp;
        }
        #endregion

    }
}
