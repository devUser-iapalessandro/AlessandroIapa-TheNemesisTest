using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TheNemesisTest.Runtime.Arena {
    public class TNTArena : MonoBehaviour {
        #region Public Variables

        [SerializeField] private Transform playerOneSpawnPoint;
        [SerializeField] private Transform playerTwoSpawnPoint;

        [SerializeField] private Transform topLeftEdge;
        [SerializeField] private Transform topRightEdge;
        [SerializeField] private Transform bottomLeftEdge;
        [SerializeField] private Transform bottomRightEdge;
        #endregion

        #region Private Variables
        private static TNTArena instance;
        #endregion

        #region Properties
        public static TNTArena Instance => instance;

        public Transform PlayerOneSpawnPoint => playerOneSpawnPoint;
        public Transform PlayerTwoSpawnPoint => playerTwoSpawnPoint;
        #endregion

        #region Behaviour Callbacks
        void Awake () {
            if(instance == null) {
                instance = this;
            }
        }

        void OnDrawGizmos () {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(topLeftEdge.position, .5f);
            Gizmos.DrawSphere(topRightEdge.position, .5f);
            Gizmos.DrawSphere(bottomLeftEdge.position, .5f);
            Gizmos.DrawSphere(bottomRightEdge.position, .5f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(playerOneSpawnPoint.position, .3f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(playerTwoSpawnPoint.position, .3f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(topLeftEdge.position, topRightEdge.position);
            Gizmos.DrawLine(topRightEdge.position, bottomRightEdge.position);
            Gizmos.DrawLine(bottomRightEdge.position, bottomLeftEdge.position);
            Gizmos.DrawLine(bottomLeftEdge.position, topLeftEdge.position);
        }
        #endregion

        #region Public Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 GetRandomPositionInsideEdges () {
            var randBetweenTops = Vector3.Lerp(topLeftEdge.position, topRightEdge.position, Random.Range(0f, 1f));
            var randBetweenBottoms = Vector3.Lerp(bottomLeftEdge.position, bottomRightEdge.position, Random.Range(0f, 1f));
            randBetweenTops.y = 0;
            randBetweenBottoms.y = 0;
            return Vector3.Lerp(randBetweenTops, randBetweenBottoms, Random.Range(0f, 1f));
        }
        #endregion
    }
}
