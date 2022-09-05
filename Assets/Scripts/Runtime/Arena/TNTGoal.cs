using Photon.Pun;
using TheNemesisTest.Runtime.NetworkSystems;
using UnityEngine;

namespace TheNemesisTest.Runtime.Arena {
    [RequireComponent(typeof(PhotonView))]
    public class TNTGoal : MonoBehaviour {
        [SerializeField] private MeshRenderer goalRenderer;
        private int goalOwnership;
        private int goalMaterialIndex;
        private PhotonView _pv;

        void Awake () {
            _pv = GetComponent<PhotonView>();
            if(_pv.InstantiationData != null) {
                goalOwnership = (int)_pv.InstantiationData[0];
                goalMaterialIndex = (int)_pv.InstantiationData[1];
                goalRenderer.material = GameManager.Instance.TeamDatabase.teams[goalMaterialIndex].goalSkin;
            }
        }

        private void OnTriggerEnter (Collider other) {
            if(other.gameObject.layer == 6) { //ball 
                GameManager.Instance.AddPoint(goalOwnership);
            }
        }
    }
}
