using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region Custom Inspector
    [CustomEditor(typeof(Timer))]
    [CanEditMultipleObjects]
    class CustomInspector : Editor
    {
        SerializedProperty timeStatusDataProp;
        SerializedProperty timeTextProp;
        SerializedProperty redZoneProp;
        SerializedProperty needleProp;
        SerializedProperty dayFloatingTextProp;
        SerializedProperty dayTextProp;

        private void OnEnable()
        {
            timeStatusDataProp = serializedObject.FindProperty("timeStatusData");
            timeTextProp = serializedObject.FindProperty("timeText");
            redZoneProp = serializedObject.FindProperty("redZone");
            needleProp = serializedObject.FindProperty("needle");
            dayFloatingTextProp = serializedObject.FindProperty("dayFloatingText");
            dayTextProp = serializedObject.FindProperty("dayText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Label("Timer time status data");
            EditorGUILayout.PropertyField(timeStatusDataProp);
            if (GUILayout.Button("Generate data file"))
            {
                OpenSaveFilePanel();
            }

            GUILayout.Space(1.25f);
            EditorGUILayout.PropertyField(redZoneProp);
            EditorGUILayout.PropertyField(needleProp);

            GUILayout.Space(1.25f);
            GUILayout.Label("Texts");
            EditorGUILayout.PropertyField(timeTextProp);
            EditorGUILayout.PropertyField(dayTextProp);
            EditorGUILayout.PropertyField(dayFloatingTextProp);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(2.5f);
            if (GUILayout.Button("Generate Required Childs", GUILayout.Height(25f)))
            {
                CreateRequiredChilds();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void OpenSaveFilePanel()
        {
            string startPath = Application.dataPath;
            string filePath = EditorUtility.SaveFilePanel("Choose path to place file.", startPath, "Data_timeStatus", "asset");

            if (filePath.Length > 0)
            {
                GenerateStatusDataFile(filePath);
            }
        }

        private void GenerateStatusDataFile(string filePath)
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

            StatusData timeStatusData = CreateInstance<StatusData>();
            timeStatusData.Init("time", 4, 60, 10, 10, -25);

            AssetDatabase.CreateAsset(timeStatusData, realPath);
            AssetDatabase.SaveAssets();

            SetMoveSpeedStatusData(timeStatusData);
        }

        private void SetMoveSpeedStatusData(StatusData newData)
        {
            timeStatusDataProp.objectReferenceValue = newData;
        }

        private void CreateDayFloatingTextObject()
        {
            Timer timer = target as Timer;
            GameObject timerObject = timer.gameObject;

            GameObject dayFloatingTextObj = new GameObject();
            dayFloatingTextObj.name = "DayFloatingText";

            dayFloatingTextObj.transform.SetParent(timerObject.transform);
            RectTransform rectTransform = dayFloatingTextObj.AddComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(-790, -90, 0);
            rectTransform.sizeDelta = new Vector2(1080, 200);

            Text text = dayFloatingTextObj.AddComponent<Text>();
            text.text = "Day 1";
            text.resizeTextForBestFit = true;
            text.resizeTextMaxSize = 170;
            text.resizeTextMinSize = 84;
            text.alignment = TextAnchor.MiddleCenter;

            dayFloatingTextObj.AddComponent<DayFloatingText>();

            dayFloatingTextProp.objectReferenceValue = dayFloatingTextObj.GetComponent<DayFloatingText>();
        }

        private void CreateRequiredChilds()
        {
            Timer timer = target as Timer;
            Transform timerTransform = timer.gameObject.transform;

            if (!timeTextProp.objectReferenceValue)
            {
                if (!timerTransform.Find("TimeText"))
                {
                    var timeText = CreateTimeTextObject(timerTransform);
                    timeTextProp.objectReferenceValue = timeText;
                }
                else
                {
                    timeTextProp.objectReferenceValue = timerTransform.Find("TimeText").GetComponent<Text>();
                }
            }

            if (!dayTextProp.objectReferenceValue)
            {
                if (!timerTransform.Find("DayText"))
                {
                    var dayText = CreateDayTextObject(timerTransform);
                    dayTextProp.objectReferenceValue = dayText;
                }
                else
                {
                    dayTextProp.objectReferenceValue = timerTransform.Find("DayText").GetComponent<Text>();
                }
            }

            if (!redZoneProp.objectReferenceValue)
            {
                if (!timerTransform.Find("RedZone"))
                {
                    var redZoneImg = CreateRedZoneObject(timerTransform);
                    redZoneProp.objectReferenceValue = redZoneImg;
                }
                else
                {
                    redZoneProp.objectReferenceValue = timerTransform.Find("RedZone").GetComponent<Image>();
                }
            }

            if (!needleProp.objectReferenceValue)
            {
                if (!timerTransform.Find("Needle"))
                {
                    var needleImg = CreateNeedleObject(timerTransform);
                    needleProp.objectReferenceValue = needleImg;
                }
                else
                {
                    needleProp.objectReferenceValue = timerTransform.Find("Needle").transform;
                }
            }

            if (!dayFloatingTextProp.objectReferenceValue)
            {
                if (!timerTransform.Find("DayFloatingText"))
                {
                    CreateDayFloatingTextObject();
                }
                else
                {
                    dayFloatingTextProp.objectReferenceValue = timerTransform.Find("DayFloatingText").GetComponent<DayFloatingText>();
                }
            }
        }
    }
    #endregion
    public static Timer Instance { get; private set; }

    public int maxTime { get; private set; }
    private float currentTimeLeft;

    public Status timeStatus { get; private set; }
    [SerializeField] private StatusData timeStatusData;

    [SerializeField] private Text timeText;
    [SerializeField] private Image redZone;
    [SerializeField] private Transform needle;

    [SerializeField] private DayFloatingText dayFloatingText;
    [SerializeField] private Text dayText;

    private Coroutine countTimeCouroutine;
    private void Awake()
    {
        timeStatus = new Status("time", timeStatusData);
        Instance = this;
    }

    private void Update()
    {
        if (currentTimeLeft >= 0)
        {
            UpdateTimerUI();
        }
    }

    private void CheckRequireObjects()
    {
        try
        {
            if (!redZone)
            {
                Transform redZoneObj = transform.Find("RedZone");
                if (redZoneObj && redZoneObj.GetComponent<Image>())
                {
                    redZone = redZoneObj.GetComponent<Image>();
                }
            }

            if (!needle)
            {
                Transform foundNeedle = transform.Find("Needle");
                if (foundNeedle)
                {
                    needle = foundNeedle;
                }
            }

            if (!timeText)
            {
                Transform timeTextObj = transform.Find("TimeText");
                if (timeTextObj && timeTextObj.GetComponent<Text>())
                {
                    timeText = timeTextObj.GetComponent<Text>();
                }
            }

            if (!dayText)
            {
                Transform dayTextObj = transform.Find("DayText");
                if (dayTextObj && dayTextObj.GetComponent<Text>())
                {
                    dayText = dayTextObj.GetComponent<Text>();
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void Begin()
    {
        if (countTimeCouroutine != null)
        {
            StopCoroutine(countTimeCouroutine);
        }

        maxTime = timeStatus.GetValue;
        currentTimeLeft = maxTime;

        SetDayText(GameManager.Instance.currentDay.ToString());

        if (dayFloatingText)
        {
            dayFloatingText.Show();
        }

        countTimeCouroutine = StartCoroutine(CountTime());
    }

    public void End()
    {
        GameManager.Instance.EndDay();
    }

    private void UpdateTimerUI()
    {
        UpdateText();
        UpdateClockNeedle();
    }

    private void UpdateText()
    {
        string timeString = "";
        float roundedTime = Mathf.RoundToInt(currentTimeLeft);

        if (currentTimeLeft >= 60)
        {
            int minute = Mathf.FloorToInt(roundedTime / 60);
            float second = roundedTime % 60;

            timeString = $"{minute}:{second.ToString("0#")}";
        }
        else
        {
            timeString = roundedTime.ToString();
        }

        timeText.text = timeString;
    }

    private void UpdateClockNeedle()
    {
        float currentTimeRotation = (currentTimeLeft / maxTime) * 360;

        if (float.IsNaN(currentTimeRotation))
        {
            return;
        }

        needle.rotation = Quaternion.Euler(new Vector3(0, 0, currentTimeRotation));

        redZone.fillAmount = 1 - (currentTimeLeft > 0 ? (currentTimeLeft / maxTime) : 0);
    }

    private IEnumerator CountTime()
    {
        while (currentTimeLeft > 0)
        {
            yield return new WaitForFixedUpdate();
            currentTimeLeft -= Time.deltaTime;
        }

        currentTimeLeft = 0;
        End();
    }

    public void SetDayText(string text)
    {
        string message = "Day " + text;

        dayText.text = message;
        dayFloatingText.SetText(message);
    }

    #region Create Timer Components
    private static Image CreateRedZoneObject(Transform parent)
    {
        GameObject redZoneObj = new GameObject();
        redZoneObj.name = "RedZone";
        redZoneObj.transform.SetParent(parent);

        Image img = redZoneObj.AddComponent<Image>();
        img.color = Color.red;
        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Radial360;
        img.fillOrigin = (int)Image.Origin360.Top;
        img.fillClockwise = true;

        return img;
    }

    private static Transform CreateNeedleObject(Transform parent)
    {
        GameObject needleObj = new GameObject();
        needleObj.name = "Needle";
        needleObj.transform.SetParent(parent);

        Image img = needleObj.AddComponent<Image>();

        return needleObj.transform;
    }

    private static Text CreateTimeTextObject(Transform parent)
    {
        GameObject timeTextObj = new GameObject();
        timeTextObj.name = "TimeText";
        timeTextObj.transform.SetParent(parent);

        Text txt = timeTextObj.AddComponent<Text>();

        return txt;
    }

    private static Text CreateDayTextObject(Transform parent)
    {
        GameObject dayTextObj = new GameObject();
        dayTextObj.name = "DayText";
        dayTextObj.transform.SetParent(parent);

        Text txt = dayTextObj.AddComponent<Text>();

        return txt;
    }
    #endregion
}
