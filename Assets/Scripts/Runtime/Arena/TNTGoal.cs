using Photon.Pun;
using TheNemesisTest.Runtime.NetworkSystems;
using UnityEngine;

namespace TheNemesisTest.Runtime.Arena {
    [RequireComponent(typeof(PhotonView))]
    public class TNTGoal : MonoBehaviour {
        [SerializeField] private MeshRenderer goalRenderer;
        private int goalIndex;
        private PhotonView _pv;

        void Awake () {
            _pv = GetComponent<PhotonView>();
            if(_pv.InstantiationData != null) {
                goalIndex = (int)_pv.InstantiationData[0];
                goalRenderer.material = GameManager.Instance.TeamDatabase.teams[goalIndex].goalSkin;
            }
        }

        private void OnTriggerEnter (Collider other) {
            if(other.gameObject.layer == 6) { //ball 
                Debug.Log("Goal");
                GameManager.Instance.AddPoint(goalIndex);
            }
        }
    }
}
