using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int dayPlayed { get; private set; }
    public float defaultTimePerDay { get; private set; } = 15;
    public float timeForNextDay { get; private set; }
    public float currentTimeLeft { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        timeForNextDay = defaultTimePerDay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRound();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            IncreaseTimeForNextDay(5);
        }
    }

    public void StartRound()
    {
        currentTimeLeft = timeForNextDay;

        //StartCoroutine(CountTime());
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
        dayPlayed++;
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
