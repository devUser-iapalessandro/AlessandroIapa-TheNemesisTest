using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TheNemesisTest.Runtime.NetworkSystems;
using UnityEngine;

namespace TheNemesisTest.Runtime.Player {
    public class TNTPlayer : MonoBehaviour {
        [SerializeField] private MeshRenderer _playerRenderer;
        [SerializeField] private PlayerController _controller;

        private PhotonView _pv;

        #region Behaviour Callbacks
        void Awake () {
            _pv = GetComponent<PhotonView>();
            if(_pv.InstantiationData != null) {
                int index = (int)_pv.InstantiationData[0];
                _playerRenderer.material = GameManager.Instance.TeamDatabase.teams[index].inGameSkin;
            }

            _controller.enabled = _pv.IsMine;
        }

        void OnValidate () {
            _controller = GetComponent<PlayerController>();
        }
        #endregion
    }
}
