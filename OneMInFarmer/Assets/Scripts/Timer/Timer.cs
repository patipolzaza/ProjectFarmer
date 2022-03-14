using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance { get; private set; }

    private float maxTime;
    private float currentTimeLeft;

    [SerializeField] private Text timeText;
    [SerializeField] private Image background;
    [SerializeField] private Transform needle;

    [SerializeField] private DayFloatingText dayFloatingText;
    [SerializeField] private Text dayText;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetTime(30);
            Begin();
        }

        if (currentTimeLeft > 0)
        {
            UpdateTimerUI();
        }
    }

    public void Begin()
    {
        currentTimeLeft = maxTime;

        SetDayText(GameManager.instance.currentDay.ToString());
        dayFloatingText.Show();

        StartCoroutine(CountTime());
    }

    public void End()
    {
        GameManager.instance.EndDay();
    }

    public void SetTime(float time)
    {
        maxTime = time;
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
        needle.rotation = Quaternion.Euler(new Vector3(0, 0, currentTimeRotation));

        background.fillAmount = currentTimeLeft > 0 ? (currentTimeLeft / maxTime) : 0;
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
