using TheNemesisTest.Runtime.Data;
using UnityEditor;
using UnityEngine;

namespace TheNemesisTest.Editor.Data {
    [CustomEditor(typeof(TeamDatabaseSO))]
    public class TeamDatabaseCE : UnityEditor.Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update();
            base.OnInspectorGUI();
            var database = (TeamDatabaseSO)target;

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                if(GUILayout.Button("Generate team IDs")) {
                    database.GenerateTeamIDs();
                }
            }GUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
