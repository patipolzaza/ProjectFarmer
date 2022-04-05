using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ScoreManager ScoreManager { get; private set; }
    public DebtManager DebtManager { get; private set; }

    public int currentDay { get; private set; } = 1;
    public int defaultTimePerDay { get; private set; } = 5;

    public Player player { get; private set; }
    void Awake()
    {
        Instance = this;

        ScoreManager = new ScoreManager();
        DebtManager = new DebtManager();
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

        Timer timer = Timer.Instance;
        timer.Begin();

        player.EnableMove();
        WalletUI.Instance.ShowWindow();
    }

    public void EndDay()
    {
        StartCoroutine(EndDayProcess());
    }

    private IEnumerator EndDayProcess()
    {
        player.DisableMove();
        WalletUI.Instance.HideWindow();

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

    public void ToNextDay()
    {
        currentDay++;

        ShopForSell.Instance.ResetTotalSoldPrice();
        StatusUpgradeManager.Instance.ResetDiaryUpgradeStatus();

        GrowUpAnimals();

        //ResetPlotsStatus();
        //ResetItemInStacks();

        UpgradeShop.Instance.OpenWindow();
    }

    public void GameOver()
    {
        Debug.Log("Game is O V E R.");
    }

    public void CalculateScore()
    {
        if (DebtManager.dayForNextDebtPayment == currentDay)
        {
            int playerCoin = player.wallet.coin;
            int debt = DebtManager.GetDebt;
            Debug.Log(playerCoin);
            Debug.Log(debt);
            if (playerCoin < debt)
            {
                GameOver();
                return;
            }

            int score = DebtManager.PayDebt(debt);
            player.wallet.LoseCoin(debt);

            ScoreManager.AddScore(score);
        }

        ToNextDay();
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    private bool isCompletedAllSetup()
    {
        if (!DayResultManager.Instance || !DayResultManager.Instance.isReadied)
            return false;
        if (!UpgradeShop.Instance || !UpgradeShop.Instance.isReadied)
            return false;
        if (!Timer.Instance)
            return false;
        if (!StatusUpgradeManager.Instance || !StatusUpgradeManager.Instance.isReadied)
            return false;
        if (!Player.Instance)
            return false;
        if (!WalletUI.Instance)
            return false;
        return true;
    }

    public int GetDayRemainForDebtPayment
    {
        get
        {
            return DebtManager.dayForNextDebtPayment - currentDay;
        }
    }
}
