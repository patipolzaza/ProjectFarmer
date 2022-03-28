using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/Data", fileName = "Status Data")]
public class StatusData : ScriptableObject
{
    public string statusName;
    public int maxLevel = 5;
    public int baseValue = 1;
    public int extraValuePerLevel = 1;
    public int baseUpgradeCost = 10;
    public int extraUpgradeCostPerLevel = 0;
    public float extraCostMultiplierPerLevel = 1;

    [CustomEditor(typeof(StatusData))]
    [CanEditMultipleObjects]
    class CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.HelpBox("Cost will calculate by: \n[ baseUpgradeCost + ((currentLevel - 1) * extraUpgradeCostPerLevel * extraCostMultiplierPerLevel) ]", MessageType.Info);
        }
    }
}
