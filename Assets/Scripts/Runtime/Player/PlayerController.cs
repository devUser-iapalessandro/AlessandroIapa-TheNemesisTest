using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheNemesisTest.Runtime.Player {
    [RequireComponent(typeof(PhotonView), typeof(PhotonRigidbodyView), typeof(PhotonTransformView))]
    public class PlayerController : MonoBehaviour {
        #region Public Variables
        [SerializeField] private float movingSpeed;
        [SerializeField] private float acceleration;
        #endregion

        #region Private Variables
        Vector3 _inputDirection;
        Vector3 _movingDirection;
        Vector3 _acceleration;
        Rigidbody _rb;
        #endregion

        #region Behaviour Callbacks

        void Awake () {
            _rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate () {
            if(_inputDirection.sqrMagnitude < 0.05f) {
                _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, Time.fixedDeltaTime * 2);
                return;
            }
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, movingSpeed);
            _movingDirection = _inputDirection * movingSpeed;
            _acceleration = _inputDirection * acceleration;
            _rb.AddForce(_acceleration, ForceMode.Acceleration);
        }


        private void OnGUI () {
            GUILayout.Label(string.Format("Input Direction = {0}, Moving Direction = {1}", _inputDirection, _movingDirection));
            GUILayout.Label(string.Format("Rigidbody Velocity = {0}", _rb.velocity));
        }
        #endregion

        public void OnMove (InputAction.CallbackContext context) {
            Vector2 movingDirection = context.ReadValue<Vector2>();

            _inputDirection = new Vector3(movingDirection.x, 0f, movingDirection.y);
        }
    }
}
