using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/PercentStatusData")]
public class PercentStatusData : StatusData
{
    public PercentStatusData() { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="extraValuePerLevel">The percentage that will increase from base value per level.</param>
    public override void Init(string statusName, int maxLevel, int baseValue, int extraValuePerLevel, int upgradeCostPerLevel, int extraCostPercentage)
    {
        base.Init(statusName, maxLevel, baseValue, extraValuePerLevel, upgradeCostPerLevel, extraCostPercentage);
    }

    [CustomEditor(typeof(PercentStatusData))]
    [CanEditMultipleObjects]
    class CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.HelpBox("Cost will calculate by: \n[ upgradeCost + ((level - 1) * upgradeCost * extraCostMultiplier) ]", MessageType.Info);
            EditorGUILayout.HelpBox("Extra value per level for percent status is as extra percentage form (ex. 30 is equal to +30% from start value.).", MessageType.Info);
        }
    }
}