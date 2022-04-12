using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentDay { get; private set; } = 1;
    public int defaultTimePerDay { get; private set; } = 5;

    public Player player { get; private set; }

    public UnityEvent OnDayStarted;
    public UnityEvent OnDayEnded;

    public UnityEvent OnGameEnded;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    void Update()
    {
        if (!player)
        {
            player = FindObjectOfType<Player>();
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitUntil(() => isCompletedAllSetup());

        StartDay();
    }

    public void StartDay()
    {
        StatusUpgradeManager.Instance.ClearUpgradeHistory();

        OnDayStarted?.Invoke();
    }

    public void EndDay()
    {
        OnDayEnded?.Invoke();
        StartCoroutine(EndDayProcess());
    }

    private IEnumerator EndDayProcess()
    {
        EndDayUI endDayUI = EndDayUI.Instance;
        endDayUI.Show();

        yield return new WaitUntil(() => EndDayUI.Instance.isFinishedAnimation);
        yield return new WaitForSeconds(2);

        endDayUI.Hide();

        var dayResultManager = DayResultManager.Instance;
        dayResultManager.ShowDayResult();
    }

    private void GrowUpAnimals()
    {
        var animals = FindObjectsOfType<Animal>();

        foreach (var animal in animals)
        {
            animal.ResetAnimalHungryStatus();
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

    public void ToNextDay()
    {
        currentDay++;

        ShopForSell.Instance.ResetTotalSoldPrice();
        StatusUpgradeManager.Instance.ResetDailyUpgradeStatus();
        DebtManager.Instance.ResetParameters();
        GrowUpAnimals();

        //ResetPlotsStatus();
        //ResetItemInStacks();

        UpgradeShop.Instance.OpenWindow();
    }

    public void GameOver()
    {
        Debug.Log("Game is O V E R.");
        OnGameEnded?.Invoke();
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    private bool isCompletedAllSetup()
    {
        if (!DayResultManager.Instance || !DayResultManager.Instance.isReadied)
        {
            //Debug.Log("DayResultManager");
            return false;
        }
        if (!UpgradeShop.Instance || !UpgradeShop.Instance.isReadied)
        {
            Debug.Log("UpgradeShop");
            return false;

        }
        if (!Timer.Instance)
        {
            Debug.Log("Timer");
            return false;

        }
        if (!StatusUpgradeManager.Instance || !StatusUpgradeManager.Instance.isReadied)
        {
            Debug.Log("StatusUpgradeManager");
            return false;

        }
        if (!Player.Instance)
        {
            Debug.Log("Player");
            return false;

        }
        return true;
    }
}
