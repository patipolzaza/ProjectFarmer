using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Timer : WindowUIBase
{
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
            countTimeCouroutine = null;
        }

        maxTime = timeStatus.GetValue;
        currentTimeLeft = maxTime;

        SetDayText(GameManager.Instance.currentDay.ToString());

        if (dayFloatingText)
        {
            dayFloatingText.Show();
        }
        LightingController.Instance.StartLighting(maxTime);
        countTimeCouroutine = StartCoroutine(CountTime());
    }

    public void End()
    {
        GameManager.Instance.EndDay();
    }

    public void Stop()
    {
        StopAllCoroutines();
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
}
