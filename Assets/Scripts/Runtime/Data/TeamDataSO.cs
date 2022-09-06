using System;
using UnityEditor;
using UnityEngine;

namespace TheNemesisTest.Runtime.Data {

    [CreateAssetMenu(menuName = "The Nemesis Test/Data/New Team", fileName = "New Team Data")]
    public class TeamDataSO : ScriptableObject {
        public int teamID;
        public string teamName;
        public Material inGameSkin;
        public Material goalSkin;

        public void GenerateTeamID () {
            if(string.IsNullOrEmpty(teamName)) {
                throw new TeamNameMissingException();
            }
            if(inGameSkin == null) {
                throw new TeamSkinMissingException();
            }if(goalSkin == null) {
                throw new TeamGoalSkinMissingException();
            }
            var tempName = teamName.ToLower();
            teamID = tempName.GetHashCode();
            EditorUtility.SetDirty(this);
        }
    }

    public class TeamNameMissingException : Exception {
    }
    public class TeamSkinMissingException : Exception {
    }
    public class TeamGoalSkinMissingException : Exception {
    }
}
