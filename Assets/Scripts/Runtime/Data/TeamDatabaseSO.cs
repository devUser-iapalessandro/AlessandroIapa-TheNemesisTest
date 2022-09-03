using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheNemesisTest.Runtime.Data {
    [CreateAssetMenu(menuName = "The Nemesis Test/Data/New Team Database", fileName = "New Team Database")]
    public class TeamDatabaseSO : ScriptableObject {
        #region Public Variables
        public List<TeamDataSO> teams = new List<TeamDataSO>();
        #endregion

        public void GenerateTeamIDs () {
            if(teams.Count == 0) {
                Debug.LogError("No teams in the database. Aborting...");
                return;
            }

            for(int i = 0; i < teams.Count; i++) {

                teams[i]?.GenerateTeamID();
                try {
                    teams[i].GenerateTeamID();
                    Debug.LogFormat("Generating Team ID for element at index {0}, team name {1}, id {2}", i, teams[i].teamName, teams[i].teamID);
                }
                catch(NullReferenceException) {
                    Debug.LogErrorFormat("Element at index {0} is null, impossible to generate ID for null element at index {0}.", i);
                }
                catch(TeamNameMissingException) {
                    Debug.LogErrorFormat("Element at index {0} has empty team name parameter, impossible to generate ID for element at index {0} with empty team name parameter.");
                }
            }
        }
    }
}
