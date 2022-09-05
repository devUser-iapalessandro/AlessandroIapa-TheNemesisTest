using System.Collections;
using TMPro;
using UnityEngine;

namespace TheNemesisTest.Runtime.UI {
    public class HUD : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI hudText;

        private WaitForSeconds wait;

        void Awake () {
            wait = new WaitForSeconds(.05f);
            hudText.text = "Home 0 - Visitors 0";
        }

        public void SetPoints (int playerOnePoints, int playerTwoPoints) {
            StopAllCoroutines();
            StartCoroutine(SetPointsCO(playerOnePoints, playerTwoPoints));
        }

        private IEnumerator SetPointsCO (int playerOnePoints, int playerTwoPoints) {
            hudText.text = "GOAL ! ! !";
            for(int i = 0; i < 3; i++) {
                hudText.gameObject.SetActive(false);
                yield return wait;
                hudText.gameObject.SetActive(true);
            }
            hudText.gameObject.SetActive(true);
            hudText.text = string.Format("Home {0} - Visitors {1}", playerOnePoints, playerTwoPoints);
        }
    }

}