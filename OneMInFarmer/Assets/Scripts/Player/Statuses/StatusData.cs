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
    public int upgradeCostPerLevel = 10;

    [Tooltip("Extra cost percentage is in form integer 100.")]
    public int extraCostPercentage = 0;

    [CustomEditor(typeof(StatusData))]
    [CanEditMultipleObjects]
    class CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.HelpBox("Cost will calculate by: \n[ upgradeCost + ((level - 1) * upgradeCost * extraCostMultiplier) ]", MessageType.Info);
        }
    }
}
