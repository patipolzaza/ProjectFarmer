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

            EditorGUILayout.HelpBox("Cost will calculate by: \n[ startUpgradeCost + ((currentLevel - 1) * upgradeCostPerLevel * extraCostMultiplierPerLevel) ]", MessageType.Info);
            EditorGUILayout.HelpBox("Extra value per level for move speed status is as extra percentage form (ex. 30 is equal to +30% from start value.).", MessageType.Info);
        }
    }
}
