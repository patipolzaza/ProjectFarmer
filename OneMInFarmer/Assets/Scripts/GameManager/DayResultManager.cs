using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayResultManager : MonoBehaviour
{
    public static DayResultManager Instance;

    private ShopForSell shop;
    [SerializeField] private DayResultUI ui;
    public bool isReadied { get; private set; } = false;

    public int totalSoldPrice { get; private set; }
    public int debt { get; private set; }
    public int profit { get; private set; }
    private int playerCoinBeforeCal;
    private int playerCoinAfterCal;

    public bool isAllProcessFinished { get; private set; }
    private bool finishedUpdateTotalPriceText;
    private bool finishedUpdateDebtText;
    private bool finishedUpdateProfitText;
    private bool finishedUpdatePlayerCoin;

    private void Awake()
    {
        StartCoroutine(InitialSetup());
    }

    private void Update()
    {
        if (!isAllProcessFinished)
        {
            ui.SetActiveContinueText(false);
            return;
        }

        ui.SetActiveContinueText(true);

        if (Input.anyKeyDown)
        {
            GameManager.Instance.CalculateScore();
        }
    }

    public void StartCalculateResult()
    {
        isAllProcessFinished = false;

        OpenWindow();

        StartCoroutine(DayResultProcess());
    }

    private IEnumerator InitialSetup()
    {
        Instance = this;
        yield return new WaitUntil(() => ShopForSell.Instance != null);
        shop = ShopForSell.Instance;
        isReadied = true;
    }

    private IEnumerator DayResultProcess()
    {
        Wallet playerWallet = Player.Instance.wallet;

        ui.SetDayText(GameManager.Instance.currentDay.ToString());
        playerCoinBeforeCal = playerWallet.coin;
        ui.SetPlayerCoinText(playerCoinBeforeCal.ToString());
        shop.SellAllItemsInContainer();
        yield return new WaitForSeconds(1.25f);
        yield return new WaitUntil(() => ShopForSell.Instance.isFinishSellingProcess);

        totalSoldPrice = shop.totalSoldPrice;
        StartCoroutine(UpdateTotalSoldPriceText());
        yield return new WaitUntil(() => finishedUpdateTotalPriceText);
        yield return new WaitForSeconds(0.75f);

        if (GameManager.Instance.DebtManager.dayForNextDebtPayment == GameManager.Instance.currentDay)
        {
            debt = GameManager.Instance.DebtManager.GetDebt;
            StartCoroutine(UpdateDebtText());
            yield return new WaitUntil(() => finishedUpdateDebtText);
            yield return new WaitForSeconds(0.75f);
        }

        profit = totalSoldPrice - debt;
        StartCoroutine(UpdateProfitText());
        yield return new WaitUntil(() => finishedUpdateProfitText);
        yield return new WaitForSeconds(0.75f);

        playerWallet.EarnCoin(profit);
        playerCoinAfterCal = playerWallet.coin;

        StartCoroutine(UpdatePlayerCoinText());
        yield return new WaitUntil(() => finishedUpdatePlayerCoin);

        yield return new WaitForSeconds(1.25f);
        isAllProcessFinished = true;
    }

    public void OpenWindow()
    {
        ui.ShowWindow();
    }

    public void CloseWindow()
    {
        ui.HideWindow();
    }

    private IEnumerator UpdateTotalSoldPriceText()
    {
        finishedUpdateTotalPriceText = false;

        float currentValue = 0;
        int target = totalSoldPrice;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, 0.15f);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();
            ui.SetTotalSoldPriceText(currentValueText);
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);

        finishedUpdateTotalPriceText = true;
    }
    private IEnumerator UpdatePlayerCoinText()
    {
        finishedUpdatePlayerCoin = false;

        float currentValue = playerCoinBeforeCal;
        int target = playerCoinAfterCal;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, 0.15f);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();

            if (currentValue < 0)
            {
                currentValueText = $"<color=red>{currentValueText}</color>";
            }

            ui.SetPlayerCoinText(currentValueText);
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);

        finishedUpdatePlayerCoin = true;
    }

    private IEnumerator UpdateProfitText()
    {
        finishedUpdateProfitText = false;

        int target = profit;
        float currentValue = 0;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, 0.15f);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();

            if (currentValue < 0)
            {
                currentValueText = $"<color=red>{currentValueText}</color>";
            }

            ui.SetNetProfitText(currentValueText);
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);

        finishedUpdateProfitText = true;
    }

    private IEnumerator UpdateDebtText()
    {
        finishedUpdateDebtText = false;

        int target = debt;
        float currentValue = 0;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, 0.15f);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();
            ui.SetDeptText($"<color=red>-{currentValueText}</color>");
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);
        finishedUpdateDebtText = true;
    }

    public void Reset()
    {
        totalSoldPrice = 0;
        debt = 0;
        profit = 0;

        playerCoinBeforeCal = 0;
        playerCoinAfterCal = 0;

        finishedUpdateTotalPriceText = false;
        finishedUpdateDebtText = false;
        finishedUpdateProfitText = false;
    }
}
