using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentDay { get; private set; } = 1;
    public float defaultTimePerDay { get; private set; } = 15;
    private float timeForNextDay;

    public string GetTimeForNextDayString
    {
        get
        {
            string timeString;
            int roundedTime = Mathf.RoundToInt(timeForNextDay);

            int minute = Mathf.FloorToInt(roundedTime / 60);
            int second = roundedTime % 60;

            timeString = $"{minute}:{second.ToString("0#")}";
            return timeString;
        }
    }

    public Player player { get; private set; }
    void Start()
    {
        instance = this;
        timeForNextDay = defaultTimePerDay;
    }

    void Update()
    {
        if (!player)
        {
            player = FindObjectOfType<Player>();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartDay();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            IncreaseTimeForNextDay(5);
        }
    }

    public void StartDay()
    {
        Timer timer = Timer.instance;
        timer.SetTime(timeForNextDay);
        timer.Begin();
    }

    public void EndDay()
    {
        ToNextDay();
        UpgradeShop.instance.OpenWindow();
    }

    public void IncreaseTimeForNextDay(float time)
    {
        timeForNextDay += time;
    }

    private void ResetAnimalsStatus()
    {
        var animals = FindObjectsOfType<Animal>();

        foreach (var animal in animals)
        {
            animal.ResetAnimalStatus();
        }
    }
    private void ResetPlotsStatus()
    {
        var plots = FindObjectsOfType<Plot>();
        foreach (Plot plot in plots)
        {
            plot.ResetPlotStatus();
        }
    }
    private void ResetItemInStacks()
    {
        var shopBuyManager = FindObjectOfType<ShopBuyManager>();
        shopBuyManager.AddItemToShop();
    }

    private void ToNextDay()
    {
        currentDay++;
        timeForNextDay = defaultTimePerDay;
        ResetAnimalsStatus();
        ResetPlotsStatus();
        ResetItemInStacks();
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }
}
