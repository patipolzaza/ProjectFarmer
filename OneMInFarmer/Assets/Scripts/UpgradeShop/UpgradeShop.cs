using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    public static UpgradeShop instance;
    private UpgradeShopWindowUI ui;
    [SerializeField] private int baseTimeCostPerSecond = 1;

    private int currentExtraTime;
    [SerializeField] private int minExtraTime = 0;
    [SerializeField] private int maxExtraTime = 30;

    private void Awake()
    {
        instance = this;
        ui = FindObjectOfType<UpgradeShopWindowUI>();
    }

    private void Start()
    {
        CloseWindow();
    }

    private void Update()
    {
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (ui.gameObject.activeSelf)
            {
                CloseWindow();
            }
            else
            {
                OpenWindow();
            }
        }
    }

    public void OpenWindow()
    {
        ui.ShowWindow();
        ui.SetTimeUpgradeButtonsInteractable(true);
        GameManager.instance.SetTimeScale(0);
    }

    public void CloseWindow()
    {
        ui.HideWindow();
        GameManager.instance.SetTimeScale(1);
    }

    private void UpdateUI()
    {
        ui.UpdatePlayerCoinText(GameManager.instance.player.wallet.coin);
        ui.UpdateCurrentExtraTimeText(currentExtraTime);
        ui.UpdateCurrentCostText(GetPrice(currentExtraTime));
        ui.UpdateTimeForNextDayText(GameManager.instance.GetTimeForNextDayString);
    }

    private bool UpgradeTime(int extraTime)
    {
        var playerWallet = GameManager.instance.player.wallet;

        if (playerWallet.coin >= GetPrice(extraTime))
        {
            int price = GetPrice(extraTime);
            GameManager.instance.IncreaseTimeForNextDay(extraTime);

            playerWallet.LoseCoin(price);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void BuyExtraTime()
    {
        if (UpgradeTime(currentExtraTime))
        {
            ui.SetTimeUpgradeButtonsInteractable(false);
        }
    }

    public void IncreaseExtraTime()
    {
        int extraTime = ++currentExtraTime;
        SetExtraTime(extraTime);
    }

    public void DecreaseExtraTime()
    {
        int extraTime = --currentExtraTime;
        SetExtraTime(extraTime);
    }

    public void SetExtraTime(int extraTime)
    {
        int playerCoin = GameManager.instance.player.wallet.coin;
        currentExtraTime = Mathf.Clamp(extraTime, minExtraTime, maxExtraTime);

        if (extraTime <= minExtraTime)
        {
            ui.SetDecreaseButtonInteractable(false);
        }
        else if (extraTime >= maxExtraTime || extraTime >= playerCoin)
        {
            ui.SetIncreaseButtonInteractable(false);
        }
        else
        {
            ui.SetDecreaseButtonInteractable(true);
            ui.SetIncreaseButtonInteractable(true);
        }

    }

    public int GetPrice(int extraTime)
    {
        int currentPrice = Mathf.CeilToInt(baseTimeCostPerSecond * (GameManager.instance.currentDay * 0.35f)) * extraTime;
        return currentPrice;
    }
}
