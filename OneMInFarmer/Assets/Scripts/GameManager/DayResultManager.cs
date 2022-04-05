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
    public int dayRemainForNextDebtPaymentInMem { get; private set; }

    public bool isFinishedAllProcess { get; private set; }
    private bool finishedUpdateTotalPriceText;

    private bool skipMode;
    private bool isInProcess;

    private void Awake()
    {
        StartCoroutine(InitialSetup());
    }

    private void Update()
    {
        if (!isFinishedAllProcess)
        {
            ui.SetActiveContinueText(false);

            if (isInProcess && Input.anyKeyDown && skipMode == false)
            {
                ShowSkippedDayResult();
                skipMode = true;
            }

            return;
        }

        ui.SetActiveContinueText(true);

        if (Input.anyKeyDown)
        {
            isFinishedAllProcess = false;
            CloseWindow();
            ResetParameters();

            if (dayRemainForNextDebtPaymentInMem < 1)
            {
                //TODO: Do debt payment process
            }
            else
            {
                GameManager.Instance.ToNextDay();
            }
        }
    }

    public void ShowDayResult()
    {
        isFinishedAllProcess = false;

        OpenWindow();

        isInProcess = true;
        StartCoroutine(DayResultProcess());
    }

    private IEnumerator InitialSetup()
    {
        Instance = this;
        yield return new WaitUntil(() => ShopForSell.Instance != null);
        shop = ShopForSell.Instance;
        yield return new WaitUntil(() => GameManager.Instance);
        dayRemainForNextDebtPaymentInMem = GameManager.Instance.DebtManager.dayForNextDebtPayment;
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
        var dayRemainForNextDebtPayment = GameManager.Instance.GetDayRemainForDebtPayment;

        ui.SetDayText(GameManager.Instance.currentDay.ToString());
        ui.SetDeptText(GameManager.Instance.DebtManager.GetDebt.ToString());
        ui.SetDayRemainingForNextDebtPaymentText(dayRemainForNextDebtPaymentInMem.ToString());

        yield return new WaitForSeconds(1.25f);

        StartCoroutine(UpdateTotalSoldPriceText());
        yield return new WaitUntil(() => finishedUpdateTotalPriceText);

        yield return new WaitForSeconds(0.75f);

        ui.SetDayRemainingForNextDebtPaymentText(dayRemainForNextDebtPayment.ToString());
        dayRemainForNextDebtPaymentInMem = dayRemainForNextDebtPayment;

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
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);

        finishedUpdateTotalPriceText = true;
    }

    private void ShowSkippedDayResult()
    {
        StopAllCoroutines();

        totalSoldPrice = shop.totalSoldPrice;
        dayRemainForNextDebtPaymentInMem = GameManager.Instance.GetDayRemainForDebtPayment;
        ui.SetDayText(GameManager.Instance.currentDay.ToString());
        ui.SetDeptText(GameManager.Instance.DebtManager.GetDebt.ToString());
        ui.SetDayRemainingForNextDebtPaymentText(dayRemainForNextDebtPaymentInMem.ToString());

        isFinishedAllProcess = true;
    }

    public void ResetParameters()
    {
        totalSoldPrice = 0;
        debt = 0;

        finishedUpdateTotalPriceText = false;
        isFinishedAllProcess = false;
        isInProcess = false;
        skipMode = false;
    }
}
