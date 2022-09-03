using System;
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
            var tempName = teamName.ToLower();
            teamID = tempName.GetHashCode();
        }
    }

    public class TeamNameMissingException : Exception {
    }
}
