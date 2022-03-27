using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/MoveSpeedData", fileName = "Move Speed Data")]
public class MoveSpeedStatusData : StatusData
{
    [CustomEditor(typeof(MoveSpeedStatusData))]
    [CanEditMultipleObjects]
    class CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.HelpBox("Extra value per level for move speed status is as multiplier (ex. 1.15 is increase 15% per level, -1.15 is decrease 15% per level).", MessageType.Info);
        }
    }
}
