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

    public bool isFinishedAllProcess { get; private set; }
    private bool finishedUpdateTotalPriceText;

    private bool _skipMode;
    private bool _isInProcess;

    private void Awake()
    {
        StartCoroutine(InitialSetup());
    }

    private void Update()
    {
        if (!isFinishedAllProcess)
        {
            ui.SetActiveContinueText(false);

            if (_isInProcess && Input.anyKeyDown && _skipMode == false)
            {
                ShowSkippedDayResult();
                _skipMode = true;
            }

            return;
        }

        ui.SetActiveContinueText(true);

        if (Input.anyKeyDown)
        {
            if (DebtManager.Instance.GetDayRemainForDebtPayment < 1)
            {
                DebtManager.Instance.ShowResultUI();
            }
            else
            {
                GameManager.Instance.ToNextDay();
            }

            isFinishedAllProcess = false;
            CloseWindow();
            ResetParameters();
        }
    }

    public void ShowDayResult()
    {
        isFinishedAllProcess = false;

        OpenWindow();

        _isInProcess = true;
        StartCoroutine(DayResultProcess());
    }

    private IEnumerator InitialSetup()
    {
        Instance = this;
        yield return new WaitUntil(() => ShopForSell.Instance);
        shop = ShopForSell.Instance;
        yield return new WaitUntil(() => DebtManager.Instance);
        isReadied = true;
    }

    public void OpenWindow()
    {
        ui.ShowWindow();
    }

    public void CloseWindow()
    {
        ui.HideWindow();
    }

    private IEnumerator DayResultProcess()
    {
        totalSoldPrice = shop.totalSoldPrice;
        var dayRemainForNextDebtPayment = DebtManager.Instance.GetDayRemainForDebtPayment;

        ui.SetDayText(GameManager.Instance.currentDay.ToString());
        ui.SetDeptText(DebtManager.Instance.GetDebt.ToString());
        ui.SetDayRemainingForNextDebtPaymentText(dayRemainForNextDebtPayment.ToString());

        yield return new WaitForSeconds(1.25f);

        StartCoroutine(UpdateTotalSoldPriceText());
        yield return new WaitUntil(() => finishedUpdateTotalPriceText);

        yield return new WaitForSeconds(0.75f);

        ui.SetDayRemainingForNextDebtPaymentText(dayRemainForNextDebtPayment.ToString());

        yield return new WaitForSeconds(1.25f);

        isFinishedAllProcess = true;
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
            yield return new WaitForSeconds(0.01f);
        } while (Mathf.RoundToInt(currentValue) != target);

        finishedUpdateTotalPriceText = true;
    }

    private void ShowSkippedDayResult()
    {
        StopAllCoroutines();

        totalSoldPrice = shop.totalSoldPrice;
        ui.SetDayText(GameManager.Instance.currentDay.ToString());
        ui.SetDeptText(DebtManager.Instance.GetDebt.ToString());
        ui.SetDayRemainingForNextDebtPaymentText(DebtManager.Instance.GetDayRemainForDebtPayment.ToString());

        isFinishedAllProcess = true;
    }

    public void ResetParameters()
    {
        totalSoldPrice = 0;
        debt = 0;

        finishedUpdateTotalPriceText = false;
        isFinishedAllProcess = false;
        _isInProcess = false;
        _skipMode = false;
    }
}
