using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayResultManager : MonoBehaviour
{
    public static DayResultManager Instance;

    private ShopForSell shop;
    [SerializeField] private DayResultUI ui;
    public bool isReadied { get; private set; } = false;

    private int totalSoldPrice;
    private int debt;
    private int profit;

    public bool isAllProcessFinished { get; private set; }
    private bool updateTotalPriceTextFinished;
    private bool updateDebtTextFinished;
    private bool updateProfitTextFinished;

    private void Awake()
    {
        StartCoroutine(InitialSetup());
    }

    public void StartCalculateResult()
    {
        isAllProcessFinished = false;

        OpenWindow();

        StartCoroutine(DayResultProcess());
    }

    private IEnumerator InitialSetup()
    {
        yield return new WaitUntil(() => ShopForSell.Instance != null);
        shop = ShopForSell.Instance;
        Instance = this;
        isReadied = true;
    }

    private IEnumerator DayResultProcess()
    {
        ui.SetDayText(GameManager.Instance.currentDay.ToString());
        shop.SellAllItemsInContainer();
        yield return new WaitForSeconds(1.25f);
        yield return new WaitUntil(() => ShopForSell.Instance.isFinishSellingProcess);

        totalSoldPrice = shop.totalSoldPrice;
        StartCoroutine(UpdateTotalSoldPriceText());
        yield return new WaitUntil(() => updateTotalPriceTextFinished);
        yield return new WaitForSeconds(0.5f);

        if (DebtManager.Instance.dayForNextDebtPayment == GameManager.Instance.currentDay)
        {
            debt = DebtManager.Instance.GetDebt;
            StartCoroutine(UpdateDebtText());
            yield return new WaitUntil(() => updateDebtTextFinished);
            yield return new WaitForSeconds(0.5f);
        }

        profit = totalSoldPrice - debt;
        StartCoroutine(UpdateProfitText());
        yield return new WaitUntil(() => updateProfitTextFinished);

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
        updateTotalPriceTextFinished = false;

        float currentValue = 0;
        int target = totalSoldPrice;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, 0.15f);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();
            ui.SetTotalSoldPriceText(currentValueText);
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);

        updateTotalPriceTextFinished = true;
    }

    private IEnumerator UpdateProfitText()
    {
        updateProfitTextFinished = false;

        int target = profit;
        float currentValue = 0;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, 0.15f);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();
            ui.SetNetProfitText(currentValueText);
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);

        updateProfitTextFinished = true;
    }

    private IEnumerator UpdateDebtText()
    {
        updateDebtTextFinished = false;

        int target = debt;
        float currentValue = 0;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, 0.15f);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();
            ui.SetDeptText($"<color=red>-{currentValueText}</color>");
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);
        updateDebtTextFinished = true;
    }

    public void Reset()
    {
        totalSoldPrice = 0;
        debt = 0;
        profit = 0;

        updateTotalPriceTextFinished = false;
        updateDebtTextFinished = false;
        updateProfitTextFinished = false;
    }
}
