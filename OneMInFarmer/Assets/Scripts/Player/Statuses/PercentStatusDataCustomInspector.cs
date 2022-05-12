using UnityEditor;

[CustomEditor(typeof(PercentStatusData))]
[CanEditMultipleObjects]
class PercentStatusDataCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.HelpBox("Cost will calculate by: \n[ upgradeCost + ((level - 1) * upgradeCost * extraCostMultiplier) ]", MessageType.Info);
        EditorGUILayout.HelpBox("Extra value per level for percent status is as extra percentage form (ex. 30 is equal to +30% from start value.).", MessageType.Info);
    }
}
