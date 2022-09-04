using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TheNemesisTest.Runtime.Arena {
    public class TNTArena : MonoBehaviour {
        #region Public Variables
        [Header("Spawn Points")]
        [SerializeField] private Transform playerOneSpawnPoint;
        [SerializeField] private Transform playerTwoSpawnPoint;
        [SerializeField] private Transform ballSpawnPoint;

        [Header("Arena Vertices")]
        [SerializeField] private Transform topLeftEdge;
        [SerializeField] private Transform topRightEdge;
        [SerializeField] private Transform bottomLeftEdge;
        [SerializeField] private Transform bottomRightEdge;
        #endregion

        #region Properties
        public Transform PlayerOneSpawnPoint => playerOneSpawnPoint;
        public Transform PlayerTwoSpawnPoint => playerTwoSpawnPoint;
        public Transform BallSpawnPoint => ballSpawnPoint;
        #endregion

        #region Behaviour Callbacks
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
