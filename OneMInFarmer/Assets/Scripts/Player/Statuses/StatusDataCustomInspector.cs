using UnityEditor;

[CustomEditor(typeof(StatusData))]
[CanEditMultipleObjects]
class StatusDataCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.HelpBox("Cost will calculate by: \n[ upgradeCost + ((level - 1) * upgradeCost * extraCostMultiplier) ]", MessageType.Info);
    }
}
