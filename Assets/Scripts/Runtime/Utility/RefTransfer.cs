using TheNemesisTest.Runtime.Arena;
using TheNemesisTest.Runtime.NetworkSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RefTransfer : MonoBehaviour
{
    [SerializeField] private TNTArena arena;

    void Awake () {
        if(SceneManager.GetActiveScene().buildIndex == 1) {
            GameManager.Instance.Arena = arena;
        }
    }
}
