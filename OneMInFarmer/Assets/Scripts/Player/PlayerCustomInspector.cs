using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
[CanEditMultipleObjects]
public class PlayerCustomInspector : Editor
{
    SerializedProperty interactableDetectorProp;
    SerializedProperty interactableDetectorRangeProp;
    SerializedProperty characterObjectProp;

    SerializedProperty moveSpeedDataProp;

    SerializedProperty OnInteractEventProp;
    SerializedProperty OnPickingEventProp;
    SerializedProperty OnDropingEventProp;
    SerializedProperty OnRefillingEventProp;

    private void OnEnable()
    {
        interactableDetectorProp = serializedObject.FindProperty("interactableDetector");
        interactableDetectorRangeProp = serializedObject.FindProperty("interactableDetectRange");
        characterObjectProp = serializedObject.FindProperty("characterObject");

        moveSpeedDataProp = serializedObject.FindProperty("moveSpeedData");

        OnInteractEventProp = serializedObject.FindProperty("OnInteractEvent");
        OnPickingEventProp = serializedObject.FindProperty("OnPickingEvent");
        OnDropingEventProp = serializedObject.FindProperty("OnDropingEvent");
        OnRefillingEventProp = serializedObject.FindProperty("OnRefillingEvent");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Label("Character Object.");
        EditorGUILayout.PropertyField(characterObjectProp);
        GUILayout.Space(2f);

        GUILayout.Label("Status Data.");
        EditorGUILayout.PropertyField(moveSpeedDataProp);
        if (GUILayout.Button("Generate data file"))
        {
            OpenSaveFilePanel();
        }
        GUILayout.Space(1.25f);
        GUILayout.Label("Interactable Detection.");
        EditorGUILayout.PropertyField(interactableDetectorProp);
        EditorGUILayout.PropertyField(interactableDetectorRangeProp);
        GUILayout.Space(1.25f);

        GUILayout.Label("Unity Envent.");
        EditorGUILayout.PropertyField(OnInteractEventProp);
        EditorGUILayout.PropertyField(OnPickingEventProp);
        EditorGUILayout.PropertyField(OnDropingEventProp);
        EditorGUILayout.PropertyField(OnRefillingEventProp);

        serializedObject.ApplyModifiedProperties();
    }

    private void OpenSaveFilePanel()
    {
        string startPath = Application.dataPath;
        string filePath = EditorUtility.SaveFilePanel("Choose path to place file.", startPath, "Data_MoveSpeedStatus", "asset");

        if (filePath.Length > 0)
        {
            GenerateMoveSpeedData(filePath);
        }
    }

    private void GenerateMoveSpeedData(string filePath)
    {
        string[] paths = filePath.Split('/');

        string realPath = "";
        bool isFoundAssetsAtPath = false;
        int index = 0;
        string appendingPath;
        foreach (var splittedPath in paths)
        {
            if (splittedPath.Equals("Assets"))
            {
                isFoundAssetsAtPath = true;
            }

            if (isFoundAssetsAtPath)
            {
                appendingPath = splittedPath;

                realPath += appendingPath;

                if (index < paths.Length - 1)
                {
                    realPath += "/";
                }
            }

            index++;
        }

        PercentStatusData moveSpeedData = CreateInstance<PercentStatusData>();
        moveSpeedData.Init("Move Speed", 4, 250, 30, 10, 5);

        AssetDatabase.CreateAsset(moveSpeedData, realPath);
        AssetDatabase.SaveAssets();

        SetMoveSpeedStatusData(moveSpeedData);
    }

    private void SetMoveSpeedStatusData(PercentStatusData newData)
    {
        moveSpeedDataProp.objectReferenceValue = newData;
    }
}
