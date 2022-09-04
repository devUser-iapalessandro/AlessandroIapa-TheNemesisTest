using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheNemesisTest.Runtime.Player {
    public class TNTPlayer : MonoBehaviour {

        public void Setup (int teamIndex) {
            Debug.Log("Received Index" + teamIndex);
        }
    }
}
