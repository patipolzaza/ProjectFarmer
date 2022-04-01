using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentDay { get; private set; } = 1;
    public int defaultTimePerDay { get; private set; } = 15;

    public string GetTimeForNextDayString
    {
        get
        {
            string timeString;
            int time = Timer.Instance.maxTime;

            int minute = Mathf.FloorToInt(time / 60);
            int second = time % 60;

            timeString = $"{minute}:{second.ToString("0#")}";
            return timeString;
        }
    }

    public Player player { get; private set; }
    void Start()
    {
        Instance = this;
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
    }

    public void StartDay()
    {
        StatusUpgradeManager.Instance.ClearUpgradeHistory();

        Timer timer = Timer.Instance;
        timer.Begin();
    }

    public void EndDay()
    {
        var dayResultManager = DayResultManager.Instance;
        dayResultManager.StartCalculateResult();

        /*ToNextDay();
        UpgradeShop.Instance.OpenWindow();*/
    }

    private void GrowUpAnimals()
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

        StatusUpgradeManager.Instance.ResetDiaryUpgradeStatus();

        GrowUpAnimals();

        ResetPlotsStatus();
        ResetItemInStacks();
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }
}
