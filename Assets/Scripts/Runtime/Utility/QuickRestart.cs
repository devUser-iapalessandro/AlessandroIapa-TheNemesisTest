using TheNemesisTest.Runtime.NetworkSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheNemesisTest.Runtime.Utility {
    public class QuickRestart : MonoBehaviour {
        private void Start () {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += TriggerQuickRestart;
        }

        private void TriggerQuickRestart (Scene scene, LoadSceneMode mode) {
            if(scene.buildIndex == 0) {
                TNTLobby.Instance.EnableQuickStart = true;
                Destroy(gameObject);
            }
        }

        void OnDestroy () {
            SceneManager.sceneLoaded -= TriggerQuickRestart;

        }
    }
}