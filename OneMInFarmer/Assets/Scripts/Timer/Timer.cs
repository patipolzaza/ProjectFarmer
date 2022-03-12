using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float maxTime;
    private float currentTimeLeft;

    [SerializeField] private Text timeText;
    [SerializeField] private Image background;
    [SerializeField] private Transform needle;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetTime(90);
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
        StartCoroutine(CountTime());
    }

    public void End()
    {

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
    }
}
